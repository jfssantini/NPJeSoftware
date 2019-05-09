using NPJe.FrontEnd.Enums;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class AlunoEspecialidade
    {
        public AlunoEspecialidade()
        {
            Disponibilidades = new List<Disponibilidade>();
        }

        public long Id { get; set; }

        public long? IdAluno { get; set; }

        public Aluno Aluno { get; set; }

        public EspecialidadeEnum IdEspecialidade { get; set; }

        public List<Disponibilidade> Disponibilidades { get; set; }

        public long IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}