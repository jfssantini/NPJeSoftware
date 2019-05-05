using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum StatusUsuarioEnum : int
    {
        [Description("ONLINE")]
        Online = -1,

        [Description("OFFLINE")]
        Offline = -2,
    }
}