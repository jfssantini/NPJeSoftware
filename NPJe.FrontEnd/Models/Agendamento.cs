using System;

namespace NPJe.FrontEnd.Models
{
    public class Agendamento
    {
        public long Id { get; set; }

        public long? IdPasta { get; set; }

        public Pasta Pasta { get; set; }

        public long? IdProcesso { get; set; }

        public Processo Processo { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataAgendamento { get; set; }

        public string Horario { get; set; }

        public bool Concluido { get; set; }

        /*****Para Controle do sistema*****/
        public bool Temporario { get; set; }

        public bool Esconder { get; set; }

        public bool Excluir { get; set; }

        public long IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}