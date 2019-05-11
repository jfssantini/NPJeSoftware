using System;

namespace NPJe.FrontEnd.Dtos
{
    public class ClienteDto
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public int Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        public string DescricaoDataNascimento { get; set; }

        public string CPFCNPJ { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public long? IdEndereco { get; set; }

        public string CEP { get; set; }

        public string Cidade { get; set; }

        public string Bairro { get; set; }

        public string InfoEndereco { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }
    }
}