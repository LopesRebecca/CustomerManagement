using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using FluentNHibernate.Mapping;

namespace CustomerManagement.Infrastructure.Persistence.Maps
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("cliente");

            Id(x => x.Id)
                .Column("id")
                .GeneratedBy.Identity();

            Map(x => x.Nome)
                .Column("nome")
                .Length(200)
                .Not.Nullable();

            Component(x => x.NumeroDocumento, m =>
            {
                m.Map(d => d.Valor)
                    .Column("numero_documento")
                    .Length(14)
                    .Not.Nullable();

                m.Map(d => d.Tipo)
                    .Column("tipo_documento")
                    .CustomType<TipoDeDocumento>()
                    .Not.Nullable();
            });

            Map(x => x.Ativo)
                .Column("ativo")
                .Not.Nullable();
        }
    }
}
