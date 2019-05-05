using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Models
{
    public class Responsavel
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public int Sexo { get; set; }

        public DateTime Datanascimento { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public long IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        public TipoResponsavelEnum IdTipoResponsavel { get; set; }

        public bool Ativo { get; set; }
    }
}