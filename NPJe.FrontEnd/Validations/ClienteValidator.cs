using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Repository.Queries;
using System;
using System.Text.RegularExpressions;

namespace NPJe.FrontEnd.Validations
{
    public class ClienteValidator
    {
        private string ValidEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public void Validate(ClienteDto dto, bool delete)
        {
            if (!delete)
            {
                if (dto.Nome.IsNullOrEmpty())
                    throw new ApplicationException("Cliente: O campo 'Nome/Razão Social' é obrigatório.");

                if (!dto.Email.IsNullOrEmpty() && !(new Regex(ValidEmailPattern, RegexOptions.IgnoreCase).IsMatch(dto.Email)))
                    throw new ApplicationException("Cliente: O campo 'Email' precisa ter um email no formato válido.");

                if (dto.Sexo != 1 && dto.Sexo != 2)
                    throw new ApplicationException("Cliente: O campo 'Sexo' é obrigatório.");

                if (!dto.CPF.IsNullOrEmpty() && dto.CPF.Length < 14)
                    throw new ApplicationException("Cliente: O campo 'CPF' deve ter 14 caracteres.");

                if (!dto.CNPJ.IsNullOrEmpty() && dto.CNPJ.Length < 18)
                    throw new ApplicationException("Cliente: O campo 'CNPJ' deve ter 14 caracteres.");

                if (!dto.Telefone.IsNullOrEmpty() && dto.Telefone.Length < 14)
                    throw new ApplicationException("Cliente: O campo 'Telefone' deve ter 14 caracteres.");

                if (!dto.Telefone.IsNullOrEmpty() && dto.Celular.Length < 16)
                    throw new ApplicationException("Cliente: O campo 'Celular' deve ter 16 caracteres.");

                if(dto.Telefone.IsNullOrEmpty() && dto.Celular.IsNullOrEmpty() && dto.Email.IsNullOrEmpty() && dto.InfoEndereco.IsNullOrEmpty())
                    throw new ApplicationException("Cliente: O cliente precisa possuir alguma informação para contato.");

                if (new ClienteRepository().IsClienteRepetidoByCPF(dto.CPF, dto.Id))
                    throw new ApplicationException("Cliente: Já existe um registro de cliente com o CPF informado.");

                if (new ClienteRepository().IsClienteRepetidoByCNPJ(dto.CNPJ, dto.Id))
                    throw new ApplicationException("Cliente: Já existe um registro de cliente com o CNPJ informado.");

                if (!dto.CNPJ.IsNullOrEmpty() && !dto.CPF.IsNullOrEmpty())
                    throw new ApplicationException("Cliente: Não é permitido preencher ambos os campos 'CNPJ' e 'CPF' para um mesmo cliente.");
            }
            
        }
    }
}