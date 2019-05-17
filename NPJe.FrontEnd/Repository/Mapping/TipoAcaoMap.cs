using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class TipoAcaoMap : EntityTypeConfiguration<TipoAcao>
    {
        public TipoAcaoMap()
        {
            ToTable("tipoacao");
        }
    }
}