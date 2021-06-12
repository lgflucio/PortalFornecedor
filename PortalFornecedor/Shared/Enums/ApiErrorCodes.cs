using System.ComponentModel;

namespace Shared.Enums
{
    public enum ApiErrorCodes
    {
        /// <summary>
        /// Erro inesperado.
        /// </summary>
        [Description("Erro inesperado.")]
        UNEXPC,

        /// <summary>
        /// O Id informado é inválido.
        /// </summary>
        [Description("O Id informado é inválido.")]
        INVID,

        /// <summary>
        /// Recurso não encontrado.
        /// </summary>
        [Description("Recurso não encontrado.")]
        NOTFND,

        /// <summary>
        /// Erro ao alterar senha.
        /// </summary>
        [Description("Erro ao alterar senha.")]
        CHGPASS,

        /// <summary>
        /// A ApiKey informada é inválida.
        /// </summary>
        [Description("A ApiKey informada é inválida.")]
        INVAPKY,

        /// <summary>
        /// Senha já utilizada.
        /// </summary>
        [Description("Senha já utilizada.")]
        ALUPAS,

        /// <summary>
        /// Erro genérico.
        /// </summary>
        [Description("Erro genérico.")]
        GENRC,

        /// <summary>
        /// O domínio informado é inválido.
        /// </summary>
        [Description("O domínio informado é inválido.")]
        INVDOM,

        /// <summary>
        /// Usuário e domínio devem ser preenchidos corretamente.
        /// </summary>
        [Description("Usuário e domínio devem ser preenchidos corretamente.")]
        INVUSDOM,

        /// <summary>
        /// Usuário ou senha inválidos.
        /// </summary>
        [Description("Usuário ou senha inválidos.")]
        INVUSPASS,

        /// <summary>
        /// Senha expirada.
        /// </summary>
        [Description("Senha expirada.")]
        EXPPASS,

        /// <summary>
        /// Tentativas excedidas de autenticação, aguarde e tente novamente mais tarde.
        /// </summary>
        [Description("Tentativas excedidas de autenticação, aguarde e tente novamente mais tarde.")]
        LCKLOG,

        /// <summary>
        /// Sem permissão para acessar o recurso.
        /// </summary>
        [Description("Sem permissão para acessar o recurso.")]
        NOTALLW,

        /// <summary>
        /// Login já cadastrado.
        /// </summary>
        [Description("Login já cadastrado.")]
        ALULOG,

        /// <summary>
        /// Erro ao fazer login.
        /// </summary>
        [Description("Erro ao fazer login.")]
        ERRLOG,

        /// <summary>
        /// Erro ao alterar senha pelo Identity na criação do usuário.
        /// </summary>
        [Description("Erro ao alterar senha pelo Identity na criação do usuário.")]
        CHGPASCRIDNT,

        /// <summary>
        /// Erro ao criar usuário pelo Identity.
        /// </summary>
        [Description("Erro ao criar usuário pelo Identity.")]
        CRUSIDNT,

        /// <summary>
        /// Usuario de rede não pode ser alterado através do EzAuth.
        /// </summary>
        [Description("Usuario de rede não pode ser alterado através do EzAuth.")]
        CHUSEZAUTH,

        /// <summary>
        /// O Id do sistema informado é inválido.
        /// </summary>
        [Description("O Id do sistema informado é inválido.")]
        INVSID,

        /// <summary>
        /// O usuário não possui e-mail cadastrado.
        /// </summary>
        [Description("O usuário não possui e-mail cadastrado.")]
        NOTFNDEMUS,

        /// <summary>
        /// Login e e-mail não conferem.
        /// </summary>
        [Description("Login e e-mail não conferem.")]
        LOGEMAILUS,

        /// <summary>
        /// Erro no reset de senha pelo Identity.
        /// </summary>
        [Description("Erro no reset de senha pelo Identity.")]
        RESTPASS,

        /// <summary>
        /// O login informado é inválido.
        /// </summary>
        [Description("O login informado é inválido.")]
        INVLOG,

        /// <summary>
        /// Usuário não encontrado para o login informado.
        /// </summary>
        [Description("Usuário não encontrado para o login informado.")]
        USNOTFNDBYLOG,

        /// <summary>
        /// O CPF informado é inválido.
        /// </summary>
        [Description("O CPF informado é inválido.")]
        INVCPF,

        /// <summary>
        /// Usuário não encontrado para o CPF informado.
        /// </summary>
        [Description("Usuário não encontrado para o CPF informado.")]
        USNOTFNDBYCPF,

        /// <summary>
        /// Preencha os logins para continuar.
        /// </summary>
        [Description("Preencha os logins para continuar.")]
        LOGSREQ,

        /// <summary>
        /// Nenhuma ApiKey encontrada para o usuário informado.
        /// </summary>
        [Description("Nenhuma ApiKey encontrada para o usuário informado.")]
        APKYNOTFNDBYUS,

        /// <summary>
        /// Nenhum usuário encontrado com o id informado.
        /// </summary>
        [Description("Nenhum usuário encontrado com o id informado.")]
        USNOTFNDBYID,

        /// <summary>
        /// Nenhum sistema encontrado com o id informado.
        /// </summary>
        [Description("Nenhum sistema encontrado com o id informado.")]
        SISNOTFNDBYID,

        /// <summary>
        /// Não há nenhuma ApiKey registrada para esse sistema.
        /// </summary>
        [Description("Não há nenhuma ApiKey registrada para esse sistema.")]
        NOAPKYSIS,

        /// <summary>
        /// Nenhumm usuário encontrado com a ApiKey informada.
        /// </summary>
        [Description("Nenhumm usuário encontrado com a ApiKey informada.")]
        NOUSAPKY,

        /// <summary>
        /// Senha entre uma das 5 últimas utilizadas anteriormente, escolha outra senha.
        /// </summary>
        [Description("Senha entre uma das 5 últimas utilizadas anteriormente, escolha outra senha.")]
        ALUPASFIVELAST,

        /// <summary>
        /// Usuário para autenticação no contexto não deve ser nulo.
        /// </summary>
        [Description("Usuário para autenticação no contexto não deve ser nulo.")]
        USREQCONTXT,

        /// <summary>
        /// Não foi possível encontrar as configurações do Token JWT.
        /// </summary>
        [Description("Não foi possível encontrar as configurações do Token JWT.")]
        APKYCONFGREQ,

        /// <summary>
        /// Api-key sem permissão para esse sistema.
        /// </summary>
        [Description("Api-key sem permissão para esse sistema.")]
        APKYNOTALLWDSIS,

        /// <summary>
        /// Erros de validação no ModelState.
        /// </summary>
        [Description("Erros de validação no ModelState.")]
        MODNOTVALD,

        /// <summary>
        /// Versão da API não suportada.
        /// </summary>
        [Description("Versão da API não suportada.")]
        NOTSUPAPIVERS,

        /// <summary>
        /// O servidor AD não está operacional.
        /// </summary>
        [Description("O servidor AD não está operacional.")]
        SRVADNOTAVAI,

        /// <summary>
        /// Registro já se encontra na base de dados.
        /// </summary>
        [Description("Registro já se encontra na base de dados")]
        REGINBD
    }
}