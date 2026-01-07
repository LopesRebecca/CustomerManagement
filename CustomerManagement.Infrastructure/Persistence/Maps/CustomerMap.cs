using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using System.Reflection.Metadata;

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
                    .CustomType<DocumentType>()
                    .Not.Nullable();
            });

            Map(x => x.Active)
                .Column("active")
                .Not.Nullable();
        }
    }
}
