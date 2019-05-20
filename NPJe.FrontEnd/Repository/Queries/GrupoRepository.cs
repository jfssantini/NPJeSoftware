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
    public class GrupoRepository : QueriesRepository
    {
        public GrupoRepository() : base() { }

        #region Grupo
        public RetornoDto GetGrupoDtoGrid(int draw, int start, int length, string search, string order, string dir, 
            bool incluiExcluidos, bool apenasGruposVazios, long? idEspecialidade)
        {
            var consulta = (from g in Contexto.Grupo
                            select g);

            if (!search.IsNullOrEmpty() && int.TryParse(search, out int result))
                consulta = consulta.Where(x => x.Numero == result);

            if (idEspecialidade.HasValue)
                consulta = consulta.Where(x => x.IdEspecialidade == (EspecialidadeEnum)idEspecialidade);

            if (!incluiExcluidos)
                consulta = consulta.Where(x => !x.IdUsuarioExclusao.HasValue);

            if (apenasGruposVazios)
                consulta = (from g in consulta
                            from a in Contexto.AlunoGrupo.Where(x => x.IdGrupo == g.Id).DefaultIfEmpty()
                            where !((long?)a.Id).HasValue
                            select g);


            var data = (from g in consulta
                        select new GrupoDto()
                        {
                            Id = g.Id,
                            IdEspecialidade = g.IdEspecialidade,
                            Numero = g.Numero,
                        })
                    .OrderBy(order.RemoveAccents() + " " + dir)
                    .Skip(start).Take(length);

            var grupos = data.ToList();
            var idGrupos = grupos.Select(x => x.Id).ToList();

            var vinculoAlunos = (from a in Contexto.AlunoGrupo
                                 where idGrupos.Contains(a.IdGrupo) &&
                                 a.Aluno.Ativo
                                 select a.IdGrupo).ToList();

            grupos.ForEach(x =>
            {
                x.QuantidadeAlunos = vinculoAlunos.Count(y => y == x.Id);
            });
                                
            return CreateDataResult(grupos.Count(), grupos);
        }

        public RetornoComboDto GetGrupoComboDto(long? id, string search, bool filtroGrupo)
        {
            var idGruposPermitidos = filtroGrupo ? GetListaGruposUsuario(null) : new List<long>();

            if (!ValidarUsuarioComGrupos(idGruposPermitidos) && filtroGrupo)
                return new RetornoComboDto() { results = new List<GenericInfoComboDto>(), total = 0 };

            var consulta = (from g in Contexto.Grupo select g);

            if (idGruposPermitidos.Any())
                consulta = consulta.Where(g => idGruposPermitidos.Contains(g.Id));

            if (id.HasValue)
                consulta = consulta.Where(x => x.Id == id);
            else if (!search.IsNullOrEmpty() && int.TryParse(search, out int result))
                consulta = consulta.Where(x => x.Numero == result);

            var grupo = (from g in consulta
                         orderby g.Numero
                         select g).ToList();

            var data = new List<GenericInfoComboDto>();


            grupo.ForEach(x =>
            {
                data.Add(new GenericInfoComboDto()
                {
                    id = x.Id,
                    text = x.Numero + " - " + x.IdEspecialidade.GetDescription()
                });
            });

            return CreateDataComboResult(data.Count(), data);
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

        public bool GrupoPossuiVinculos(long id)
        {
            var vinculoAluno = (from a in Contexto.AlunoGrupo
                                where a.IdGrupo == id
                                select a.Id).Count() > 0;

            var vinculoPasta = (from p in Contexto.Pasta
                                where p.IdGrupo == id
                                select p.Id).Count() > 0;

            return (vinculoAluno || vinculoPasta);
        }

        public bool IsGrupoRepetido(long id, int Numero)
        {
            return (from g in Contexto.Grupo
                            where g.Numero == Numero && g.Id != id
                    select g.Id).Count() > 0;

        }

        public bool RemoveGrupo(long id)
        {
            Contexto.Database.ExecuteSqlCommand($@"UPDATE dbo.grupo 
                SET IdUsuarioExclusao = {SessionUser.IdUsuario}, 
                DataHoraExclusao = '{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}'
                WHERE Id = {id}");

            Contexto.Database.ExecuteSqlCommand($@"DELETE FROM dbo.alunogrupo 
                WHERE IdGrupo = {id}");

            return true;
        }
        #endregion
    }
}