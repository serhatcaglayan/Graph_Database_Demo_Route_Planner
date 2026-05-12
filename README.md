# 📍 Neo4j Tabanlı Şehir Rota Planlayıcı (Adana Transit)

Bu proje, Adana şehri için tasarlanmış, toplu taşıma (metro ve otobüs) hatlarını kullanarak başlangıç noktasından hedef noktaya en uygun rotaları hesaplayan bir yönlendirme (routing) uygulamasıdır. 

Geleneksel ilişkisel veritabanlarının (SQL) rota ve ağ problemlerindeki performans sorunlarını ve karmaşıklıklarını aşmak amacıyla projede tamamen **Çizge Veritabanı (Graph Database)** teknolojisi kullanılmıştır.

---

## 🚀 Özellikler

* **En Hızlı Rota:** İki istasyon arasındaki en kısa süreli rotayı Dijkstra algoritması ile hesaplar.
* **En Ucuz Rota:** Zamanı gözetmeksizin, biniş maliyetinin en düşük olduğu rotayı bulur.
* **Sadece Metro:** Otobüs hatlarını devre dışı bırakarak yalnızca metro ile gidilebilecek güzergahı hesaplar.
* **Dinamik Harita Görselleştirme:** Kullanıcı rotasını karanlık tema (dark mode) destekli Leaflet.js haritası üzerinde istasyon pinleri ve yön çizgileri ile çizer.
* **İstatistik Paneli:** Ağa en çok bağlanan (hub) "En Yoğun İstasyonları" canlı olarak listeler.

---

## 🛠️ Kullanılan Teknolojiler

* **Veritabanı:** Neo4j (Graph Database), Cypher Query Language (CQL)
* **Graf Algoritmaları:** APOC (apoc.algo.dijkstra)
* **Backend:** C# / .NET 8 Web API
* **Neo4j Sürücüsü:** Neo4j.Driver
* **Frontend:** HTML5, CSS3, Vanilla JavaScript
* **Harita Kütüphanesi:** Leaflet.js

---

## 🗂️ Veri Modeli ve Graf Mimarisi

Sistem aşağıdaki düğüm ve ilişkiler üzerine inşa edilmiştir:
- **Düğümler (Nodes):** `Istasyon` ve `Lokasyon`. (Özellikleri: `id`, `isim`, `enlem`, `boylam`)
- **İlişkiler (Relationships):** `BAGLANTI` (Özellikleri: `mesafe`, `sure`, `maliyet`, `tur`, `hat`)

Graph mimarisinin sunduğu **Index-Free Adjacency** sayesinde, SQL üzerinde karmaşık `JOIN` işlemlerine gerek kalmadan istasyonlar arasındaki bağlantılar milisaniyeler içerisinde hesaplanır.

---

## 💻 Kurulum ve Çalıştırma

### 1. Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Neo4j Desktop](https://neo4j.com/download/) veya [AuraDB](https://neo4j.com/cloud/platform/aura-graph-database/)

### 2. Veritabanını Hazırlama
1. Neo4j üzerinde yeni bir veritabanı oluşturun ve çalıştırın.
2. Neo4j Browser'ı açın ve proje dizinindeki `seed_data.cypher` dosyasının içindeki tüm Cypher komutlarını kopyalayıp çalıştırarak graf verilerinizi yükleyin.
3. APOC eklentisinin Neo4j üzerinde aktif olduğundan emin olun.

### 3. Backend Ayarları
1. `RoutePlanner.API` klasörü içindeki `appsettings.json` veya `appsettings.Development.json` dosyasını açın.
2. Neo4j bağlantı bilgilerinizi kendi veritabanı ayarlarınıza göre güncelleyin:
   ```json
   "Neo4j": {
     "Uri": "bolt://localhost:7687",
     "User": "neo4j",
     "Password": "sizin_sifreniz"
   }
   ```

### 4. Projeyi Başlatma
Terminal üzerinde `RoutePlanner.API` klasörüne gidip şu komutu çalıştırın:
```bash
dotnet run
```
API çalışmaya başladıktan sonra, `RoutePlanner.API/wwwroot/index.html` dosyasını tarayıcınızda (Örn: Live Server eklentisiyle) açarak uygulamayı kullanmaya başlayabilirsiniz.

---

## 📄 Raporlama

Proje mimarisi ve Cypher sorgularının detaylı açıklamaları proje kök dizininde bulunan **`ProjeRaporu.txt`** dosyasında detaylandırılmıştır. Daha derin bir teknik okuma için lütfen ilgili belgeyi inceleyin.
