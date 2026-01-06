using CustomerManagement.Domain.Entities;
using FluentNHibernate.Mapping;

namespace CustomerManagement.Infrastructure.Persistence.Maps
{
    public class ClientMap : ClassMap<ClientEntity>
    {
        public ClientMap()
        {
            Table("client");

            Id(x => x.Id)
                .GeneratedBy.Increment();

            Map(x => x.Name)
                .Column("name")
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
