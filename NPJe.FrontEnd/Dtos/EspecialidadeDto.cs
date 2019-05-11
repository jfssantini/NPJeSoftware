using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPJe.FrontEnd.Dtos
{
    public class EspecialidadeDto
    {
        public EspecialidadeDto()
        {
            DisponibilidadeGrid = new List<HorarioDto>();
        }
        public long Id { get; set; }

        public long? IdAluno { get; set; }

        public EspecialidadeEnum IdEspecialidade { get; set; }

        public string DescricaoEspecialidade { get { return IdEspecialidade.GetDescription(); } set { } }

        public List<HorarioDto> DisponibilidadeGrid { get; set; }

        public long IdUsuario { get; set; }
    }
}