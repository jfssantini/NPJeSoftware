using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class AtendimentoMap : EntityTypeConfiguration<Atendimento>
    {
        public AtendimentoMap() 
        {
            ToTable("atendimento");

            HasRequired(x => x.Cliente)
                    .WithMany()
                    .HasForeignKey(x => x.IdCliente);

            HasRequired(x => x.Grupo)
                    .WithMany()
                    .HasForeignKey(x => x.IdGrupo);

            HasRequired(x => x.UsuarioCriacao)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioCriacao);

            HasRequired(x => x.UsuarioResponsavel)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioResponsavel);

            HasOptional(x => x.TipoAtendimento)
                    .WithMany()
                    .HasForeignKey(x => x.IdTipoAtendimento);
        }
    }
}