using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;

namespace NPJe.FrontEnd.Dtos
{
    public class DisponibilidadeDto
    {
        public long Id { get; set; }

        public long IdAlunoEspecialidade { get; set; }

        public DiaSemanaEnum IdDiaSemana { get; set; }

        public string DescricaoDiaSemana { get { return IdDiaSemana.GetDescription(); } set { } }

        public string HorarioInicio { get; set; }

        public string HorarioFim { get; set; }
    }
}