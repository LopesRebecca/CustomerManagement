using CustomerManagement.Infrastructure.Persistence.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace CustomerManagement.Infrastructure.Persistence
{
    public class NHibernateSessionFactory
    {
        public static ISessionFactory CriarSessionFactory(string connectionString)
        {
            return Fluently.Configure()
                .Database(
                    PostgreSQLConfiguration.Standard
                        .ConnectionString(connectionString)
                        .ShowSql()
                )
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<ClientMap>()
                )
                .BuildSessionFactory();
        }
    }
}
