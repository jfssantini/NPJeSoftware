using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AgendamentoMap : EntityTypeConfiguration<Agendamento>
    {
        public AgendamentoMap()
        {
            ToTable("agendamento");

            HasOptional(x => x.Pasta)
                   .WithMany(x => x.Agendamentos)
                   .HasForeignKey(x => x.IdPasta);

            HasOptional(x => x.Processo)
                   .WithMany(x => x.Agendamentos)
                   .HasForeignKey(x => x.IdProcesso);

            HasRequired(x => x.Usuario)
                   .WithMany()
                   .HasForeignKey(x => x.IdUsuario);
        }
    }
}