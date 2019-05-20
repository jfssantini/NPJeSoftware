using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class ProcessoMap : EntityTypeConfiguration<Processo>
    {
        public ProcessoMap()
        {
            ToTable("processo");

            HasRequired(x => x.Pasta)
                    .WithMany()
                    .HasForeignKey(x => x.IdPasta);

            HasRequired(x => x.TipoAcao)
                    .WithMany()
                    .HasForeignKey(x => x.IdTipoAcao);

            HasRequired(x => x.UsuarioCriacao)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioCriacao);

            HasOptional(x => x.UsuarioExclusao)
                .WithMany()
                .HasForeignKey(x => x.IdUsuarioExclusao);
        }
    }
}