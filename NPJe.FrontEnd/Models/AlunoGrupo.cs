
namespace NPJe.FrontEnd.Models
{
    public class AlunoGrupo
    {
        public long Id { get; set; }

        public long? IdAluno { get; set; }

        public Aluno Aluno { get; set; }

        public long IdGrupo { get; set; }

        public Grupo Grupo { get; set; }

        public long IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        public bool Temporario { get; set; }

        public bool Esconder { get; set; }

        public bool Excluir { get; set; }
    }
}