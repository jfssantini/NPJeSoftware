using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AlunoEspecialidadeMap : EntityTypeConfiguration<AlunoEspecialidade>
    {
        public AlunoEspecialidadeMap()
        {
            ToTable("alunoespecialidade");

            HasOptional(x => x.Aluno)
                    .WithMany(x => x.Especialidades)
                    .HasForeignKey(x => x.IdAluno);

            HasRequired(x => x.Usuario)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuario);
        }
    }
}