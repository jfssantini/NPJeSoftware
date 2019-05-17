using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Dtos
{
    public class ProcessoDto
    {
        public long Id { get; set; }

        public DateTime? Distribuicao { get; set; }

        public string DescricaoDistribuicao { get; set; }

        public string NumeroProcesso { get; set; }

        public long IdUsuario { get; set; }

        public string Usuario { get; set; }

        public long? IdCliente { get; set; }

        public string Cliente { get; set; }

        public long? IdGrupo { get; set; }

        public int NumeroGrupo { get; set; }

        public EspecialidadeEnum? IdEspecialidadeGrupo { get; set; }

        public string Grupo { get { return IdEspecialidadeGrupo.HasValue ? NumeroGrupo + " - " + IdEspecialidadeGrupo.Value.GetDescription() : ""; } set { } }

        public long? IdUsuarioResponsavel { get; set; }

        public string UsuarioResponsavel { get; set; }

        public long IdPasta { get; set; }

        public string Pasta { get; set; }

        public PoloEnum? IdPolo { get; set; }

        public SituacaoNpjEnum? IdSituacaoNpjAtual { get; set; }

        public SituacaoProjudiEnum? IdSituacaoProjudiAtual { get; set; }

        public long IdTipoAcao { get; set; }

        public string TipoAcao { get; set; }

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

        public string UsuarioCriacao { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public string DescricaoDataHoraCriacao { get { return DataHoraCriacao.ToString("dd/MM/yyyy HH:mm"); } set { } }
    }
}