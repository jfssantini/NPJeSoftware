using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPJe.FrontEnd.Dtos
{
    public class ResponsavelDto
    {

        public long Id { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public string Nome { get; set; }

        public int Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        public string DescricaoDataNascimento { get; set; }

        public string CPF { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public long IdUsuario { get; set; }

        public TipoResponsavelEnum IdTipoResponsavel { get; set; }

        public bool Ativo { get; set; }
    }
}