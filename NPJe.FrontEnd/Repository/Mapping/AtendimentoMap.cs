using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AtendimentoMap : EntityTypeConfiguration<Atendimento>
    {
        public AtendimentoMap()
        {
            ToTable("atendimento");

            HasOptional(x => x.Pasta)
                    .WithMany(x => x.Atendimentos)
                    .HasForeignKey(x => x.IdPasta);

            HasRequired(x => x.UsuarioRegistro)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioRegistro);

            HasOptional(x => x.UsuarioExclusao)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioExclusao);

            HasRequired(x => x.Usuario)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuario);

            HasOptional(x => x.Processo)
                    .WithMany(x => x.Atendimentos)
                    .HasForeignKey(x => x.IdProcesso);
        }
    }
}