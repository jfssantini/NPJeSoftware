using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class DisponibilidadeMap : EntityTypeConfiguration<Disponibilidade>
    {
        public DisponibilidadeMap()
        {
            ToTable("disponibilidade");

            HasRequired(x => x.AlunoEspecialidade)
                    .WithMany(x => x.Disponibilidades)
                    .HasForeignKey(x => x.IdAlunoEspecialidade);
        }
    }
}