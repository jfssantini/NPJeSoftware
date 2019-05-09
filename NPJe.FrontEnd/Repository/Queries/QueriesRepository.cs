using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Context;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Dynamic;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class QueriesRepository
    {
        private Contexto Contexto { get; set; }
        public QueriesRepository()
        {
            Contexto = new Contexto();
        }

        public RetornoDto GetAlunoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            var data = (from a in Contexto.Aluno
                        where a.Nome.Contains(search)
                        select new AlunoGridDto() {
                            Id = a.Id,
                            Nome = a.Nome,
                            Grupos = a.Grupos.Select(x => x.Grupo.Numero + "").ToList(),
                            Especialidades = a.Especialidades.Select(x => x.IdEspecialidade).ToList(),
                            Sexo = a.Sexo,
                            DataNascimento = a.Datanascimento,
                            Matricula = a.Matricula
                        })
                        .OrderBy(order.RemoveAccents() + " " + dir)
                        .Skip(start).Take(length);

            return CreateDataResult(Contexto.Aluno, data);
        }

        public bool EditGrupo(GrupoDto dto)
        {
            var grupo = (from g in Contexto.Grupo
                         where g.Id == dto.Id
                         select g).FirstOrDefault();

            grupo.IdEspecialidade = dto.IdEspecialidade;
            grupo.Numero = dto.Numero;

            Contexto.SaveChanges();

            return true;
        }

        public GrupoDto GetGrupoDto(long id)
        {
            return (from g in Contexto.Grupo
                            where g.Id == id
                            select new GrupoDto()
                            {
                                Id = g.Id,
                                IdEspecialidade = g.IdEspecialidade,
                                Numero = g.Numero
                            }).FirstOrDefault();
        }

        public RetornoDto GetEspecialidadeAlunoGrid(string order, string dir, long? idAluno)
        {
            var data = (from e in Contexto.AlunoEspecialidade
                            where e.IdAluno == idAluno && 
                            e.IdUsuario == SessionUser.IdUsuario
                            select new EspecialidadeDto() {
                                Id = e.Id,
                                IdAluno = e.IdAluno,
                                IdEspecialidade = e.IdEspecialidade,
                                IdUsuario = e.IdUsuario,
                                DisponibilidadeGrid = e.Disponibilidades.Select(x => new HorarioDto() {
                                    Id = x.Id,
                                    IdAlunoEspecialidade = x.IdAlunoEspecialidade,
                                    IdDiaSemana = x.IdDiaSemana,
                                    HorarioInicio = x.HorarioInicio,
                                    HorarioFim = x.HorarioFim
                                }).ToList()
                            }).ToList();

            return CreateDataResult(data.Count, data);
        }

        public RetornoDto GetGrupoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            var consulta = (from g in Contexto.Grupo
                            select g);

            if (!search.IsNullOrEmpty() && int.TryParse(search, out int result))
                consulta = consulta.Where(x => x.Numero == result);

            var data = (from g in consulta
                        select new GrupoDto()
                        {
                            Id = g.Id,
                            IdEspecialidade = g.IdEspecialidade,
                            Numero = g.Numero
                        })
                    .OrderBy(order.RemoveAccents() + " " + dir)
                    .Skip(start).Take(length)
                    .ToList();

            return CreateDataResult(Contexto.Grupo, data);
        }

        public bool SaveGrupo(GrupoDto dto)
        {
            Contexto.Grupo.Add(new Grupo()
            {
                IdEspecialidade = dto.IdEspecialidade,
                Numero = dto.Numero
            });
            Contexto.SaveChanges();

            return true;
        }

        public bool SaveDisponibilidade(EspecialidadeDto dto)
        {
            var entity = new AlunoEspecialidade()
            {
                IdAluno = dto.IdAluno == 0 ? null : dto.IdAluno,
                IdEspecialidade = dto.IdEspecialidade,
                IdUsuario = SessionUser.IdUsuario,
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

        public bool EditDisponibilidade(EspecialidadeDto dto)
        {
            throw new NotImplementedException();
        }



        public bool RemoveGrupo(long id)
        {
            Grupo grupo = new Grupo() { Id = id };
            Contexto.Grupo.Attach(grupo);
            Contexto.Grupo.Remove(grupo);
            Contexto.SaveChanges();
            return true;
        }

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
            MD5 md5Hasher = MD5.Create();
            byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(valor));
            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < valorCriptografado.Length; i++) {
                strBuilder.Append(valorCriptografado[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private RetornoDto CreateDataResult<T>(DbSet<T> dbSet, object data) where T : class{

            return new RetornoDto
            {
                recordsFiltered = dbSet.Count(),
                data = data
            };
        } 

        private RetornoDto CreateDataResult(int count, object data)
        {
            return new RetornoDto
            {
                recordsFiltered = count,
                data = data
            };
        }

        public class RetornoDto
        {
            public int recordsFiltered { get; set; }
            public object data { get; set; }
        }
    }
}