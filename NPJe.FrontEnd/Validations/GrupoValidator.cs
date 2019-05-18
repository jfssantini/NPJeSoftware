using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Repository.Queries;
using System;

namespace NPJe.FrontEnd.Validations
{
    public class GrupoValidator
    {
        public void Validate(GrupoDto dto, bool delete)
        {
            if (SessionUser.IdPapel != PapelUsuarioEnum.Administrador)
                throw new ApplicationException("Você não possui permissão para executar esta operação.");

            if (!delete)
            {
                if (dto.Numero <= 0)
                    throw new ApplicationException("Grupo: O campo 'Número' não foi informado corretamente.");

                if (dto.IdEspecialidade == 0)
                    throw new ApplicationException("Grupo: O campo 'Especialidade' é obrigatório.");

                if(new GrupoRepository().IsGrupoRepetido(dto.Numero))
                    throw new ApplicationException("Grupo: Já existe um registro de grupo com o número informado.");
            }
            else
            {
                if (new GrupoRepository().GrupoPossuiVinculos(dto.Id))
                    throw new ApplicationException("Não é permitido excluir o registro pois o mesmo possui vínculos.");
            }
        }
            
    }
}