using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Dtos
{
    public class EspecialidadeDto
    {
        public EspecialidadeDto()
        {
            DisponibilidadeGrid = new List<DisponibilidadeDto>();
        }
        public long Id { get; set; }

        public long? IdAluno { get; set; }

        public EspecialidadeEnum IdEspecialidade { get; set; }

        public string DescricaoEspecialidade { get { return IdEspecialidade.GetDescription(); } set { } }

        public List<DisponibilidadeDto> DisponibilidadeGrid { get; set; }

        public long IdUsuario { get; set; }

        public List<string> Disponibilidades { get; set; }

        public string Disponibilidade { get { return string.Join(", ", Disponibilidades); } set { } }
    }
}