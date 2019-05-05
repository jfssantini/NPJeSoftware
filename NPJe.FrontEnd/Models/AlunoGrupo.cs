
namespace NPJe.FrontEnd.Models
{
    public class AlunoGrupo
    {
        public long Id { get; set; }

        public long IdAluno { get; set; }

        public Aluno Aluno { get; set; }

        public long IdGrupo { get; set; }

        public Grupo Grupo { get; set; }
    }
}