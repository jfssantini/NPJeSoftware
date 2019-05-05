﻿using System;
namespace NPJe.FrontEnd.Models
{
    public class Cliente
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public int Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        public string CPFCNPJ { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public long? IdEndereco { get; set; }

        public Endereco Endereco { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }
    }
}