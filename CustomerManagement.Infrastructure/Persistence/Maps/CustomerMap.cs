using CustomerManagement.Domain.Entities;
using FluentNHibernate.Mapping;

namespace CustomerManagement.Infrastructure.Persistence.Maps
{
    public class CustomerMap : ClassMap<CustomerEntity>
    {
        public CustomerMap()
        {
            Table("Customer");

            Id(x => x.Id)
                .Column("id")
                .GeneratedBy.Identity();

            Map(x => x.Name)
                .Column("name")
                .Length(200)
                .Not.Nullable();

            Component(x => x.DocumentNumber, m =>
            {
                m.Map(d => d.Value)
                    .Column("document_number")
                    .Length(14)
                    .Not.Nullable();

                m.Map(d => d.Type)
                    .Column("document_type")
                    .Not.Nullable();
            });

            Map(x => x.Active)
                .Column("active")
                .Not.Nullable();
        }
    }
}
