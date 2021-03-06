﻿using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Dtos
{
    public class ResponsavelGridDto
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public int Sexo { get; set; }

        public string DescricaoSexo { get { return Sexo == 1 ? "Masculino" : "Feminino"; } set { } }

        public DateTime DataNascimento { get; set; }

        public string DescricaoDataNascimento { get { return DataNascimento.ToString("dd/MM/yyyy"); } set { } }

        public TipoResponsavelEnum IdTipoResponsavel { get; set; }

        public string DescricaoTipoResponsavel { get { return IdTipoResponsavel.GetDescription(); } set { } }
    }
}