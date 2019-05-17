using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class ClienteRepository : QueriesRepository
    {
        public ClienteRepository() : base() { }

        #region Cliente
        public RetornoDto GetClienteDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            var data = (from c in Contexto.Cliente
                        where search.Length > 0 ? c.Nome.Contains(search) : true
                        select new ClienteGridDto()
                        {
                            Id = c.Id,
                            Nome = c.Nome,
                            Sexo = c.Sexo,
                            DataNascimento = c.DataNascimento
                        })
                        .OrderBy(AjustarOrdenacao(order) + " " + dir)
                        .Skip(start).Take(length);

            return CreateDataResult(data.Count(), data.ToList());
        }

        public ClienteDto GetClienteDto(long id)
        {
            var retorno = (from c in Contexto.Cliente
                           where c.Id == id
                           select new ClienteDto()
                           {
                               Id = c.Id,
                               Nome = c.Nome,
                               Sexo = c.Sexo,
                               DataNascimento = c.DataNascimento,
                               Celular = c.Celular,
                               Email = c.Email,
                               Telefone = c.Telefone,
                               IdEndereco = c.IdEndereco,
                               CPF = c.CPF,
                               CNPJ = c.CNPJ,
                               CEP = c.Endereco.CEP,
                               InfoEndereco = c.Endereco.InfoEndereco,
                               Bairro = c.Endereco.Bairro,
                               Cidade = c.Endereco.Cidade,
                               Complemento = c.Endereco.Complemento,
                               Numero = c.Endereco.Numero,
                               Observacao = c.Endereco.Observacao
                           }).FirstOrDefault();

            retorno.DescricaoDataNascimento = retorno.DataNascimento.ToString("dd/MM/yyyy");

            return retorno;
        }

        public bool SaveCliente(ClienteDto dto)
        {
            var retorno = Contexto.Cliente.Add(new Cliente()
            {
                Nome = dto.Nome,
                Sexo = dto.Sexo,
                DataNascimento = Convert.ToDateTime(dto.DescricaoDataNascimento),
                Telefone = dto.Telefone,
                Celular = dto.Celular,
                Email = dto.Email,
                CPF = dto.CPF,
                CNPJ = dto.CNPJ,
                Endereco = new Endereco()
                {
                    CEP = dto.CEP,
                    Bairro = dto.Bairro,
                    Cidade = dto.Cidade,
                    InfoEndereco = dto.InfoEndereco,
                    Complemento = dto.Complemento,
                    Numero = dto.Numero,
                    Observacao = dto.Observacao
                }
            });

            Contexto.SaveChanges();

            return true;
        }

        public RetornoComboDto GetClienteComboDto(long? id, string search)
        {
            var consulta = (from c in Contexto.Cliente
                            select c);

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty())
                consulta = consulta.Where(x => x.Nome.Contains(search));

            var grupo = (from c in consulta
                         orderby c.Nome
                         select c).ToList();

            var data = new List<GenericInfoComboDto>();


            grupo.ForEach(x =>
            {
                data.Add(new GenericInfoComboDto()
                {
                    id = x.Id,
                    text = x.Nome
                });
            });

            return CreateDataComboResult(data.Count(), data);
        }

        public bool EditCliente(ClienteDto dto)
        {
            var cliente = (from c in Contexto.Cliente
                         where c.Id == dto.Id
                         select c).FirstOrDefault();

            cliente.Endereco = (from e in Contexto.Endereco
                             where e.Id == cliente.IdEndereco
                             select e).FirstOrDefault();

            cliente.Nome = dto.Nome;
            cliente.Sexo = dto.Sexo;
            cliente.DataNascimento = Convert.ToDateTime(dto.DescricaoDataNascimento);
            cliente.Telefone = dto.Telefone;
            cliente.Celular = dto.Celular;
            cliente.Email = dto.Email;
            cliente.CPF = dto.CPF;
            cliente.CNPJ = dto.CNPJ;

            cliente.Endereco.CEP = dto.CEP;
            cliente.Endereco.Bairro = dto.Bairro;
            cliente.Endereco.Cidade = dto.Cidade;
            cliente.Endereco.InfoEndereco = dto.InfoEndereco;
            cliente.Endereco.Complemento = dto.Complemento;
            cliente.Endereco.Numero = dto.Numero;
            cliente.Endereco.Observacao = dto.Observacao;

            Contexto.SaveChanges();

            return true;
        }

        public bool RemoveCliente(long id)
        {
            var cliente = (from c in Contexto.Cliente
                               where c.Id == id
                               select c).FirstOrDefault();

            var endereco = (from e in Contexto.Endereco
                           where e.Id == cliente.IdEndereco
                           select e).FirstOrDefault();

            Contexto.Cliente.Remove(cliente);
            Contexto.Endereco.Remove(endereco);
            Contexto.SaveChanges();
            return true;
        }

        #endregion

        public string AjustarOrdenacao(string order)
        {
            var retorno = "";
            switch (order)
            {
                case "Data de nascimento/Início":
                    retorno = "DataNascimento";
                    break;
                case "Nome/Razão Social":
                    retorno = "Nome";
                    break;
                default:
                    break;
            }
            return retorno;
        }
    }
}