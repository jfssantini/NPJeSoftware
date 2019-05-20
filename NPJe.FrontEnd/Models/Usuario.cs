using NPJe.FrontEnd.Enums;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class Usuario
    {
        public long Id { get; set; }

        public string UsuarioLogin { get; set; }

        public string Senha { get; set; }

        public string UserKey { get; set; }

        public PapelUsuarioEnum IdPapelUsuario { get; set; }

        public StatusUsuarioEnum IdStatusUsuario { get; set; }

        public bool Excluido { get; set; }
    }
}