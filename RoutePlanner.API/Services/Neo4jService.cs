using Neo4j.Driver;

namespace RoutePlanner.API.Services
{
    public class Neo4jService : IDisposable
    {
        private readonly IDriver _driver;

        public Neo4jService(IConfiguration configuration)
        {
            // appsettings.json'daki bilgileri oku
            var uri = configuration["Neo4j:Uri"];
            var user = configuration["Neo4j:User"];
            var password = configuration["Neo4j:Password"];

            // Neo4j Driver'ı oluştur
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        // Sorgu çalıştırmak için bu driver'ı kullanacağız
        public IDriver Driver => _driver;

        // Uygulama kapandığında bağlantıyı güvenli bir şekilde kapat
        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
