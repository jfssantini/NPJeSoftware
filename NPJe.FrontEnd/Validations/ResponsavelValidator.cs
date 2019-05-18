using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Repository.Queries;
using System;
using System.Text.RegularExpressions;

namespace NPJe.FrontEnd.Validations
{
    public class ResponsavelValidator
    {
        private string ValidEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public void Validate(ResponsavelDto dto, bool delete)
        {
            if (SessionUser.IdPapel != PapelUsuarioEnum.Administrador)
                throw new ApplicationException("Você não possui permissão para executar esta operação.");

            if (!delete) {
                if (dto.Usuario.IsNullOrEmpty())
                    throw new ApplicationException("Responsável: O campo 'Login' é obrigatório.");

                if (!dto.Senha.IsNullOrEmpty() && dto.Senha.Length < 6)
                    throw new ApplicationException("Responsável: O campo 'Senha' deve ter no mínimo 6 caracteres.");

                if (dto.Nome.IsNullOrEmpty())
                    throw new ApplicationException("Responsável: O campo 'Nome' é obrigatório.");

                if (!dto.CPF.IsNullOrEmpty() && dto.CPF.Length < 14)
                    throw new ApplicationException("Responsável: O campo 'CPF' deve ter 14 caracteres.");

                if (!dto.Email.IsNullOrEmpty() && !(new Regex(ValidEmailPattern, RegexOptions.IgnoreCase).IsMatch(dto.Email)))
                    throw new ApplicationException("Responsável: O campo 'Email' precisa ter um email no formato válido.");

                if (dto.Sexo != 1 && dto.Sexo != 2)
                    throw new ApplicationException("Responsável: O campo 'Sexo' é obrigatório.");

                if (!dto.DescricaoDataNascimento.IsNullOrEmpty() && !DateTime.TryParse(dto.DescricaoDataNascimento, out DateTime dataNasc) && dataNasc >= DateTime.Now.Date)
                    throw new ApplicationException("Responsável: O campo 'Data de nascimento' não foi informado corretamente.");

                if (!dto.Telefone.IsNullOrEmpty() && dto.Telefone.Length < 14)
                    throw new ApplicationException("Responsável: O campo 'Telefone' deve ter 14 caracteres.");

                if (!dto.Telefone.IsNullOrEmpty() && dto.Celular.Length < 16)
                    throw new ApplicationException("Responsável: O campo 'Celular' deve ter 16 caracteres.");

                if (dto.IdTipoResponsavel == 0)
                    throw new ApplicationException("Responsável: O campo 'Tipo' é obrigatório.");

                if (new ResponsavelRepository().IsResponsavelRepetidoByCPF(dto.CPF))
                    throw new ApplicationException("Responsável: Já existe um registro de responsável com o CPF informado.");

                if (new ResponsavelRepository().IsUsuarioLoginRepetido(dto.Usuario))
                    throw new ApplicationException("Responsável: Já existe um registro de usuário com o login informado.");
            }
            else
            {
                
                if (new ResponsavelRepository().ResponsavelPossuiVinculos(dto.Id))
                    throw new ApplicationException("Não é permitido excluir o registro pois o mesmo possui vínculos.");
            }
        }
    }
}