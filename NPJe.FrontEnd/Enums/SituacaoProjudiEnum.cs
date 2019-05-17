using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum SituacaoProjudiEnum : int
    {
        [Description("ARQUIVADO DEFINITIVAMENTE")]
        ArquivadoDefinitivamente = -1,

        [Description("ARQUIVADO PROVISORIAMENTE")]
        ArquivadoProvisoriamente = -2,

        [Description("ATIVO")]
        Ativo = -3,

        [Description("AVALIAÇÃO")]
        Avaliacao = -4,

        [Description("CONHECIMENTO")]
        Conhecimento = -5,

        [Description("DISTRIBUIR")]
        Distribuir = -6,

        [Description("HABILITAR")]
        Habilitar = -7,

        [Description("SUSPENSO")]
        Suspenso = -8
    }
}