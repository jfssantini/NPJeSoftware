using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class PastaMap : EntityTypeConfiguration<Pasta>
    {
        public PastaMap() 
        {
            ToTable("pasta");

            HasRequired(x => x.Cliente)
                    .WithMany()
                    .HasForeignKey(x => x.IdCliente);

            HasRequired(x => x.Grupo)
                    .WithMany()
                    .HasForeignKey(x => x.IdGrupo);

            HasOptional(x => x.UsuarioResponsavel)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioResponsavel);

            HasRequired(x => x.Assunto)
                    .WithMany()
                    .HasForeignKey(x => x.IdAssunto);

            HasRequired(x => x.UsuarioCriacao)
                    .WithMany()
                    .HasForeignKey(x => x.IdUsuarioCriacao);

            HasOptional(x => x.UsuarioExclusao)
                .WithMany()
                .HasForeignKey(x => x.IdUsuarioExclusao);
        }
    }
}