﻿using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Enums;
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
        public RetornoDto GetClienteDtoGrid(int draw, int start, int length, string search, string order, string dir,
            bool apenasSemProcessoAndamento, bool apenasPF, bool apenasPJ)
        {
            var consulta = (from c in Contexto.Cliente select c);

            if (!search.IsNullOrEmpty())
            {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(c => (c.Nome.ToLower()).Contains(searchLower));
            }

            if (apenasPF)
                consulta = consulta.Where(x => x.CPF.Length > 0);

            if (apenasPJ)
                consulta = consulta.Where(x => x.CNPJ.Length > 0);

            if (apenasSemProcessoAndamento)
                consulta = (from c in consulta
                            from p in Contexto.Pasta.Where(x => x.IdCliente == c.Id).DefaultIfEmpty()
                            where (!((long?)p.Id).HasValue)
                            select c);

            var data = (from c in consulta                        
                        select new ClienteGridDto()
                        {
                            Id = c.Id,
                            Nome = c.Nome,
                            Sexo = c.Sexo,
                            DataNascimento = c.DataNascimento
                        })
                        .Distinct()
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

        public bool IsClienteRepetidoByCNPJ(string CNPJ, long id)
        {
            if (!CNPJ.IsNullOrEmpty())
            {
                var responsavel = (from r in Contexto.Cliente
                               .Where(x => x.CNPJ == CNPJ && x.Id != id)
                                   select r.Id).Count() > 0;

                return responsavel;
            }
            return false;
        }

        public bool IsClienteRepetidoByCPF(string CPF, long id)
        {
            if (!CPF.IsNullOrEmpty())
            {
                var responsavel = (from r in Contexto.Cliente
                               .Where(x => x.CPF == CPF && x.Id != id)
                                   select r.Id).Count() > 0;

                return responsavel;
            }
            return false;
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
            {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(x => (x.Nome.ToLower()).Contains(searchLower));
            }
                

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
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.cliente 
            SET IdUsuarioExclusao = {SessionUser.IdUsuario}, 
            DataHoraExclusao = '{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}'
            WHERE Id = {id}");

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