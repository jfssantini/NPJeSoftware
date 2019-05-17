using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class Processo
    {
        public Processo()
        {
            Atendimentos = new List<Atendimento>();
            Agendamentos = new List<Agendamento>();
        }

        public long Id { get; set; }

        public DateTime Distribuicao { get; set; }

        public long IdPasta { get; set; }

        public Pasta Pasta { get; set; }

        public string NumeroProcesso { get; set; }

        public PoloEnum? IdPolo { get; set; }

        public SituacaoNpjEnum? IdSituacaoNpjAtual { get; set; }

        public SituacaoProjudiEnum? IdSituacaoProjudiAtual { get; set; }

        public SituacaoAtendimentoEnum? IdSituacaoAtendimentoAtual { get; set; }

        public long IdTipoAcao { get; set; }

        public TipoAcao TipoAcao { get; set; }

        public bool Status { get; set; }

        public decimal ExpectativaValorCausa { get; set; }

        public decimal PercentualHonorarios { get; set; }

	    public decimal ValorHonorarios { get; set; }

        public string SegmentoJudiciario { get; set; }

        public string Comarca { get; set; }

        public string Vara { get; set; }

        public string Tribunal { get; set; }

        public string AnotacoesGerais { get; set; }

        public long IdUsuarioCriacao { get; set; }

        public Usuario UsuarioCriacao { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public List<Atendimento> Atendimentos { get; set; }

        public List<Agendamento> Agendamentos { get; set; }
    }
}