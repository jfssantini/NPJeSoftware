using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class TipoAtendimentoMap : EntityTypeConfiguration<TipoAtendimento>
    {
        public TipoAtendimentoMap()
        {
            ToTable("tipoatendimento");
        }
    }
}