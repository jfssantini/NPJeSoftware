using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Context;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Dynamic;
using System.Collections.Generic;
using NPJe.FrontEnd.Enums;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class QueriesRepository
    {
        protected Contexto Contexto { get; set; }
        public QueriesRepository()
        {
            Contexto = new Contexto();
        }

        public List<long> GetListaGruposUsuario(long? idAluno)
        {
            var idGruposPermitidos = new List<long>();
            if (SessionUser.IdPapel == PapelUsuarioEnum.Aluno || idAluno.HasValue)
            {
                var consulta = (from a in Contexto.AlunoGrupo select a);
                if (idAluno.HasValue)
                    consulta = consulta.Where(x => x.IdAluno == idAluno);
                else
                    consulta = consulta.Where(x => x.Aluno.IdUsuario == SessionUser.IdUsuario);

                idGruposPermitidos = (from a in consulta
                                      select a.IdGrupo).ToList();
            }
            return idGruposPermitidos;
        }

        public bool ValidarUsuarioComGrupos(List<long> idGrupos)
        {
            if (SessionUser.IdPapel == PapelUsuarioEnum.Aluno && !idGrupos.Any())
                return false;

            return true;
        }

        #region Configurações extras
        public Usuario ConfirmLogin(string login, string password)
        {
            var MD5pass = GerarMD5(password);

            return (from c in Contexto.Usuario
                    where c.UsuarioLogin == login &&
                    MD5pass == c.Senha && !c.Excluido
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