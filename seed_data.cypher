// =========================================================
// ADANA ROTA PLANLAYICI — SEED DATA v5
// Metro: L1-L50 ardisik (M1 hatti)
// Hat-A: L1 - L4 - L7 - L10 - L13 - L16 - L19 - L22 - L25 - L28 (10 durak)
// Hat-B: L2 - L5 - L8 - L11 - L14 - L17 - L20 - L23 - L26 - L29 (10 durak)
// Hat-C: L3 - L6 - L9 - L12 - L15 - L18 - L21 - L24 - L27 - L30 (10 durak)
// =========================================================

// 1. TEMIZLIK
MATCH (n) DETACH DELETE n;

// 2. KISITLAMALAR
CREATE CONSTRAINT lokasyon_id_unique IF NOT EXISTS FOR (l:Lokasyon) REQUIRE l.id IS UNIQUE;

// 3. TÜM LOKASYONLAR (L1-L50)
UNWIND [
  // --- Kuzey / Cukurova Bolgesi ---
  {id:'L1',  isim:'Cukurova Yurdu',          enlem:37.0621, boylam:35.3108},
  {id:'L2',  isim:'Guzide Sokak',             enlem:37.0589, boylam:35.3412},
  {id:'L3',  isim:'Resatbey Mahallesi',       enlem:37.0554, boylam:35.3221},
  {id:'L4',  isim:'Turhan Cemal Beriker Blv', enlem:37.0602, boylam:35.2978},
  {id:'L5',  isim:'Demiryolu Caddesi',        enlem:37.0471, boylam:35.3551},

  // --- Kuzeybati / Seyhan Kenari ---
  {id:'L6',  isim:'Ozler Caddesi',            enlem:37.0512, boylam:35.2843},
  {id:'L7',  isim:'Inonu Caddesi',            enlem:37.0438, boylam:35.2765},
  {id:'L8',  isim:'Ziya Pasa Bulvari',        enlem:37.0388, boylam:35.3023},
  {id:'L9',  isim:'Seyhan Koprusu',           enlem:37.0401, boylam:35.3198},
  {id:'L10', isim:'Barajyolu Koprusu',         enlem:37.0350, boylam:35.2912},

  // --- Merkez / Sehir Ici ---
  {id:'L11', isim:'Turgut Ozal Bulvari',      enlem:37.0198, boylam:35.3312},
  {id:'L12', isim:'Cemalpasa Caddesi',        enlem:37.0145, boylam:35.3089},
  {id:'L13', isim:'Cumhuriyet Meydani',       enlem:37.0077, boylam:35.3201},
  {id:'L14', isim:'Ataturk Parki',            enlem:37.0112, boylam:35.3402},
  {id:'L15', isim:'Hukumet Konagi',           enlem:37.0055, boylam:35.2988},

  // --- Kucuksaat / Eski Merkez ---
  {id:'L16', isim:'Merkez Posta',             enlem:37.0021, boylam:35.3145},
  {id:'L17', isim:'Kucuksaat Meydani',        enlem:36.9987, boylam:35.3288},
  {id:'L18', isim:'Adnan Menderes Bulvari',   enlem:36.9944, boylam:35.3512},
  {id:'L19', isim:'Akinci Turk Caddesi',      enlem:36.9978, boylam:35.2891},
  {id:'L20', isim:'Ataturk Caddesi',          enlem:36.9932, boylam:35.3078},

  // --- Dogu / Yuregir Bolgesi ---
  {id:'L21', isim:'Kazimpasa Sokak',          enlem:37.0089, boylam:35.3701},
  {id:'L22', isim:'Mithatpasa Caddesi',       enlem:37.0022, boylam:35.3834},
  {id:'L23', isim:'Barbaros Bulvari',         enlem:36.9965, boylam:35.3745},
  {id:'L24', isim:'Yeni Baris Caddesi',       enlem:36.9901, boylam:35.3621},
  {id:'L25', isim:'Optimum AVM',              enlem:36.9867, boylam:35.3489},

  // --- Guney / Havaalani Bolgesi ---
  {id:'L26', isim:'Havaalani Bulvari',        enlem:36.9921, boylam:35.2812},
  {id:'L27', isim:'Adana Havaalani',          enlem:36.9834, boylam:35.2798},
  {id:'L28', isim:'Yesiloba Caddesi',         enlem:36.9789, boylam:35.2945},
  {id:'L29', isim:'Fevzipasa Bulvari',        enlem:36.9745, boylam:35.3122},
  {id:'L30', isim:'Kenan Evren Bulvari',      enlem:36.9712, boylam:35.3298},

  // --- Guneydogu / Yuregir Ic ---
  {id:'L31', isim:'Namik Kemal Caddesi',      enlem:36.9981, boylam:35.4012},
  {id:'L32', isim:'Yuregir Bulvari',          enlem:36.9912, boylam:35.4145},
  {id:'L33', isim:'Kozan Yolu Caddesi',       enlem:36.9848, boylam:35.4278},
  {id:'L34', isim:'Sehit Temel Sokak',        enlem:36.9771, boylam:35.4089},
  {id:'L35', isim:'Adana Otogar',             enlem:36.9705, boylam:35.3812},

  // --- Bati / Seyhan Bati ---
  {id:'L36', isim:'Guneykent Caddesi',        enlem:37.0312, boylam:35.2612},
  {id:'L37', isim:'Pinar Mahallesi',          enlem:37.0245, boylam:35.2501},
  {id:'L38', isim:'Dumlupinar Bulvari',       enlem:37.0178, boylam:35.2678},
  {id:'L39', isim:'Gazipasa Caddesi',         enlem:37.0098, boylam:35.2589},
  {id:'L40', isim:'Mavi Bulvar',              enlem:37.0034, boylam:35.2712},

  // --- Guneybaati / Sariham Bolgesi ---
  {id:'L41', isim:'Seyhanoglu Sokak',         enlem:36.9878, boylam:35.2934},
  {id:'L42', isim:'Balcali Yolu',             enlem:36.9801, boylam:35.3389},
  {id:'L43', isim:'Universite Bulvari',       enlem:36.9734, boylam:35.3567},
  {id:'L44', isim:'Cukurova Universitesi',    enlem:36.9667, boylam:35.3712},
  {id:'L45', isim:'Yunus Emre Caddesi',       enlem:36.9623, boylam:35.3512},

  // --- Uzak Guney / Periferiler ---
  {id:'L46', isim:'Bahcelievler Bulvari',     enlem:36.9578, boylam:35.3145},
  {id:'L47', isim:'Kurttepe Yolu',            enlem:36.9534, boylam:35.2901},
  {id:'L48', isim:'Guvenbulvar Caddesi',      enlem:36.9689, boylam:35.2645},
  {id:'L49', isim:'Saricam Merkez',           enlem:36.9445, boylam:35.3234},
  {id:'L50', isim:'Akincilar Son Durak',      enlem:36.9512, boylam:35.3678}
] AS row
CREATE (l:Lokasyon:Istasyon {
    id:     row.id,
    isim:   row.isim,
    enlem:  row.enlem,
    boylam: row.boylam,
    tip:    'Genel'
});

// 4. METRO BAGLANTILARI (L1-L50 ardisik)
MATCH (l1:Istasyon), (l2:Istasyon)
WHERE toInteger(substring(l1.id, 1)) >= 1
  AND toInteger(substring(l1.id, 1)) <= 49
  AND toInteger(substring(l2.id, 1)) = toInteger(substring(l1.id, 1)) + 1
CREATE (l1)-[:BAGLANTI {mesafe: 1.5, sure: 3, maliyet: 25.0, tur: 'metro', hat: 'M1'}]->(l2),
       (l2)-[:BAGLANTI {mesafe: 1.5, sure: 3, maliyet: 25.0, tur: 'metro', hat: 'M1'}]->(l1);

// 5. OTOBUS DURAKLARINI ISARETLE
MATCH (l:Lokasyon)
WHERE toInteger(substring(l.id, 1)) <= 30
SET l:OtobusDuragi;

// 6. HAT-A (L1, L4, L7, L10, L13, L16, L19, L22, L25, L28)
MATCH (l1:Istasyon), (l2:Istasyon)
WHERE toInteger(substring(l1.id, 1)) IN [1, 4, 7, 10, 13, 16, 19, 22, 25]
  AND toInteger(substring(l2.id, 1)) = toInteger(substring(l1.id, 1)) + 3
CREATE (l1)-[:BAGLANTI {mesafe: 4.0, sure: 10, maliyet: 12.0, tur: 'otobus', hat: 'Hat-A'}]->(l2),
       (l2)-[:BAGLANTI {mesafe: 4.0, sure: 10, maliyet: 12.0, tur: 'otobus', hat: 'Hat-A'}]->(l1);

// 7. HAT-B (L2, L5, L8, L11, L14, L17, L20, L23, L26, L29)
MATCH (l1:Istasyon), (l2:Istasyon)
WHERE toInteger(substring(l1.id, 1)) IN [2, 5, 8, 11, 14, 17, 20, 23, 26]
  AND toInteger(substring(l2.id, 1)) = toInteger(substring(l1.id, 1)) + 3
CREATE (l1)-[:BAGLANTI {mesafe: 4.0, sure: 10, maliyet: 12.0, tur: 'otobus', hat: 'Hat-B'}]->(l2),
       (l2)-[:BAGLANTI {mesafe: 4.0, sure: 10, maliyet: 12.0, tur: 'otobus', hat: 'Hat-B'}]->(l1);

// 8. HAT-C (L3, L6, L9, L12, L15, L18, L21, L24, L27, L30)
MATCH (l1:Istasyon), (l2:Istasyon)
WHERE toInteger(substring(l1.id, 1)) IN [3, 6, 9, 12, 15, 18, 21, 24, 27]
  AND toInteger(substring(l2.id, 1)) = toInteger(substring(l1.id, 1)) + 3
CREATE (l1)-[:BAGLANTI {mesafe: 4.0, sure: 10, maliyet: 12.0, tur: 'otobus', hat: 'Hat-C'}]->(l2),
       (l2)-[:BAGLANTI {mesafe: 4.0, sure: 10, maliyet: 12.0, tur: 'otobus', hat: 'Hat-C'}]->(l1);
