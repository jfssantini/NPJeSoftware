﻿using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Dtos
{
    public class ProcessoGridDto
    {
        public long Id { get; set; }

        public DateTime? Distribuicao { get; set; }

        public string DescricaoDistribuicao { get { return Distribuicao?.ToString("dd/MM/yyyy") ?? ""; } set { } }

        public string NumeroProcesso { get; set; }

        public long IdCliente { get; set; }

        public string Cliente { get; set; }

        public long IdGrupo { get; set; }

        public int NumeroGrupo { get; set; }

        public EspecialidadeEnum IdEspecialidadeGrupo { get; set; }

        public string Grupo { get { return NumeroGrupo + " - " + IdEspecialidadeGrupo.GetDescription(); } set { } }

        public SituacaoNpjEnum? IdSituacaoNpjAtual { get; set; }
        public string DescricaoSituacaoNpjAtual { get { return IdSituacaoNpjAtual?.GetDescription() ?? ""; } set { } }

        public SituacaoProjudiEnum? IdSituacaoProjudiAtual { get; set; }
        public string DescricaoSituacaoProjudiAtual { get { return IdSituacaoProjudiAtual?.GetDescription() ?? ""; } set { } }

        public long? IdUsuarioUltimaTarefa { get; set; }

        public string UsuarioUltimaTarefa { get; set; }

        public DateTime? DataHoraUltimaTarefa { get; set; }

        public string DescricaoDataHoraUltimaTarefa { get { return DataHoraUltimaTarefa?.ToString("dd/MM/yyyy HH:mm") ?? ""; } set { } }

        public DateTime? DataHoraCriacao { get; set; }

        public long IdUsuario { get; set; }

        public string Usuario { get; set; }

        public string TipoAcao { get; set; }

        public bool Status { get; set; }
    }
}