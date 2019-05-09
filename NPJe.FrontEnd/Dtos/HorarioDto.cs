using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPJe.FrontEnd.Dtos
{
    public class HorarioDto
    {
        public long Id { get; set; }

        public long IdAlunoEspecialidade { get; set; }

        public DiaSemanaEnum IdDiaSemana { get; set; }

        public string DiaSemanaDescricao { get { return IdDiaSemana.GetDescription(); } set { } }

        public string HorarioInicio { get; set; }

        public string HorarioFim { get; set; }
    }
}