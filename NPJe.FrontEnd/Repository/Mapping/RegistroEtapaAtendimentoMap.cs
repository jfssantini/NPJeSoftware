using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class RegistroEtapaAtendimentoMap : EntityTypeConfiguration<RegistroEtapaAtendimento>
    {
        public RegistroEtapaAtendimentoMap()
        {
            ToTable("registrortapaatendimento");

            HasRequired(x => x.UsuarioRegistro)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioRegistro);

            HasOptional(x => x.Atendimento)
                    .WithMany(x => x.EtapasAtendimento)
                    .HasForeignKey(x => x.IdAtendimento);
        }
    }
}