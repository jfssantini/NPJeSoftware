using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum SituacaoNpjEnum : int
    {
        [Description("AÇÃO INICIADA")]
        AcaoIniciada = -1,

        [Description("AGENDAR ATENDIMENTO")]
        AgendarAtendimento = -2,

        [Description("ANÁLISE DO CASO")]
        AnaliseDoCaso = -3,

        [Description("DESISTÊNCIA")]
        Desistencia = -4,

        [Description("ENTRADA DE RECURSO")]
        EntradaDeRecurso = -5,

        [Description("SOLICITAÇÃO DE DOCUMENTOS")]
        SolicitacaoDeDocumentos = -6,

        [Description("CANCELADO")]
        Cancelado = -7,

        [Description("HABILITAR")]
        Habilitar = -8,

        [Description("REVOGADO")]
        Revogado = -9
    }
}