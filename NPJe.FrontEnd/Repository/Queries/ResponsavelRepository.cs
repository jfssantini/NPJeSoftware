using System;
using System.Linq;
using System.Linq.Dynamic;
using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Context;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class ResponsavelRepository : QueriesRepository
    {
        private Contexto Contexto { get; set; }
        public ResponsavelRepository()
        {
            Contexto = new Contexto();
        }

        #region Responsaveis
        public RetornoDto GetResponsavelDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            var data = (from r in Contexto.Responsavel
                        where search.Length > 0 ? r.Nome.Contains(search) : true
                        select new ResponsavelGridDto()
                        {
                            Id = r.Id,
                            Nome = r.Nome,
                            Sexo = r.Sexo,
                            DataNascimento = r.DataNascimento,
                            Login = r.Usuario.UsuarioLogin,
                            IdTipoResponsavel = r.IdTipoResponsavel
                        })
                        .OrderBy(AjustarOrdenacao(order) + " " + dir)
                        .Skip(start).Take(length);

            return CreateDataResult(data.Count(), data.ToList());
        }

        public ResponsavelDto GetResponsavelDto(long id)
        {
            var retorno = (from r in Contexto.Responsavel
                           where r.Id == id
                           select new ResponsavelDto()
                           {
                               Id = r.Id,
                               Nome = r.Nome,
                               Sexo = r.Sexo,
                               DataNascimento = r.DataNascimento,
                               Usuario = r.Usuario.UsuarioLogin,
                               Celular = r.Celular,
                               CPF = r.CPF,
                               Email = r.Email,
                               IdUsuario = r.IdUsuario,
                               Telefone = r.Telefone,
                               IdTipoResponsavel = r.IdTipoResponsavel,
                               Ativo = r.Ativo,
                           }).FirstOrDefault();

            retorno.DescricaoDataNascimento = retorno.DataNascimento.ToString("dd/MM/yyyy");

            return retorno;
        }

        public bool SaveResponsavel(ResponsavelDto dto)
        {
            var retorno = Contexto.Responsavel.Add(new Responsavel()
            {
                Nome = dto.Nome,
                Sexo = dto.Sexo,
                DataNascimento = Convert.ToDateTime(dto.DescricaoDataNascimento),
                Telefone = dto.Telefone,
                Celular = dto.Celular,
                Email = dto.Email,
                CPF = dto.CPF,
                Ativo = dto.Ativo,
                IdTipoResponsavel = dto.IdTipoResponsavel,
                Usuario = new Usuario()
                {
                    UsuarioLogin = dto.Usuario,
                    Senha = GerarMD5(dto.Senha),
                    IdPapelUsuario = PapelUsuarioEnum.Administrador,
                    IdStatusUsuario = StatusUsuarioEnum.Offline
                }
            });

            Contexto.SaveChanges();

            return true;
        }

        public bool EditResponsavel(ResponsavelDto dto)
        {
            var responsavel = (from r in Contexto.Responsavel
                         where r.Id == dto.Id
                         select r).FirstOrDefault();

            responsavel.Usuario = (from u in Contexto.Usuario
                             where u.Id == responsavel.IdUsuario
                             select u).FirstOrDefault();

            responsavel.Nome = dto.Nome;
            responsavel.Sexo = dto.Sexo;
            responsavel.DataNascimento = Convert.ToDateTime(dto.DescricaoDataNascimento);
            responsavel.Telefone = dto.Telefone;
            responsavel.Celular = dto.Celular;
            responsavel.Email = dto.Email;
            responsavel.CPF = dto.CPF;
            responsavel.Ativo = dto.Ativo;
            responsavel.Usuario.UsuarioLogin = dto.Usuario;
            responsavel.IdTipoResponsavel = dto.IdTipoResponsavel;

            if (dto.Senha.Length > 3)
                responsavel.Usuario.Senha = GerarMD5(dto.Senha);

            Contexto.SaveChanges();

            return true;
        }

        public bool RemoveResponsavel(long id)
        {
            var responsavel = (from r in Contexto.Responsavel
                               where r.Id == id
                               select r).FirstOrDefault();

            var usuario = (from u in Contexto.Usuario
                                   where u.Id == responsavel.IdUsuario
                           select u).FirstOrDefault();

            Contexto.Responsavel.Remove(responsavel);
            Contexto.Usuario.Remove(usuario);
            Contexto.SaveChanges();
            return true;
        }

        #endregion

        public string AjustarOrdenacao(string order)
        {
            var retorno = "";
            switch (order)
            {
                case "Data de nascimento":
                    retorno = "DataNascimento";
                    break;
                case "Nome":
                    retorno = order;
                    break;
                case "Tipo":
                    retorno = "IdTipoResponsavel";
                    break;
                default:
                    break;
            }
            return retorno;
        }
    }
}