using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Mapping;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NPJe.FrontEnd.Repository.Context
{
    public class Contexto : DbContext
    {
        public Contexto() : base("name=NPJe")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        #region DbSets
        public DbSet<Agendamento> Agendamento { get; set; }
        public DbSet<Aluno> Aluno { get; set; }
        public DbSet<AlunoEspecialidade> AlunoEspecialidade { get; set; }
        public DbSet<AlunoGrupo> AlunoGrupo { get; set; }
        public DbSet<Assunto> Assunto { get; set; }
        public DbSet<Atendimento> Atendimento { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Disponibilidade> Disponibilidade { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
        public DbSet<Pasta> Pasta { get; set; }
        public DbSet<Processo> Processo { get; set; }
        public DbSet<Responsavel> Responsavel { get; set; }
        public DbSet<TipoAcao> TipoAcao { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Add(new PostgresqlConventions.AllCapsTableAndColumnConvention());
            modelBuilder.Conventions.Add(new PostgresqlConventions.AllCapsForeignKeyConvention());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<NotMappedTypeAttributeConvention>();
            modelBuilder.Properties<long>().Where(x => x.Name == "Id").Configure(x => x.IsKey());
            modelBuilder.Properties<int>().Where(x => x.Name == "Id").Configure(x => x.IsKey());

            #region Mapeamentos
            modelBuilder.Configurations.Add(new AlunoEspecialidadeMap());
            modelBuilder.Configurations.Add(new AgendamentoMap());
            modelBuilder.Configurations.Add(new AlunoGrupoMap());
            modelBuilder.Configurations.Add(new AlunoMap());
            modelBuilder.Configurations.Add(new AssuntoMap());
            modelBuilder.Configurations.Add(new AtendimentoMap());
            modelBuilder.Configurations.Add(new ClienteMap());
            modelBuilder.Configurations.Add(new DisponibilidadeMap());
            modelBuilder.Configurations.Add(new EnderecoMap());
            modelBuilder.Configurations.Add(new GrupoMap());
            modelBuilder.Configurations.Add(new PastaMap());
            modelBuilder.Configurations.Add(new ProcessoMap());
            modelBuilder.Configurations.Add(new ResponsavelMap());
            modelBuilder.Configurations.Add(new TipoAcaoMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            #endregion

            base.OnModelCreating(modelBuilder);

        }

    }

   
}