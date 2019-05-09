using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Models
{
    public class Disponibilidade
    {
        public long Id { get; set; }

        public long IdAlunoEspecialidade { get; set; }

        public AlunoEspecialidade AlunoEspecialidade { get; set; }

        public DiaSemanaEnum IdDiaSemana { get; set; }

        public string HorarioInicio { get; set; }

        public string HorarioFim { get; set; }
    }
}