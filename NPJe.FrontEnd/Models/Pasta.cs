using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class Pasta
    {
        public Pasta()
        {
            Atendimentos = new List<Atendimento>();
            Agendamentos = new List<Agendamento>();
        }

        public long Id { get; set; }

        public long IdCliente { get; set; }

        public Cliente Cliente { get; set; }

        public long IdGrupo { get; set; }

        public Grupo Grupo { get; set; }

        public long? IdUsuarioResponsavel { get; set; }

        public Usuario UsuarioResponsavel { get; set; }

        public SituacaoNpjEnum? IdSituacaoNpjAtual { get; set; }

        public SituacaoProjudiEnum? IdSituacaoProjudiAtual { get; set; }

        public SituacaoAtendimentoEnum? IdSituacaoAtendimentoAtual { get; set; }

        public long IdAssunto { get; set; }

        public Assunto Assunto { get; set; }

        public bool Concluido { get; set; }

        public long IdUsuarioCriacao { get; set; }

        public Usuario UsuarioCriacao { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public long? IdUsuarioExclusao { get; set; }

        public Usuario UsuarioExclusao { get; set; }

        public DateTime? DataHoraExclusao { get; set; }

        public DateTime DataHoraAlteracao { get; set; }

        public List<Atendimento> Atendimentos { get; set; }

        public List<Agendamento> Agendamentos { get; set; }
    }
}