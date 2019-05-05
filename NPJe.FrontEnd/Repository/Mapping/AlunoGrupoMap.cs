using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AlunoGrupoMap : EntityTypeConfiguration<AlunoGrupo>
    {
        public AlunoGrupoMap()
        {
            ToTable("alunogrupo");

            HasRequired(x => x.Aluno)
                    .WithMany(x => x.Grupos)
                    .HasForeignKey(x => x.IdAluno);

            HasRequired(x => x.Grupo)
                    .WithMany(x => x.Alunos)
                    .HasForeignKey(x => x.IdGrupo);
        }
    }
}