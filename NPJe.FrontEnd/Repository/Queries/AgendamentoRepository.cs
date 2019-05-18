using NPJe.FrontEnd.Configs;
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
        public RetornoDto GetAgendamentoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            var consulta = (from a in Contexto.Agendamento
                            where a.IdUsuario == SessionUser.IdUsuario
                            select a);

            if (!search.IsNullOrEmpty())
                consulta = consulta.Where(x => x.Titulo.Contains(search));

            var data = (from a in consulta
                        select new AgendamentoDto()
                        {
                            Id = a.Id,
                            Titulo = a.Titulo,
                            Descricao = a.Descricao,
                            DataAgendamento = a.DataAgendamento,
                            Horario = a.Horario
                        })
                    .OrderBy(order.RemoveAccents() + " " + dir)
                    .Skip(start).Take(length);

            var agendamentos = data.ToList();

            agendamentos.ForEach(x => x.DescricaoDataAgendamento = x.DataAgendamento.ToString("dd/MM/yyyy"));

            return CreateDataResult(data.Count(), agendamentos);
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
                        Horario = a.Horario
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
                IdUsuario = SessionUser.IdUsuario
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
            var dataDeHoje = DateTime.Now.Date;
            var agendamentos = (from a in Contexto.Agendamento
                               where !a.Concluido && 
                               a.IdUsuario == SessionUser.IdUsuario
                               && a.DataAgendamento == dataDeHoje
                                select new GenericInfoComboDto() {
                                   id = a.Id,
                                   text = a.Titulo,
                                   complement = a.Horario
                                })
                               .ToList();
            agendamentos.ForEach(x => x.complement = x.complement.Substring(0, 5));
            return agendamentos;
        }

        #endregion

        public RetornoComboDto GetPastaComboDto(long? id, string search)
        {
            var idGruposPermitidos = GetListaGruposUsuario();

            var consulta = (from p in Contexto.Pasta select p);

            if (idGruposPermitidos.Any())
                consulta = consulta.Where(p => idGruposPermitidos.Contains(p.IdGrupo));

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty())
                consulta = consulta.Where(x => x.Assunto.Descricao.Contains(search) || x.Id.ToString() == search);

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
                consulta = consulta.Where(x => x.TipoAcao.Descricao.Contains(search));

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
    }
}