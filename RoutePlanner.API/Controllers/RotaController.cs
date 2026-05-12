using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using RoutePlanner.API.Models;
using RoutePlanner.API.Services;

namespace RoutePlanner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RotaController : ControllerBase
    {
        private readonly Neo4jService _neo4jService;

        public RotaController(Neo4jService neo4jService)
        {
            _neo4jService = neo4jService;
        }

        // ── Yardimci: IPath -> RotaAdimDto listesi ──────────────
        private static List<RotaAdimDto> ParsePath(IPath path)
        {
            var nodes = path.Nodes.ToList();
            var rels  = path.Relationships.ToList();
            var adimlar = new List<RotaAdimDto>();

            for (int i = 0; i < rels.Count; i++)
            {
                var node = nodes[i + 1];
                var rel  = rels[i];
                adimlar.Add(new RotaAdimDto
                {
                    Isim    = node.Properties["isim"].As<string>(),
                    Tur     = rel.Properties["tur"].As<string>(),
                    Hat     = rel.Properties.ContainsKey("hat") ? rel.Properties["hat"].As<string>() : "",
                    Sure    = Convert.ToDouble(rel.Properties["sure"]),
                    Mesafe  = Convert.ToDouble(rel.Properties["mesafe"]),
                    Maliyet = Convert.ToDouble(rel.Properties["maliyet"])
                });
            }
            return adimlar;
        }

        // ── GET /api/rota/planla — En Kisa (durak sayisi) ───────
        [HttpGet("planla")]
        public async Task<IActionResult> Planla(string baslangic, string hedef)
        {
            using var session = _neo4jService.Driver.AsyncSession();
            var cypher = @"
                MATCH (s:Lokasyon {isim: $baslangic}), (e:Lokasyon {isim: $hedef})
                CALL apoc.algo.dijkstra(s, e, 'BAGLANTI', 'sure') YIELD path, weight AS toplamSure
                RETURN path,
                       toplamSure,
                       reduce(x=0.0, r IN relationships(path) | x+r.mesafe)  AS toplamMesafe,
                       reduce(x=0.0, r IN relationships(path) | x+r.maliyet) AS toplamMaliyet";

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, new { baslangic, hedef });
                if (!await cursor.FetchAsync()) return null;
                var rec  = cursor.Current;
                var path = rec["path"].As<IPath>();
                return BuildResponse(baslangic, hedef, "hizli", path, rec, null);
            });

            if (result == null) return NotFound("Rota bulunamadi.");
            return Ok(result);
        }

        // ── GET /api/rota/ucuz — En Ucuz (maliyet bazli) ────────
        [HttpGet("ucuz")]
        public async Task<IActionResult> Ucuz(string baslangic, string hedef)
        {
            using var session = _neo4jService.Driver.AsyncSession();
            var cypher = @"
                MATCH (s:Lokasyon {isim: $baslangic}), (e:Lokasyon {isim: $hedef})
                CALL apoc.algo.dijkstra(s, e, 'BAGLANTI', 'maliyet') YIELD path, weight AS toplamMaliyet
                RETURN path,
                       toplamMaliyet,
                       reduce(x=0.0, r IN relationships(path) | x+r.sure)    AS toplamSure,
                       reduce(x=0.0, r IN relationships(path) | x+r.mesafe)  AS toplamMesafe";

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, new { baslangic, hedef });
                if (!await cursor.FetchAsync()) return null;
                var rec  = cursor.Current;
                var path = rec["path"].As<IPath>();
                return BuildResponse(baslangic, hedef, "ucuz", path, rec, null);
            });

            if (result == null) return NotFound("Ucuz rota bulunamadi.");
            return Ok(result);
        }

        // ── GET /api/rota/metro — Sadece Metro ──────────────────
        [HttpGet("metro")]
        public async Task<IActionResult> SadeceMetro(string baslangic, string hedef)
        {
            using var session = _neo4jService.Driver.AsyncSession();
            var cypher = @"
                MATCH (s:Lokasyon {isim: $baslangic}), (e:Lokasyon {isim: $hedef})
                MATCH path = shortestPath((s)-[rels:BAGLANTI*..60]-(e))
                WHERE ALL(r IN rels WHERE r.tur = 'metro')
                RETURN path,
                       reduce(x=0.0, r IN relationships(path) | x+r.sure)    AS toplamSure,
                       reduce(x=0.0, r IN relationships(path) | x+r.mesafe)  AS toplamMesafe,
                       reduce(x=0.0, r IN relationships(path) | x+r.maliyet) AS toplamMaliyet";

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, new { baslangic, hedef });
                if (!await cursor.FetchAsync()) return null;
                var rec  = cursor.Current;
                var path = rec["path"].As<IPath>();
                return BuildResponse(baslangic, hedef, "metro", path, rec, null);
            });

            if (result == null) return NotFound("Sadece metro ile bu rota mevcut degil.");
            return Ok(result);
        }

        // ── GET /api/rota/aktarmali?max=N — Aktarma Limitli ─────
        [HttpGet("aktarmali")]
        public async Task<IActionResult> AktarmaLimitli(string baslangic, string hedef, int max = 1)
        {
            using var session = _neo4jService.Driver.AsyncSession();
            var cypher = @"
                MATCH (s:Lokasyon {isim: $baslangic}), (e:Lokasyon {isim: $hedef})
                CALL {
                    WITH s, e
                    MATCH p1 = shortestPath((s)-[:BAGLANTI*..60]-(e))
                    RETURN p1 AS path
                    UNION
                    WITH s, e
                    MATCH p2 = shortestPath((s)-[rels:BAGLANTI*..60]-(e))
                    WHERE ALL(r IN rels WHERE r.tur = 'metro')
                    RETURN p2 AS path
                }
                WITH path,
                     [r IN relationships(path) | r.tur] AS turlar,
                     reduce(x=0.0, r IN relationships(path) | x+r.sure)    AS toplamSure,
                     reduce(x=0.0, r IN relationships(path) | x+r.mesafe)  AS toplamMesafe,
                     reduce(x=0.0, r IN relationships(path) | x+r.maliyet) AS toplamMaliyet
                WITH path, turlar, toplamSure, toplamMesafe, toplamMaliyet,
                     reduce(a=0, i IN range(1, size(turlar)-1) |
                       CASE WHEN turlar[i] <> turlar[i-1] THEN a+1 ELSE a END) AS aktarmaSayisi
                WHERE aktarmaSayisi <= $max
                RETURN path, toplamSure, toplamMesafe, toplamMaliyet, aktarmaSayisi
                ORDER BY toplamMaliyet ASC, length(path) ASC
                LIMIT 1";

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, new { baslangic, hedef, max });
                if (!await cursor.FetchAsync()) return null;
                var rec  = cursor.Current;
                var path = rec["path"].As<IPath>();
                var aktarma = Convert.ToInt32(rec["aktarmaSayisi"]);
                return BuildResponse(baslangic, hedef, "aktarmali", path, rec, aktarma);
            });

            if (result == null) return NotFound($"Max {max} aktarmayla rota bulunamadi.");
            return Ok(result);
        }

        // ── Ortak response builder ───────────────────────────────
        private object BuildResponse(string baslangic, string hedef, string mod,
                                      IPath path, IRecord rec, int? aktarmaSayisi)
        {
            var adimlar = ParsePath(path);
            return new
            {
                Baslangic     = baslangic,
                Hedef         = hedef,
                Mod           = mod,
                AktarmaSayisi = aktarmaSayisi,
                ToplamSure    = Convert.ToDouble(rec["toplamSure"]),
                ToplamMesafe  = Convert.ToDouble(rec["toplamMesafe"]),
                ToplamMaliyet = Convert.ToDouble(rec["toplamMaliyet"]),
                Rota          = adimlar
            };
        }
    }
}
