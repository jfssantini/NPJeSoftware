using System;

namespace NPJe.FrontEnd.Dtos
{
    public class AgendamentoDto
    {
        public long Id { get; set; }

        public long? IdPasta { get; set; }

        public string Pasta { get; set; }

        public long? IdProcesso { get; set; }

        public string Processo { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataAgendamento { get; set; }

        public string DescricaoDataAgendamento { get; set; }

        public string Horario { get; set; }
    }
}