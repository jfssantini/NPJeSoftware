using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Context;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Dynamic;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class QueriesRepository
    {
        protected Contexto Contexto { get; set; }
        public QueriesRepository()
        {
            Contexto = new Contexto();
        }

        #region Configurações extras
        public Usuario ConfirmLogin(string login, string password)
        {
            var MD5pass = GerarMD5(password);

            return (from c in Contexto.Usuario
                    where c.UsuarioLogin == login &&
                    MD5pass == c.Senha
                    select c)
                    .FirstOrDefault();
        }

        public string GerarMD5(string valor)
        {
            if (valor.Length > 1)
            {
                MD5 md5Hasher = MD5.Create();
                byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(valor));
                StringBuilder strBuilder = new StringBuilder();

                for (int i = 0; i < valorCriptografado.Length; i++)
                {
                    strBuilder.Append(valorCriptografado[i].ToString("x2"));
                }

                return strBuilder.ToString();
            }
            return null;
        }

        protected RetornoDto CreateDataResult(int count, object data)
        {
            return new RetornoDto
            {
                recordsFiltered = count,
                data = data
            };
        }

        protected RetornoComboDto CreateDataComboResult(int count, object data)
        {
            return new RetornoComboDto
            {
                total = count,
                results = data
            };
        }
        #endregion
    }
}