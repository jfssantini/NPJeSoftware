using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Dtos
{
    public class AtendimentoDto
    {
        public long Id { get; set; }

        public long? IdPasta { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public SituacaoNpjEnum? IdSituacaoNpj { get; set; }

        public string DescricaoSituacaoNpj { get { return IdSituacaoNpj?.GetDescription() ?? ""; } set { } }

        public SituacaoProjudiEnum? IdSituacaoProjudi { get; set; }

        public string DescricaoSituacaoProjudi { get { return IdSituacaoProjudi?.GetDescription() ?? ""; } set { } }

        public SituacaoAtendimentoEnum? IdSituacaoAtendimento { get; set; }

        public string DescricaoSituacaoAtendimento { get { return IdSituacaoAtendimento?.GetDescription() ?? ""; } set { } }

        public long? IdProcesso { get; set; }

        //public Processo Processo { get; set; }

        public long IdUsuarioRegistro { get; set; }

        public string UsuarioRegistro { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public string DescricaoDataHoraRegistro { get { return DataHoraRegistro.ToString("dd/MM/yyyy HH:mm"); } set { } }

        public long IdUsuarioAlteracao { get; set; }

        public string UsuarioAlteracao { get; set; }

        public DateTime DataHoraAlteracao { get; set; }

        public string DescricaoDataHoraAlteracao { get { return DataHoraAlteracao.ToString("dd/MM/yyyy HH:mm"); } set { } }

        public long? IdUsuarioExclusao { get; set; }

        public string UsuarioExclusao { get; set; }

        public DateTime? DataHoraExclusao { get; set; }

        public string DescricaoDataHoraExclusao { get { return DataHoraExclusao?.ToString("dd/MM/yyyy HH:mm") ?? ""; } set { } }
    }
}