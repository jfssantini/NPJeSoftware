using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;


namespace NPJe.FrontEnd.Repository.Mapping
{
    public class ResponsavelMap : EntityTypeConfiguration<Responsavel>
    {
        public ResponsavelMap()
        {
            ToTable("responsavel");

            HasRequired(x => x.Usuario)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuario);
        }
    }
}