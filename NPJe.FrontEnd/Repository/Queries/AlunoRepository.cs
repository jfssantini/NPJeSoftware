using NPJe.FrontEnd.Configs;
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
    public class AlunoRepository : QueriesRepository
    {
        public AlunoRepository() : base() { }

        #region Aluno
        public RetornoDto GetAlunoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            var consulta = (from a in Contexto.Aluno select a);

            if (search != null && search.Length > 0)
                consulta = consulta.Where(a => a.Nome.Contains(search));

            var data = (from a in consulta
                        select new AlunoGridDto()
                        {
                            Id = a.Id,
                            Nome = a.Nome,
                            Sexo = a.Sexo,
                            DataNascimento = a.DataNascimento,
                            Matricula = a.Matricula,
                            Login = a.Usuario.UsuarioLogin
                        })
                        .OrderBy(AjustarOrdenacao(order) + " " + dir)
                        .Skip(start).Take(length);


            var alunos = data.ToList();
            var idAlunos = alunos.Select(x => x.Id);

            var grupos = (from g in Contexto.AlunoGrupo
                          where idAlunos.Contains(g.IdAluno.Value)
                          select new { g.IdAluno, g.Grupo.Numero, g.Grupo.IdEspecialidade })
                          .ToList();

            var especialidades = (from e in Contexto.AlunoEspecialidade
                                  where idAlunos.Contains(e.IdAluno.Value)
                                  select new { e.IdAluno, e.IdEspecialidade })
                          .ToList();

            alunos.ForEach(x =>
            {
                x.Grupos = grupos.Where(y => y.IdAluno == x.Id).Select(y => y.Numero + " - " + y.IdEspecialidade.GetDescription()).ToList();
                x.Especialidades = especialidades.Where(y => y.IdAluno == x.Id).Select(y => y.IdEspecialidade).ToList();
            });

            return CreateDataResult(data.Count(), alunos);
        }

        public AlunoDto GetAlunoDto(long id)
        {
            var retorno = (from a in Contexto.Aluno
                           where a.Id == id
                           select new AlunoDto()
                           {
                               Id = a.Id,
                               Nome = a.Nome,
                               Sexo = a.Sexo,
                               DataNascimento = a.DataNascimento,
                               Matricula = a.Matricula,
                               Usuario = a.Usuario.UsuarioLogin,
                               Celular = a.Celular,
                               CPF = a.CPF,
                               Email = a.Email,
                               IdUsuario = a.IdUsuario,
                               IdResponsavel = a.IdResponsavel,
                               Semestre = a.Semestre,
                               Telefone = a.Telefone,
                               Ativo = a.Ativo
                           }).FirstOrDefault();

            retorno.DescricaoDataNascimento = retorno.DataNascimento.ToString("dd/MM/yyyy");

            return retorno;
        }

        public bool SaveAluno(AlunoDto dto)
        {
            var retorno = Contexto.Aluno.Add(new Aluno()
            {
                Nome = dto.Nome,
                Sexo = dto.Sexo,
                DataNascimento = Convert.ToDateTime(dto.DescricaoDataNascimento),
                Telefone = dto.Telefone,
                Celular = dto.Celular,
                Email = dto.Email,
                Matricula = dto.Matricula,
                Semestre = dto.Semestre,
                CPF = dto.CPF,
                Ativo = dto.Ativo,
                IdResponsavel = dto.IdResponsavel,
                Usuario = new Usuario()
                {
                    UsuarioLogin = dto.Usuario,
                    Senha = GerarMD5(dto.Senha),
                    IdPapelUsuario = PapelUsuarioEnum.Aluno,
                    IdStatusUsuario = StatusUsuarioEnum.Offline
                }
            });

            Contexto.SaveChanges();

            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunoespecialidade 
                SET Temporario = false, IdAluno = {retorno.Id} 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdAluno IS NULL
                AND Temporario = true");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunogrupo 
                SET Temporario = false, IdAluno = {retorno.Id} 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdAluno IS NULL
                AND Temporario = true");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunoespecialidade 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                (Esconder = true OR Excluir = true);");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunogrupo 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                (Esconder = true OR Excluir = true);");

            return true;
        }

        public bool EditAluno(AlunoDto dto)
        {
            var aluno = (from a in Contexto.Aluno
                         where a.Id == dto.Id
                         select a).FirstOrDefault();

            aluno.Usuario = (from u in Contexto.Usuario
                             where u.Id == aluno.IdUsuario
                             select u).FirstOrDefault();

            aluno.Nome = dto.Nome;
            aluno.Sexo = dto.Sexo;
            aluno.DataNascimento = Convert.ToDateTime(dto.DescricaoDataNascimento);
            aluno.Telefone = dto.Telefone;
            aluno.Celular = dto.Celular;
            aluno.Email = dto.Email;
            aluno.Matricula = dto.Matricula;
            aluno.Semestre = dto.Semestre;
            aluno.CPF = dto.CPF;
            aluno.Ativo = dto.Ativo;
            aluno.IdResponsavel = dto.IdResponsavel;
            aluno.Usuario.UsuarioLogin = dto.Usuario;

            if (dto.Senha.Length > 3)
                aluno.Usuario.Senha = GerarMD5(dto.Senha);

            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunoespecialidade 
                SET Temporario = false, IdAluno = {dto.Id} 
                WHERE IdUsuario = {SessionUser.IdUsuario}
                AND Temporario = true");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunogrupo 
                SET Temporario = false, IdAluno = {dto.Id} 
                WHERE IdUsuario = {SessionUser.IdUsuario}
                AND Temporario = true");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunoespecialidade 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                (Esconder = true OR Excluir = true);");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunogrupo 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND 
                (Esconder = true OR Excluir = true);");

            Contexto.SaveChanges();

            return true;
        }

        public bool ExcluirRegistrosInconsistentes()
        {
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunoespecialidade 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND Temporario = true;");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunogrupo 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND Temporario = true;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunoespecialidade 
                SET Esconder = false WHERE IdUsuario = {SessionUser.IdUsuario} 
                AND Esconder = true;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunogrupo 
                SET Esconder = false WHERE IdUsuario = {SessionUser.IdUsuario} 
                AND Esconder = true;");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunoespecialidade 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdAluno IS NULL
                AND Temporario = false AND Esconder = false AND Excluir = false;");
            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunogrupo 
                WHERE IdUsuario = {SessionUser.IdUsuario} AND IdAluno IS NULL
                AND Temporario = false AND Esconder = false AND Excluir = false;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunoespecialidade 
                SET Excluir = false WHERE IdUsuario = {SessionUser.IdUsuario}
                AND Excluir = true;");
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunogrupo 
                SET Excluir = false WHERE IdUsuario = {SessionUser.IdUsuario}
                AND Excluir = true;");

            return true;
        }

        public bool IsAlunoRepetidoByCPF(string CPF)
        {
            if (!CPF.IsNullOrEmpty())
            {
                var responsavel = (from r in Contexto.Aluno
                               .Where(x => x.CPF == CPF)
                                   select r.Id).Count() > 0;

                return responsavel;
            }
            return false;
        }

        public bool IsUsuarioLoginRepetido(string login)
        {
            if (!login.IsNullOrEmpty())
            {
                var responsavel = (from u in Contexto.Usuario
                                   .Where(x => x.UsuarioLogin == login)
                                   select u.Id).Count() > 0;

                return responsavel;
            }
            return false;
        }
        #endregion

        #region AlunoGrupo
        public RetornoDto GetAlunoGrupoGrid(string order, string dir, long? idAluno)
        {
            var consulta = (from e in Contexto.AlunoGrupo
                            where !e.Esconder && !e.Excluir
                            select e);

            if (idAluno.HasValue)
            {
                consulta = from e in consulta
                           where (e.IdAluno == idAluno || (e.Temporario && !e.IdAluno.HasValue && e.IdUsuario == SessionUser.IdUsuario))
                           select e;
            }
            else
            {
                consulta = from e in consulta
                           where e.Temporario && !e.IdAluno.HasValue &&
                            e.IdUsuario == SessionUser.IdUsuario
                           select e;
            }

            var data = (from e in consulta
                        orderby e.Grupo.Numero
                        select new AlunoGrupoDto()
                        {
                            Id = e.Id,
                            IdAluno = e.IdAluno,
                            IdGrupo = e.IdGrupo,
                            Numero = e.Grupo.Numero,
                            IdEspecialidadeGrupo = e.Grupo.IdEspecialidade
                        }).ToList();

            return CreateDataResult(data.Count, data);
        }

        public bool SaveAlunoGrupo(AlunoGrupoDto dto)
        {
            var entity = new AlunoGrupo()
            {
                IdAluno = dto.IdAluno == 0 ? null : dto.IdAluno,
                IdGrupo = dto.IdGrupo,
                IdUsuario = SessionUser.IdUsuario,
                Temporario = true
            };

            Contexto.AlunoGrupo.Add(entity);
            Contexto.SaveChanges();
            return true;
        }

        public bool EditAlunoGrupo(AlunoGrupoDto dto)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunogrupo 
			SET Esconder = true, Temporario = false WHERE Id = {dto.Id}");

            SaveAlunoGrupo(dto);

            return true;
        }

        public bool RemoveAlunoGrupo(long id)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunogrupo 
                SET Excluir = true WHERE Id = {id}");

            return true;
        }
        #endregion

        #region AlunoEspecialidade
        public RetornoDto GetAlunoEspecialidadeGrid(string order, string dir, long? idAluno)
        {
            var consulta = (from e in Contexto.AlunoEspecialidade
                            where !e.Esconder && !e.Excluir
                            select e);

            if (idAluno.HasValue)
            {
                consulta = from e in consulta
                           where (e.IdAluno == idAluno || (e.Temporario && !e.IdAluno.HasValue && e.IdUsuario == SessionUser.IdUsuario))
                           select e;
            }
            else
            {
                consulta = from e in consulta
                           where e.Temporario && !e.IdAluno.HasValue &&
                            e.IdUsuario == SessionUser.IdUsuario
                           select e;
            }

            var data = (from e in consulta
                        orderby e.Id
                        select new EspecialidadeDto()
                        {
                            Id = e.Id,
                            IdAluno = e.IdAluno,
                            IdEspecialidade = e.IdEspecialidade,
                            IdUsuario = e.IdUsuario,
                            DisponibilidadeGrid = e.Disponibilidades.Select(x => new DisponibilidadeDto()
                            {
                                Id = x.Id,
                                IdAlunoEspecialidade = x.IdAlunoEspecialidade,
                                IdDiaSemana = x.IdDiaSemana,
                                HorarioInicio = x.HorarioInicio,
                                HorarioFim = x.HorarioFim
                            }).ToList()
                        }).ToList();

            data.ForEach(x =>
            {
                x.Disponibilidades = x.DisponibilidadeGrid.Select(y => y.IdDiaSemana.GetDescription()
                + " - " + y.HorarioInicio.Substring(0, 5) + " até " + y.HorarioFim.Substring(0, 5)).ToList();
            });

            return CreateDataResult(data.Count, data);
        }

        public bool SaveAlunoEspecialidade(EspecialidadeDto dto)
        {
            if (dto.DisponibilidadeGrid == null)
                dto.DisponibilidadeGrid = new List<DisponibilidadeDto>();

            var entity = new AlunoEspecialidade()
            {
                IdAluno = dto.IdAluno == 0 ? null : dto.IdAluno,
                IdEspecialidade = dto.IdEspecialidade,
                IdUsuario = SessionUser.IdUsuario,
                Temporario = true,
                Disponibilidades = new List<Disponibilidade>()
            };

            dto.DisponibilidadeGrid.ForEach(x => {
                entity.Disponibilidades.Add(new Disponibilidade()
                {
                    IdAlunoEspecialidade = x.IdAlunoEspecialidade,
                    HorarioInicio = x.HorarioInicio,
                    HorarioFim = x.HorarioFim,
                    IdDiaSemana = x.IdDiaSemana
                });
            });

            var retorno = Contexto.AlunoEspecialidade.Add(entity);
            Contexto.SaveChanges();
            return true;
        }

        public bool EditAlunoEspecialidade(EspecialidadeDto dto)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunoespecialidade 
			SET Esconder = true, Temporario = false WHERE Id = {dto.Id}");

            SaveAlunoEspecialidade(dto);

            return true;
        }

        public bool RemoveAlunoEspecialidade(long id)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.alunoespecialidade 
                SET Excluir = true WHERE Id = {id}");

            return true;
        }
        #endregion

        public string AjustarOrdenacao (string order)
        {
            var retorno = "";
            switch (order)
            {
                case "Data de nascimento":
                    retorno =  "DataNascimento";
                    break;
                case "Nome":
                    retorno = order;
                    break;
                default:
                    break;
            }
            return retorno;
        }
    }
}