using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NPJe.FrontEnd.Repository.Queries
{
    public class GrupoRepository : QueriesRepository
    {
        private Contexto Contexto { get; set; }
        public GrupoRepository()
        {
            Contexto = new Contexto();
        }

        #region Grupo
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
                    .Skip(start).Take(length);

            return CreateDataResult(data.Count(), data.ToList());
        }

        public RetornoComboDto GetGrupoComboDto(long? id, string search)
        {
            var consulta = (from g in Contexto.Grupo
                            select g);

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

        public bool RemoveGrupo(long id)
        {
            Grupo grupo = new Grupo() { Id = id };
            Contexto.Grupo.Attach(grupo);
            Contexto.Grupo.Remove(grupo);
            Contexto.SaveChanges();
            return true;
        }
        #endregion
    }
}