using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AlunoMap : EntityTypeConfiguration<Aluno>
    {
        public AlunoMap()
        {
            ToTable("aluno");

            HasRequired(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario);

            HasOptional(x => x.Responsavel)
                .WithMany()
                .HasForeignKey(x => x.IdResponsavel);

            HasOptional(x => x.UsuarioExclusao)
                .WithMany()
                .HasForeignKey(x => x.IdUsuarioExclusao);
        }
    }
}