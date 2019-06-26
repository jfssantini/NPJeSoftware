using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Models;
using System;

namespace NPJe.FrontEnd.Validations
{
    public class PastaValidator
    {
        public void Validate(object dto, bool delete)
        {
            if (dto is PastaDto)
                ValidatePasta(dto as PastaDto, delete);
            if (dto is AtendimentoDto)
                ValidateAtendimento(dto as AtendimentoDto, delete);
            if (dto is Assunto)
                ValidateAssunto(dto as Assunto, delete);

        }

        private void ValidateAssunto(Assunto entity, bool delete)
        {
            if (entity.Descricao.IsNullOrEmpty() && !delete)
                throw new ApplicationException("Assunto: O campo 'Descrição' é obrigatório.");
        }

        private void ValidateAtendimento(AtendimentoDto dto, bool delete)
        {
            if (dto.Titulo.IsNullOrEmpty() && !delete)
                throw new ApplicationException("Atendimento: O campo 'Título' é obrigatório.");
        }

        private void ValidatePasta(PastaDto dto, bool delete)
        {
            if (!delete)
            {
                if (dto.IdAssunto == 0)
                    throw new ApplicationException("Processo: O campo 'Assunto' é obrigatório.");

                if (dto.IdGrupo == 0)
                    throw new ApplicationException("Processo: O campo 'Grupo' é obrigatório.");

                if (dto.IdCliente == 0)
                    throw new ApplicationException("Processo: O campo 'Cliente' é obrigatório.");
            }

        }
    }
}