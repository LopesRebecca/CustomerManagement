using CustomerManagement.Domain.Entities;
using FluentNHibernate.Mapping;

namespace CustomerManagement.Infrastructure.Persistence.Maps
{
    public class ClientMap : ClassMap<ClientEntity>
    {
        public ClientMap()
        {
            Table("clients");

            Id(x => x.Id)
                .GeneratedBy.Increment();

            Map(x => x.FantasyName)
                .Column("fantasy_name")
                .Not.Nullable();


            Map(x => x.DocumentNumber.Valor)
                  .Column("document_valor")
                  .Not.Nullable();


            Map(x => x.Active)
                .Column("active")
                .Not.Nullable();
        }
    }
}
