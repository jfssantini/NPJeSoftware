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
    public class PastaRepository : QueriesRepository
    {
        public PastaRepository() : base() { }

        #region Pasta
        public RetornoDto GetPastaDtoGrid(int draw, int start, int length, string search, string order, string dir,
            long? idCliente, long? idGrupo, int? idSituacaoAtendimento)
        {
            var idGruposPermitidos = GetListaGruposUsuario(null);

            if (!ValidarUsuarioComGrupos(idGruposPermitidos))
                return new RetornoDto() { data = new List<PastaGridDto>(), recordsFiltered = 0 };

            var consulta = (from p in Contexto.Pasta
                            select p);

            if (idGruposPermitidos.Any())
                consulta = consulta.Where(p => idGruposPermitidos.Contains(p.IdGrupo));

            if (!search.IsNullOrEmpty())
            {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(p => (p.Assunto.Descricao.ToLower()).Contains(searchLower));
            }

            if(idCliente.HasValue)
                consulta = consulta.Where(x => x.IdCliente == idCliente);

            if (idGrupo.HasValue)
                consulta = consulta.Where(x => x.IdGrupo == idGrupo);

            if (idSituacaoAtendimento.HasValue)
                consulta = consulta.Where(x => x.IdSituacaoAtendimentoAtual == (SituacaoAtendimentoEnum)idSituacaoAtendimento);

            var data = (from p in consulta
                        select new PastaGridDto()
                    {
                        Id = p.Id,
                        Assunto = p.Assunto.Descricao,
                        IdCliente = p.IdCliente,
                        Cliente = p.Cliente.Nome,
                        IdGrupo = p.IdGrupo,
                        NumeroGrupo = p.Grupo.Numero,
                        IdEspecialidadeGrupo = p.Grupo.IdEspecialidade,
                        IdSituacaoNpjAtual = p.IdSituacaoNpjAtual,
                        IdSituacaoProjudiAtual = p.IdSituacaoProjudiAtual,
                        IdSituacaoAtendimentoAtual = p.IdSituacaoAtendimentoAtual,
                        DataHoraCriacao = p.DataHoraCriacao,
                        DataHoraAlteracao = p.DataHoraAlteracao
                    })
                    .OrderBy(AjustarOrdenacao(order) + " " + dir)
                    .Skip(start).Take(length);


            var pastas = data.ToList();

            var idPastas = pastas.Select(x => x.Id);

            if (idPastas.Any())
            {
                var atendimentos = (from a in Contexto.Atendimento
                                    where idPastas.Contains(a.IdPasta.Value)
                                    orderby a.Id descending
                                    select new AtendimentoDto() {
                                        Id = a.Id,
                                        IdPasta = a.IdPasta,
                                        IdUsuarioRegistro = a.IdUsuarioRegistro,
                                        DataHoraRegistro = a.DataHoraRegistro,
                                        UsuarioRegistro = a.UsuarioRegistro.UsuarioLogin
                                    }).ToList();

                pastas.ForEach(x =>
                {
                    var ultimoAtendimento = atendimentos.Where(y => y.IdPasta == x.Id).OrderByDescending(y => y.Id).FirstOrDefault();
                    if (ultimoAtendimento != null)
                    {
                        x.UsuarioUltimaTarefa = ultimoAtendimento.UsuarioRegistro;
                        x.IdUsuarioUltimaTarefa = ultimoAtendimento.IdUsuarioRegistro;
                    }
                });
            }

            return CreateDataResult(data.Count(), pastas);
        }

        public PastaDto GetPastaDto(long id)
        {
            var retorno = (from p in Contexto.Pasta
                           from r in Contexto.Responsavel.Where(x => x.IdUsuario == p.IdUsuarioResponsavel).DefaultIfEmpty()
                           where p.Id == id
                           select new PastaDto()
                           {
                               Id = p.Id,
                               IdCliente = p.IdCliente,
                               Cliente = p.Cliente.Nome,
                               IdGrupo = p.IdGrupo,
                               NumeroGrupo = p.Grupo.Numero,
                               IdEspecialidadeGrupo = p.Grupo.IdEspecialidade,
                               IdUsuarioResponsavel = p.IdUsuarioResponsavel,
                               UsuarioResponsavel = p.IdUsuarioResponsavel.HasValue ? r.Nome : "",
                               IdSituacaoNpjAtual = p.IdSituacaoNpjAtual,
                               IdSituacaoProjudiAtual = p.IdSituacaoProjudiAtual,
                               IdSituacaoAtendimentoAtual = p.IdSituacaoAtendimentoAtual,
                               IdAssunto = p.IdAssunto,
                               Assunto = p.Assunto.Descricao,
                               IdUsuarioCriacao = p.IdUsuarioCriacao,
                               UsuarioCriacao = p.UsuarioCriacao.UsuarioLogin,
                               DataHoraCriacao = p.DataHoraCriacao,
                               Concluido = p.Concluido,
                               DataHoraAlteracao = p.DataHoraAlteracao
                           }).FirstOrDefault();

            return retorno;
        }

        public bool SavePasta(PastaDto dto)
        {
            var retorno = Contexto.Pasta.Add(new Pasta()
            {
                IdCliente = dto.IdCliente,
                IdGrupo = dto.IdGrupo,
                IdUsuarioResponsavel = dto.IdUsuarioResponsavel,
                IdSituacaoNpjAtual = dto.IdSituacaoNpjAtual,
                IdSituacaoProjudiAtual = dto.IdSituacaoProjudiAtual,
                IdSituacaoAtendimentoAtual = dto.IdSituacaoAtendimentoAtual,
                IdAssunto = dto.IdAssunto,
                Concluido = dto.Concluido,
                DataHoraCriacao = DateTime.Now,
                DataHoraAlteracao = DateTime.Now,
                IdUsuarioCriacao = SessionUser.IdUsuario
            });

            Contexto.SaveChanges();

            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Temporario = false, IdPasta = {retorno.Id}
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdPasta IS NULL
                AND Temporario = true AND IdProcesso IS NULL");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Esconder = true AND IdProcesso IS NULL");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Excluir = false, IdUsuarioExclusao = {SessionUser.IdUsuario}
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Excluir = true AND IdProcesso IS NULL");

            return true;
        }

        public bool EditPasta(PastaDto dto)
        {
            var pasta = (from p in Contexto.Pasta
                         where p.Id == dto.Id
                         select p).FirstOrDefault();

            pasta.IdCliente = dto.IdCliente;
            pasta.IdGrupo = dto.IdGrupo;
            pasta.IdAssunto = dto.IdAssunto;
            pasta.Concluido = dto.Concluido;
            pasta.IdUsuarioResponsavel = dto.IdUsuarioResponsavel;
            pasta.DataHoraAlteracao = DateTime.Now;

            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Temporario = false, IdPasta = {dto.Id}
                WHERE IdUsuario = {SessionUser.IdUsuario}
                AND Temporario = true AND IdProcesso IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Esconder = true AND IdProcesso IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Excluir = false, IdUsuarioExclusao = {SessionUser.IdUsuario}
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Excluir = true AND IdProcesso IS NULL;");

            var ultimosStatus = (from a in Contexto.Atendimento
                               where a.IdPasta == dto.Id && !a.IdUsuarioExclusao.HasValue
                               orderby a.DataHoraAlteracao descending
                               select new { a.IdSituacaoAtendimento, a.IdSituacaoNpj, a.IdSituacaoProjudi })
                               .FirstOrDefault();

            if (ultimosStatus != null)
            {
                pasta.IdSituacaoAtendimentoAtual = ultimosStatus.IdSituacaoAtendimento;
                pasta.IdSituacaoNpjAtual = ultimosStatus.IdSituacaoNpj;
                pasta.IdSituacaoProjudiAtual = ultimosStatus.IdSituacaoProjudi;
            }

            Contexto.SaveChanges();

            return true;
        }

        public bool ExcluirRegistrosInconsistentes()
        {
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND Temporario = true 
                AND IdProcesso IS NULL; ");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Esconder = false WHERE IdUsuario = {SessionUser.IdUsuario} 
                AND Esconder = true AND IdProcesso IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdPasta IS NULL
                AND Temporario = false AND Esconder = false AND Excluir = false
                AND IdProcesso IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Excluir = false, IdUsuarioExclusao = NULL, DataHoraExclusao = NULL
                WHERE IdUsuario = {SessionUser.IdUsuario}
                AND IdProcesso IS NULL AND Excluir = true;");

            return true;
        }

        public bool RemovePasta(PastaDto dto)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.pasta 
                SET IdUsuarioExclusao = {SessionUser.IdUsuario}, 
                DataHoraExclusao = '{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}'
                WHERE Id = {dto.Id}");

            return true;
        }

        #endregion

        #region Atendimento
        public RetornoDto GetAtendimentoGrid(string order, string dir, long? idPasta)
        {
            var consulta = (from a in Contexto.Atendimento
                            where !a.Esconder && !a.Excluir
                            select a);

            if(SessionUser.IdPapel == PapelUsuarioEnum.Aluno)
            {
                consulta = (from a in consulta
                 where !a.IdUsuarioExclusao.HasValue
                 select a);
            }

            if (idPasta.HasValue)
            {
                consulta = from a in consulta
                           where (a.IdPasta == idPasta || 
                           (a.Temporario && !a.IdPasta.HasValue && a.IdUsuario == SessionUser.IdUsuario && !a.IdProcesso.HasValue))
                           select a;
            }
            else
            {
                consulta = from a in consulta
                           where a.Temporario && !a.IdPasta.HasValue && !a.IdProcesso.HasValue &&
                            a.IdUsuario == SessionUser.IdUsuario
                           select a;
            }

            var data = (from a in consulta
                        orderby a.DataHoraRegistro descending
                        select new AtendimentoDto()
                        {
                            Id = a.Id,
                            IdPasta = a.IdPasta,
                            Titulo = a.Titulo,
                            Descricao = a.Descricao,
                            IdSituacaoNpj = a.IdSituacaoNpj,
                            IdSituacaoProjudi = a.IdSituacaoProjudi,
                            IdSituacaoAtendimento = a.IdSituacaoAtendimento,
                            IdProcesso = a.IdProcesso,
                            IdUsuarioRegistro = a.IdUsuarioRegistro,
                            UsuarioRegistro = a.UsuarioRegistro.UsuarioLogin,
                            DataHoraRegistro = a.DataHoraRegistro,
                            IdUsuarioExclusao = a.IdUsuarioExclusao,
                            UsuarioExclusao = a.UsuarioExclusao.UsuarioLogin,
                            DataHoraExclusao = a.DataHoraExclusao,
                            DataHoraAlteracao = a.DataHoraAlteracao,
                            IdUsuarioAlteracao = a.IdUsuario,
                            UsuarioAlteracao = a.Usuario.UsuarioLogin
                        }).ToList();

            return CreateDataResult(data.Count, data);
        }

        public bool SaveAtendimento(AtendimentoDto dto, DateTime? dataHoraRegistro = null, long? idUsuarioRegistro = null)
        {
            var entity = new Atendimento()
            {
                IdPasta = dto.IdPasta == 0 ? null : dto.IdPasta,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                IdSituacaoNpj = dto.IdSituacaoNpj,
                IdSituacaoProjudi = dto.IdSituacaoProjudi,
                IdSituacaoAtendimento = dto.IdSituacaoAtendimento,
                IdProcesso = dto.IdProcesso == 0 ? null : dto.IdProcesso,
                IdUsuarioRegistro = idUsuarioRegistro ?? SessionUser.IdUsuario,
                DataHoraRegistro = dataHoraRegistro ?? DateTime.Now,
                DataHoraAlteracao = DateTime.Now,
                IdUsuario = SessionUser.IdUsuario,
                Temporario = true
            };

            Contexto.Atendimento.Add(entity);
            Contexto.SaveChanges();
            return true;
        }

        public bool EditAtendimento(AtendimentoDto dto)
        {
            var tarefa = (from a in Contexto.Atendimento
                                    where a.Id == dto.Id
                                    select new { a.DataHoraRegistro, a.IdUsuarioRegistro }).FirstOrDefault();

            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
			SET Esconder = true, Temporario = false WHERE Id = {dto.Id}");

            SaveAtendimento(dto, tarefa.DataHoraRegistro, tarefa.IdUsuarioRegistro);

            return true;
        }

        public bool RemoveAtendimento(long id)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Excluir = true, IdUsuarioExclusao = {SessionUser.IdUsuario},
                DataHoraExclusao = '{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}' 
                WHERE Id = {id}");

            return true;
        }
        #endregion

        #region Assunto
        public RetornoComboDto GetAssuntoComboDto(long? id, string search)
        {
            var consulta = (from a in Contexto.Assunto
                            select a);

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty()) {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(x => (x.Descricao.ToLower()).Contains(searchLower));
            }
                

            var assuntos = (from a in consulta
                            orderby a.Descricao
                            select a).ToList();

            var data = new List<GenericInfoComboDto>();

            assuntos.ForEach(x =>
            {
                data.Add(new GenericInfoComboDto()
                {
                    id = x.Id,
                    text = x.Descricao
                });
            });

            return CreateDataComboResult(data.Count(), data);
        }

        public long SaveAssunto(Assunto entity)
        {
            var retorno = Contexto.Assunto.Add(entity);
            Contexto.SaveChanges();
            return retorno.Id;
        }
        #endregion

        public string AjustarOrdenacao(string order)
        {
            var retorno = "";
            switch (order)
            {
                case "Id":
                case "Assunto":
                case "Cliente":
                    retorno = order;
                    break;
                case "Grupo":
                    retorno = "NumeroGrupo";
                    break;
                case "Ult. alteração":
                    retorno = "DataHoraAlteracao";
                    break;
                default:
                    break;
            }
            return retorno;
        }
    }
}