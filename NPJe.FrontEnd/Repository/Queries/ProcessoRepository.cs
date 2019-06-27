using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Dtos.Relatorios;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class ProcessoRepository : QueriesRepository
    {
        public ProcessoRepository() : base() { }

        #region Processo
        public RetornoDto GetProcessoDtoGrid(int draw, int start, int length, string search, string order, string dir,
            bool somenteConcluidos, bool processos15dias, bool processos30dias, string dataDistribuicao, 
            long? idCliente, long? idGrupo, int? idSituacaoNpj, int? idSituacaoProjudi)
        {
            var idGruposPermitidos = GetListaGruposUsuario(null);

            if (!ValidarUsuarioComGrupos(idGruposPermitidos))
                return new RetornoDto() { data = new List<ProcessoGridDto>(), recordsFiltered = 0 };

            var consulta = (from p in Contexto.Processo select p);

            if (idGruposPermitidos.Any())
                consulta = consulta.Where(p => idGruposPermitidos.Contains(p.Pasta.IdGrupo));

            if (search != null && search.Length > 0)
                consulta = consulta.Where(p => p.NumeroProcesso.Contains(search));

            if (!dataDistribuicao.IsNullOrEmpty())
            {
                var distribuicaoDate = Convert.ToDateTime(dataDistribuicao);
                consulta = consulta.Where(x => x.Distribuicao == distribuicaoDate);
            }

            if(idCliente.HasValue)
                consulta = consulta.Where(x => x.Pasta.IdCliente == idCliente);

            if (idGrupo.HasValue)
                consulta = consulta.Where(x => x.Pasta.IdGrupo == idGrupo);

            if (idSituacaoNpj.HasValue)
                consulta = consulta.Where(x => x.IdSituacaoNpjAtual == (SituacaoNpjEnum)idSituacaoNpj);

            if (idSituacaoProjudi.HasValue)
                consulta = consulta.Where(x => x.IdSituacaoProjudiAtual == (SituacaoProjudiEnum)idSituacaoProjudi);

            if(somenteConcluidos)
                consulta = consulta.Where(x => x.Status);

            if (processos15dias)
            {
                var dataLimite = DateTime.Now.AddDays(-15);
                var dataLimite2 = DateTime.Now.AddDays(-29);
                consulta = consulta.Where(x => !x.Status && x.DataHoraAlteracao < dataLimite && !(x.DataHoraAlteracao < dataLimite2));
            }

            if (processos30dias)
            {
                var dataLimite = DateTime.Now.AddDays(-30);
                consulta = consulta.Where(x => !x.Status && x.DataHoraAlteracao < dataLimite);
            }
                

            var data = (from p in consulta
                        select new ProcessoGridDto()
                        {
                            Id = p.Id,
                            Distribuicao = p.Distribuicao,
                            NumeroProcesso = p.NumeroProcesso,
                            IdCliente = p.Pasta.IdCliente,
                            Cliente = p.Pasta.Cliente.Nome,
                            IdGrupo = p.Pasta.IdGrupo,
                            NumeroGrupo = p.Pasta.Grupo.Numero,
                            IdEspecialidadeGrupo = p.Pasta.Grupo.IdEspecialidade,
                            IdSituacaoNpjAtual = p.IdSituacaoNpjAtual,
                            IdSituacaoProjudiAtual = p.IdSituacaoProjudiAtual,
                            DataHoraCriacao = p.DataHoraCriacao,
                            IdUsuario = p.IdUsuarioCriacao,
                            Usuario = p.UsuarioCriacao.UsuarioLogin,
                            TipoAcao = p.TipoAcao.Descricao,
                            Status = p.Status
                        })
                        .OrderBy(AjustarOrdenacao(order) + " " + dir)
                        .Skip(start).Take(length);


            var processos = data.ToList();

            var idProcessos = processos.Select(x => x.Id);

            if (idProcessos.Any())
            {
                var atendimentos = (from a in Contexto.Atendimento
                                    where idProcessos.Contains(a.IdProcesso.Value)
                                    orderby a.Id descending
                                    select new AtendimentoDto()
                                    {
                                        Id = a.Id,
                                        IdPasta = a.IdPasta,
                                        IdUsuarioRegistro = a.IdUsuarioRegistro,
                                        DataHoraRegistro = a.DataHoraRegistro,
                                        UsuarioRegistro = a.UsuarioRegistro.UsuarioLogin
                                    }).ToList();

                processos.ForEach(x =>
                {
                    var ultimoAtendimento = atendimentos.Where(y => y.IdPasta == x.Id).OrderByDescending(y => y.Id).FirstOrDefault();
                    if (ultimoAtendimento != null)
                    {
                        x.UsuarioUltimaTarefa = ultimoAtendimento.UsuarioRegistro;
                        x.IdUsuarioUltimaTarefa = ultimoAtendimento.IdUsuarioRegistro;
                        x.DataHoraUltimaTarefa = ultimoAtendimento.DataHoraRegistro;
                    }
                    else
                        x.DataHoraUltimaTarefa = x.DataHoraCriacao;

                    x.DescricaoDistribuicao = x.Distribuicao?.ToString("dd/MM/yyyy") ?? "";
                });
            }

            return CreateDataResult(data.Count(), processos);
        }

        public ProcessoDto GetProcessoDto(long id)
        {
            var retorno = (from p in Contexto.Processo
                           from r in Contexto.Responsavel.Where(x => x.IdUsuario == p.Pasta.IdUsuarioResponsavel).DefaultIfEmpty()
                           where p.Id == id
                           select new ProcessoDto()
                           {
                               Id = p.Id,
                               IdCliente = p.Pasta.IdCliente,
                               Cliente = p.Pasta.Cliente.Nome,
                               IdGrupo = p.Pasta.IdGrupo,
                               Distribuicao = p.Distribuicao,
                               NumeroProcesso = p.NumeroProcesso,
                               NumeroGrupo = p.Pasta.Grupo.Numero,
                               IdEspecialidadeGrupo = p.Pasta.Grupo.IdEspecialidade,
                               IdPasta = p.IdPasta,
                               Pasta = p.IdPasta + " - " + p.Pasta.Assunto.Descricao,
                               IdUsuarioResponsavel = p.Pasta.IdUsuarioResponsavel,
                               UsuarioResponsavel = r.Nome,
                               IdPolo = p.IdPolo,
                               IdSituacaoNpjAtual = p.IdSituacaoNpjAtual,
                               IdSituacaoProjudiAtual = p.IdSituacaoProjudiAtual,
                               IdTipoAcao = p.IdTipoAcao,
                               TipoAcao = p.TipoAcao.Descricao,
                               Status = p.Status,
                               ExpectativaValorCausa = p.ExpectativaValorCausa,
                               PercentualHonorarios = p.PercentualHonorarios,
                               ValorHonorarios = p.ValorHonorarios,
                               SegmentoJudiciario = p.SegmentoJudiciario,
                               Comarca = p.Comarca,
                               Vara = p.Vara,
                               Tribunal = p.Tribunal,
                               AnotacoesGerais = p.AnotacoesGerais,
                               IdUsuarioCriacao = p.IdUsuarioCriacao,
                               UsuarioCriacao = p.UsuarioCriacao.UsuarioLogin,
                               DataHoraCriacao = p.DataHoraCriacao,
                               DataHoraAlteracao = p.DataHoraAlteracao
                           }).FirstOrDefault();

            retorno.DescricaoDistribuicao = retorno.Distribuicao?.ToString("dd/MM/yyyy") ?? "";
            return retorno;
        }

        public RelatorioProcessoDto GetQuantidadeProcessos(long? idAluno)
        {
            var result = new RelatorioProcessoDto();

            var consulta = (from p in Contexto.Processo select p);

            if(SessionUser.IdPapel == PapelUsuarioEnum.Aluno || idAluno.HasValue)
            {
                var idGruposPermitidos = GetListaGruposUsuario(idAluno);

                if (!ValidarUsuarioComGrupos(idGruposPermitidos))
                    return result;

                if (idGruposPermitidos.Any())
                    consulta = consulta.Where(p => idGruposPermitidos.Contains(p.Pasta.IdGrupo));
            }

            var processos = (from p in consulta
                          select new Processos {
                              Id = p.Id,
                              Status = p.Status,
                              DataHoraAlteracao = p.DataHoraAlteracao,
                              DataHoraCriacao = p.DataHoraCriacao
                          }).ToList();

            result.QuantidadeProcessos = processos.Count();
            result.QuantidadeProcessosConcluidos = processos.Count(x => x.Status);
            result.QuantidadeProcessosParados15Dias = processos.Count(x => !x.Status && x.DataHoraAlteracao < DateTime.Now.AddDays(-15) && !(x.DataHoraAlteracao < DateTime.Now.AddDays(-29)));
            result.QuantidadeProcessosParados30Dias = processos.Count(x => !x.Status && x.DataHoraAlteracao < DateTime.Now.AddDays(-30));


            var processosPorMes = processos.GroupBy(x => x.MesCriacao).ToList();

            processosPorMes = processosPorMes.OrderBy(x => x.FirstOrDefault()?.MesCriacao ?? 0).ToList();

            for (int i = 0; i < 12; i++){
                //var atual = processosPorMes.Count > i ? processosPorMes[i] : null;
                var atual = processosPorMes.Any() ? processosPorMes.FirstOrDefault(y => y.All(z => z.MesCriacao == (i + 1))) : null;
                result.QuantidadeAbertaMes.Add(atual?.Count() ?? 0);
                result.QuantidadeConcluidaMes.Add(atual?.Count(y => y.Status) ?? 0);
            }

            return result;
        }

        public bool SaveProcesso(ProcessoDto dto)
        {
            var retorno = Contexto.Processo.Add(new Processo()
            {
                Distribuicao = Convert.ToDateTime(dto.DescricaoDistribuicao),
                NumeroProcesso = dto.NumeroProcesso,
                IdPasta = dto.IdPasta,
                IdPolo = dto.IdPolo,
                IdSituacaoNpjAtual = dto.IdSituacaoNpjAtual,
                IdSituacaoProjudiAtual = dto.IdSituacaoProjudiAtual,
                IdTipoAcao = dto.IdTipoAcao,
                Status = dto.Status,
                ExpectativaValorCausa = dto.ExpectativaValorCausa,
                PercentualHonorarios = dto.PercentualHonorarios,
                ValorHonorarios = dto.ValorHonorarios,
                SegmentoJudiciario = dto.SegmentoJudiciario,
                Comarca = dto.Comarca,
                Vara = dto.Vara,
                Tribunal = dto.Tribunal,
                AnotacoesGerais = dto.AnotacoesGerais,
                DataHoraCriacao = DateTime.Now,
                IdUsuarioCriacao = SessionUser.IdUsuario,
                DataHoraAlteracao = DateTime.Now
            });

            Contexto.SaveChanges();

            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Temporario = false, IdProcesso = {retorno.Id}
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdProcesso IS NULL
                AND Temporario = true AND IdPasta IS NULL");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Esconder = true AND IdPasta IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Excluir = false, IdUsuarioExclusao = {SessionUser.IdUsuario}
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Excluir = true AND IdPasta IS NULL;");

            return true;
        }

        public bool EditProcesso(ProcessoDto dto)
        {
            var processo = (from p in Contexto.Processo
                         where p.Id == dto.Id
                         select p).FirstOrDefault();

            processo.Distribuicao = Convert.ToDateTime(dto.DescricaoDistribuicao);
            processo.NumeroProcesso = dto.NumeroProcesso;
            processo.IdPasta = dto.IdPasta;
            processo.IdPolo = dto.IdPolo;
            processo.IdSituacaoNpjAtual = dto.IdSituacaoNpjAtual;
            processo.IdSituacaoProjudiAtual = dto.IdSituacaoProjudiAtual;
            processo.IdTipoAcao = dto.IdTipoAcao;
            processo.Status = dto.Status;
            processo.ExpectativaValorCausa = dto.ExpectativaValorCausa;
            processo.PercentualHonorarios = dto.PercentualHonorarios;
            processo.ValorHonorarios = dto.ValorHonorarios;
            processo.SegmentoJudiciario = dto.SegmentoJudiciario;
            processo.Comarca = dto.Comarca;
            processo.Vara = dto.Vara;
            processo.Tribunal = dto.Tribunal;
            processo.AnotacoesGerais = dto.AnotacoesGerais;
            processo.DataHoraAlteracao = DateTime.Now;

            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Temporario = false, IdProcesso = {dto.Id}
                WHERE IdUsuario = {SessionUser.IdUsuario}
                AND Temporario = true AND IdPasta IS NULL");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Esconder = true AND IdPasta IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Excluir = false, IdUsuarioExclusao = {SessionUser.IdUsuario}
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                Excluir = true AND IdPasta IS NULL;");

            var ultimosStatus = (from a in Contexto.Atendimento
                                 where a.IdProcesso == dto.Id && !a.IdUsuarioExclusao.HasValue
                                 orderby a.DataHoraAlteracao descending
                                 select new { a.IdSituacaoAtendimento, a.IdSituacaoNpj, a.IdSituacaoProjudi })
                               .FirstOrDefault();

            if (ultimosStatus != null)
            {
                processo.IdSituacaoAtendimentoAtual = ultimosStatus.IdSituacaoAtendimento;
                processo.IdSituacaoNpjAtual = ultimosStatus.IdSituacaoNpj;
                processo.IdSituacaoProjudiAtual = ultimosStatus.IdSituacaoProjudi;
            }

            Contexto.SaveChanges();

            return true;
        }

        public bool ExcluirRegistrosInconsistentes()
        {
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND Temporario = true
                AND IdPasta IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Esconder = false WHERE IdUsuario = {SessionUser.IdUsuario} 
                AND Esconder = true AND IdPasta IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.atendimento 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdProcesso IS NULL
                AND Temporario = false AND Esconder = false AND Excluir = false
                AND IdPasta IS NULL;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.atendimento 
                SET Excluir = false, IdUsuarioExclusao = NULL, DataHoraExclusao = NULL
                WHERE IdUsuario = {SessionUser.IdUsuario}
                AND Excluir = true AND IdPasta IS NULL;");

            return true;
        }

        public bool RemoveProcesso(ProcessoDto dto)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.processo 
                SET IdUsuarioExclusao = {SessionUser.IdUsuario}, 
                DataHoraExclusao = '{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}'
                WHERE Id = {dto.Id}");

            return true;
        }

        #endregion

        #region Atendimento
        public RetornoDto GetAtendimentoGrid(string order, string dir, long? idProcesso)
        {
            var consulta = (from a in Contexto.Atendimento
                            where !a.Esconder && !a.Excluir
                            select a);

            if (SessionUser.IdPapel == PapelUsuarioEnum.Aluno)
            {
                consulta = (from a in consulta
                            where !a.IdUsuarioExclusao.HasValue
                            select a);
            }

            if (idProcesso.HasValue)
            {
                consulta = from a in consulta
                           where (a.IdProcesso == idProcesso ||
                           (a.Temporario && !a.IdProcesso.HasValue && a.IdUsuario == SessionUser.IdUsuario && !a.IdPasta.HasValue))
                           select a;
            }
            else
            {
                consulta = from a in consulta
                           where a.Temporario && !a.IdProcesso.HasValue && !a.IdPasta.HasValue &&
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

        #region TipoAcao
        public RetornoComboDto GetTipoAcaoComboDto(long? id, string search)
        {
            var consulta = (from t in Contexto.TipoAcao
                            select t);

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty())
            {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(x => (x.Descricao.ToLower()).Contains(searchLower));
            }
                ;

            var assuntos = (from t in consulta
                            orderby t.Descricao
                            select t).ToList();

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

        public long SaveTipoAcao(TipoAcao entity)
        {
            var retorno = Contexto.TipoAcao.Add(entity);
            Contexto.SaveChanges();
            return retorno.Id;
        }
        #endregion

        public RetornoComboDto GetPastaComboDto(long? id, string search)
        {
            var idGruposPermitidos = GetListaGruposUsuario(null);

            if (!ValidarUsuarioComGrupos(idGruposPermitidos))
                return new RetornoComboDto() { results = new List<GenericInfoComboDto>(), total = 0 };

            var consulta = (from t in Contexto.Pasta select t);

            if (idGruposPermitidos.Any())
                consulta = consulta.Where(t => idGruposPermitidos.Contains(t.IdGrupo));

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty()) {
                var searchLower = search.ToLowerInvariant();
                consulta = consulta.Where(x => (x.Assunto.Descricao.ToLower()).Contains(searchLower) || x.Id.ToString() == search);
            }
                

            var pastas = (from t in consulta
                            orderby t.Id
                            select new { t.Id, t.Assunto.Descricao }).ToList();

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

        public RetornoComboDto GetInformacoesPastas(long idPasta)
        {
            var data = (from p in Contexto.Pasta
                        from r in Contexto.Responsavel.Where(x => x.IdUsuario == p.IdUsuarioResponsavel).DefaultIfEmpty()
                        where p.Id == idPasta
                        select new
                        {
                            p.IdCliente,
                            Cliente = p.Cliente.Nome,
                            p.IdGrupo,
                            NumeroGrupo = p.Grupo.Numero,
                            Grupo = "",
                            EspecialidadeGrupo = p.Grupo.IdEspecialidade,
                            p.IdUsuarioResponsavel,
                            UsuarioResponsavel = p.IdUsuarioResponsavel.HasValue ? r.Nome : "",
                        }).FirstOrDefault();

            

            return new RetornoComboDto()
            {
                results = new {
                    data.IdCliente,
                    data.Cliente,
                    data.IdGrupo,
                    Grupo = data.NumeroGrupo + " - " + data.EspecialidadeGrupo.GetDescription(),
                    data.IdUsuarioResponsavel,
                    data.UsuarioResponsavel
                }
            };
        }

        public string AjustarOrdenacao(string order)
        {
            var retorno = "";
            switch (order)
            {
                case "Distribuição":
                    retorno = "Distribuicao";
                    break;
                case "Número":
                    retorno = "NumeroProcesso";
                    break;
                case "Grupo":
                    retorno = "NumeroGrupo";
                    break;
                case "Cliente":
                    retorno = order.RemoveAccents();
                    break;
                default:
                    break;
            }
            return retorno;
        }
    }

    public class Processos : IComparable
    {
        public long Id { get; set; }

        public bool Status { get; set; }

        public DateTime? DataHoraAlteracao { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public int MesCriacao { get { return DataHoraCriacao.Month; } set { } }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}