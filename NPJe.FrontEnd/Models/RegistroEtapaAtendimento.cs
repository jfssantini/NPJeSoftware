using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Models
{
    public class RegistroEtapaAtendimento
    {
        public long Id { get; set; }

        public long? IdAtendimento { get; set; }

        public Atendimento Atendimento { get; set; }

        public EtapaAtendimentoEnum IdEtapaAtendimento { get; set; }

        public long IdUsuarioRegistro { get; set; }

        public Usuario UsuarioRegistro { get; set; }

        public DateTime DataHora { get; set; }
    }
}