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
                .Column("id")
                .GeneratedBy.Increment();

            Map(x => x.Name)
                .Column("name")
                .Length(200)
                .Not.Nullable();

            Component(x => x.DocumentNumber, m =>
            {
                m.Map(d => d.Valor)
                    .Column("document_number")
                    .Length(14)
                    .Not.Nullable();

                m.Map(d => d.Tipo)
                    .Column("document_type")
                    .Not.Nullable();
            });

            Map(x => x.Active)
                .Column("active")
                .Not.Nullable();
        }
    }
}
