
namespace NPJe.FrontEnd.Models
{
    public class Endereco
    {
        public long Id { get; set; }

        public string CEP { get; set; }

        public string Cidade { get; set; }

        public string Bairro { get; set; }

        public string InfoEndereco { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }

        public string Observacao { get; set; }
    }
}