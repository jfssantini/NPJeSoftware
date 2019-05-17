using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AssuntoMap : EntityTypeConfiguration<Assunto>
    {
        public AssuntoMap()
        {
            ToTable("assunto");
        }
    }
}