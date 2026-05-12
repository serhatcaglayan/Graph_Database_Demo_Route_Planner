using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using RoutePlanner.API.Models;
using RoutePlanner.API.Services;

namespace RoutePlanner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IstasyonController : ControllerBase
    {
        private readonly Neo4jService _neo4jService;

        public IstasyonController(Neo4jService neo4jService)
        {
            _neo4jService = neo4jService;
        }

        // ── GET /api/istasyon — Tum istasyonlar + hat bilgisi ───
        [HttpGet]
        public async Task<IActionResult> GetIstasyonlar()
        {
            using var session = _neo4jService.Driver.AsyncSession();
            var cypher = @"
                MATCH (s:Istasyon)
                OPTIONAL MATCH (s)-[r:BAGLANTI]-()
                WITH s,
                     [x IN collect(DISTINCT r.hat) WHERE x IS NOT NULL] AS hatlar,
                     [x IN collect(DISTINCT r.tur) WHERE x IS NOT NULL] AS turlar
                RETURN s, hatlar, turlar";

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher);
                return await cursor.ToListAsync(record =>
                {
                    var node  = record["s"].As<INode>();
                    var hatlar = record["hatlar"].As<List<object>>()
                                    .Select(h => h?.ToString() ?? "").Distinct().ToList();
                    var turlar = record["turlar"].As<List<object>>()
                                    .Select(t => t?.ToString() ?? "").Distinct().ToList();
                    return new IstasyonDto
                    {
                        Id     = node.Properties.ContainsKey("id")   ? node.Properties["id"].As<string>()   : "",
                        Isim   = node.Properties["isim"].As<string>(),
                        Enlem  = node.Properties.ContainsKey("enlem")  ? node.Properties["enlem"].As<double>()  : 0,
                        Boylam = node.Properties.ContainsKey("boylam") ? node.Properties["boylam"].As<double>() : 0,
                        Hat    = node.Properties.ContainsKey("hat")    ? node.Properties["hat"].As<string>()    : "",
                        HatListesi = hatlar,
                        TurListesi = turlar
                    };
                });
            });

            return Ok(result);
        }

        // ── GET /api/istasyon/istatistik — En yogun 10 durak ───
        [HttpGet("istatistik")]
        public async Task<IActionResult> GetIstatistik()
        {
            using var session = _neo4jService.Driver.AsyncSession();
            var cypher = @"
                MATCH (l:Lokasyon)
                OPTIONAL MATCH (l)-[r:BAGLANTI]-()
                WITH l,
                     count(r) AS baglantiSayisi,
                     [x IN collect(DISTINCT r.hat) WHERE x IS NOT NULL] AS hatlar,
                     [x IN collect(DISTINCT r.tur) WHERE x IS NOT NULL] AS turlar
                ORDER BY baglantiSayisi DESC
                LIMIT 10
                RETURN l.isim AS isim, l.id AS id, baglantiSayisi, hatlar, turlar";

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher);
                return await cursor.ToListAsync(record => new
                {
                    Isim           = record["isim"].As<string>(),
                    Id             = record["id"].As<string>(),
                    BaglantiSayisi = Convert.ToInt32(record["baglantiSayisi"]),
                    Hatlar         = record["hatlar"].As<List<object>>().Select(h => h?.ToString() ?? "").ToList(),
                    Turlar         = record["turlar"].As<List<object>>().Select(t => t?.ToString() ?? "").ToList()
                });
            });

            return Ok(result);
        }
    }
}
