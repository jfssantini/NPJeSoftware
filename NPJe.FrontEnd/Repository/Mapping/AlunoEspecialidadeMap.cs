using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AlunoEspecialidadeMap : EntityTypeConfiguration<AlunoEspecialidade>
    {
        public AlunoEspecialidadeMap()
        {
            ToTable("alunoespecialidade");

            HasRequired(x => x.Aluno)
                    .WithMany(x => x.Especialidades)
                    .HasForeignKey(x => x.IdAluno);
        }
    }
}