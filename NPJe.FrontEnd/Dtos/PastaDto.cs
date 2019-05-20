using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Dtos
{
    public class PastaDto
    {
        public long Id { get; set; }

        public long IdCliente { get; set; }

        public string Cliente { get; set; }

        public long IdGrupo { get; set; }

        public int NumeroGrupo { get; set; }

        public EspecialidadeEnum? IdEspecialidadeGrupo { get; set; }

        public string Grupo { get { return IdEspecialidadeGrupo.HasValue ? NumeroGrupo + " - " + IdEspecialidadeGrupo.Value.GetDescription() : ""; } set { } }

        public long? IdUsuarioResponsavel { get; set; }

        public string UsuarioResponsavel { get; set; }

        public SituacaoNpjEnum? IdSituacaoNpjAtual { get; set; }

        public SituacaoProjudiEnum? IdSituacaoProjudiAtual { get; set; }

        public SituacaoAtendimentoEnum? IdSituacaoAtendimentoAtual { get; set; }

        public long IdAssunto { get; set; }

        public string Assunto { get; set; }

        public bool Concluido { get; set; }

        public long IdUsuarioCriacao { get; set; }

        public string UsuarioCriacao { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public string DescricaoDataHoraCriacao { get { return DataHoraCriacao.ToString("dd/MM/yyyy HH:mm"); } set { } }

        public DateTime DataHoraAlteracao { get; set; }

        public string DescricaoDataHoraAlteracao { get { return DataHoraAlteracao.ToString("dd/MM/yyyy HH:mm"); } set { } }
    }
}