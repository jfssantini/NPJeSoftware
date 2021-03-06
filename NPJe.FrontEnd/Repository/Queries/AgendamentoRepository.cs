﻿using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class AgendamentoRepository : QueriesRepository
    {
        public AgendamentoRepository() : base() { }

        #region Agendamento
        public RetornoDto GetAgendamentoDtoGrid(int draw, int start, int length, string search, string order, string dir,
            bool somenteAlunos, bool somenteDoUsuario, string dataAgendamento, long? idAluno)
        {

            var consulta = (from a in Contexto.Agendamento
                            select a);

            if (SessionUser.IdPapel == PapelUsuarioEnum.Aluno || somenteDoUsuario)
            {
                var grupos = GetListaGruposUsuario(null);
                consulta = consulta.Where(x => x.IdUsuario == SessionUser.IdUsuario || 
                grupos.Contains(x.Pasta.IdGrupo) || grupos.Contains(x.Processo.Pasta.IdGrupo));
            }
            else
            {
                if (somenteAlunos)
                {
                    consulta = (from c in consulta
                                from a in Contexto.Aluno.Where(x => x.IdUsuario == c.IdUsuario)
                                select c);
                }
                else if (idAluno.HasValue)
                {
                    consulta = (from c in consulta
                                from a in Contexto.Aluno.Where(x => x.IdUsuario == c.IdUsuario)
                                where a.Id == idAluno
                                select c);
                }
                else
                {
                    consulta = (from c in consulta
                                from a in Contexto.Aluno.Where(x => x.IdUsuario == c.IdUsuario).DefaultIfEmpty()
                                where (c.IdUsuario == SessionUser.IdUsuario || (c.IdUsuario != SessionUser.IdUsuario && ((long?)a.Id).HasValue))
                                select c);
                }
            }

            if (!dataAgendamento.IsNullOrEmpty())
            {
                var atendimentoDate = Convert.ToDateTime(dataAgendamento);
                consulta = consulta.Where(x => x.DataAgendamento == atendimentoDate);
            }


            if (!search.IsNullOrEmpty()) {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(x => (x.Titulo.ToLower()).Contains(searchLower));
            }
            

            var data = (from a in consulta
                        select new AgendamentoDto()
                        {
                            Id = a.Id,
                            Titulo = a.Titulo,
                            Descricao = a.Descricao,
                            DataAgendamento = a.DataAgendamento,
                            Horario = a.Horario,
                            Usuario = a.Usuario.UsuarioLogin
                        })
                    .OrderBy(AjustarOrdenacao(order, dir) + " " + dir)
                    .Skip(start).Take(length);

            var agendamentos = data.ToList();

            agendamentos.ForEach(x => x.DescricaoDataAgendamento = x.DataAgendamento.ToString("dd/MM/yyyy"));

            return CreateDataResult(agendamentos.Count(), agendamentos);
        }

        public AgendamentoDto GetAgendamentoDto(long id)
        {
            var retorno = (from a in Contexto.Agendamento
                    where a.Id == id
                    select new AgendamentoDto()
                    {
                        Id = a.Id,
                        IdPasta = a.IdPasta,
                        Pasta = a.IdPasta + " - " + a.Pasta.Assunto.Descricao,
                        IdProcesso = a.IdProcesso,
                        Processo = a.Processo.TipoAcao.Descricao,
                        Titulo = a.Titulo,
                        Descricao = a.Descricao,
                        DataAgendamento = a.DataAgendamento,
                        Horario = a.Horario,
                        Concluido = a.Concluido,
                        Usuario = a.Usuario.UsuarioLogin,
                        IdUsuario = a.IdUsuario
                    }).FirstOrDefault();

            retorno.DescricaoDataAgendamento = retorno.DataAgendamento.ToString("dd/MM/yyyy");

            return retorno;
        }

        public bool SaveAgendamento(AgendamentoDto dto)
        {
            Contexto.Agendamento.Add(new Agendamento()
            {
                IdPasta = dto.IdPasta,
                IdProcesso = dto.IdProcesso,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataAgendamento = Convert.ToDateTime(dto.DescricaoDataAgendamento),
                Horario = dto.Horario,
                IdUsuario = SessionUser.IdUsuario,
                Concluido = dto.Concluido
            });
            Contexto.SaveChanges();

            return true;
        }

        public bool EditAgendamento(AgendamentoDto dto)
        {
            var agendamento = (from a in Contexto.Agendamento
                         where a.Id == dto.Id
                         select a).FirstOrDefault();

            agendamento.IdPasta = dto.IdPasta;
            agendamento.IdProcesso = dto.IdProcesso;
            agendamento.Titulo = dto.Titulo;
            agendamento.Descricao = dto.Descricao;
            agendamento.DataAgendamento = Convert.ToDateTime(dto.DescricaoDataAgendamento);
            agendamento.Horario = dto.Horario;
            agendamento.IdUsuario = SessionUser.IdUsuario;
            agendamento.Concluido = dto.Concluido;

            Contexto.SaveChanges();

            return true;
        }

        public bool RemoveAgendamento(long id)
        {
            Agendamento Agendamento = new Agendamento() { Id = id };
            Contexto.Agendamento.Attach(Agendamento);
            Contexto.Agendamento.Remove(Agendamento);
            Contexto.SaveChanges();
            return true;
        }

        public List<GenericInfoComboDto> GetAgendamentosByIsuario()
        {

            var grupos = GetListaGruposUsuario(null);

            var dataDeHoje = DateTime.Now.Date;
            var agendamentos = (from a in Contexto.Agendamento
                               where !a.Concluido && 
                               (a.IdUsuario == SessionUser.IdUsuario || 
                               grupos.Contains(a.Pasta.IdGrupo) || 
                               grupos.Contains(a.Processo.Pasta.IdGrupo))
                               && a.DataAgendamento <= dataDeHoje
                                select new GenericInfoComboDto() {
                                   id = a.Id,
                                   text = a.Titulo,
                                   complement = a.Horario,
                                   data = a.DataAgendamento
                                })
                               .ToList();

            agendamentos.ForEach(x => 
            {
                x.complement = x.data.ToString("dd/MM/yyyy") + " às " + x.complement.Substring(0, 5) + " horas";
                if (x.data < dataDeHoje)
                    x.complement += " (Atrasado)";
            });
            return agendamentos;
        }

        #endregion

        public RetornoComboDto GetPastaComboDto(long? id, string search)
        {
            var idGruposPermitidos = GetListaGruposUsuario(null);

            var consulta = (from p in Contexto.Pasta select p);

            if (idGruposPermitidos.Any())
                consulta = consulta.Where(p => idGruposPermitidos.Contains(p.IdGrupo));

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty())
            {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(x => (x.Assunto.Descricao.ToLower()).Contains(searchLower) || x.Id.ToString() == search);
            }
                

            var pastas = (from p in consulta
                          orderby p.Id
                          select new { p.Id, p.Assunto.Descricao }).ToList();

            var data = new List<GenericInfoComboDto>();

            pastas.ForEach(x =>
            {
                data.Add(new GenericInfoComboDto()
                {
                    id = x.Id,
                    text = x.Id + " - " + x.Descricao
                });
            });

            return CreateDataComboResult(data.Count(), data);
        }

        public RetornoComboDto GetProcessoComboDto(long? id, string search, long idPasta)
        {
            var consulta = (from p in Contexto.Processo
                            where p.IdPasta == idPasta
                            select p);

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty())
            {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(x => (x.TipoAcao.Descricao.ToLower()).Contains(searchLower));
            }
                

            var pastas = (from p in consulta
                          orderby p.Id
                          select new { p.Id, p.TipoAcao.Descricao }).ToList();

            var data = new List<GenericInfoComboDto>();

            pastas.ForEach(x =>
            {
                data.Add(new GenericInfoComboDto()
                {
                    id = x.Id,
                    text = x.Id + " - " + x.Descricao
                });
            });

            return CreateDataComboResult(data.Count(), data);
        }

        public string AjustarOrdenacao(string order, string dir)
        {
            var retorno = "";
            switch (order)
            {
                case "Título":
                    retorno = order.RemoveAccents();
                    break;
                case "Horário":
                    retorno = "DataAgendamento " + dir + ", " + order.RemoveAccents();
                    break;
                case "Data":
                    retorno = "DataAgendamento";
                    break;
                default:
                    break;
            }
            return retorno;
        }
    }
}