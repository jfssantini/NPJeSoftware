using System;

namespace NPJe.FrontEnd.Models
{
    public class Disponibilidade
    {
        public long Id { get; set; }

        public long IdAlunoEspecialidade { get; set; }

        public AlunoEspecialidade AlunoEspecialidade { get; set; }

        public DateTime DataHora { get; set; }
    }
}