using NPJe.FrontEnd.Models;
using System.Data.Entity.ModelConfiguration;

namespace NPJe.FrontEnd.Repository.Mapping
{
    public class ClienteMap : EntityTypeConfiguration<Cliente>
    {
        public ClienteMap()
        {
            ToTable("cliente");

            HasOptional(x => x.Endereco)
                    .WithMany()
                    .HasForeignKey(x => x.IdEndereco);

            HasOptional(x => x.UsuarioExclusao)
                .WithMany()
                .HasForeignKey(x => x.IdUsuarioExclusao);
        }
    }
}