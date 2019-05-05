using NPJe.FrontEnd.Enums;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class Grupo
    {
        public Grupo()
        {
            Alunos = new List<AlunoGrupo>();
        }

        public long Id { get; set; }

        public string Numero { get; set; }

        public EspecialidadeEnum IdEspecialidade { get; set; }

        public string Observacoes { get; set; }

        public List<AlunoGrupo> Alunos { get; set; }
    }
}