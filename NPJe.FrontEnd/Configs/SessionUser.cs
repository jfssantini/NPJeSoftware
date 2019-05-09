using NPJe.FrontEnd.Enums;

namespace NPJe.FrontEnd.Configs
{
    public static class SessionUser
    {
        public static long IdUsuario { get; set; }

        public static string Usuario { get; set; }

        public static PapelUsuarioEnum IdPapel { get; set; }

        public static string Papel { get { return IdPapel.GetDescription(); } set { } }

        public static StatusUsuarioEnum IdStatus { get; set; }

        public static string Status { get { return IdStatus.GetDescription(); } set { } }
    }
}