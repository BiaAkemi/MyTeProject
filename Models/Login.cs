using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ProjetoASPNET03MVCIdentityDb.Models
{
    /*  esta classe assume o papel de ser um instrumento lógico que opera como um " conjunto de propriedades credenciais" - cmo se 
     *  fosse um cartão de acesso com informações, de um usuário, para área restrita"*/

    public class Login
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }

        // por padrão, o AspnetCore vai - sempre - adotar uma URL pata o acesso ao "espaço"(tela/view) de inserção de credenciais.

        //http://localhost:xxxx/NomeQualquer/Login

        /* ao utilizar a prop ReturnUrl, nós estamos dizendo que é possível, se for necessário,
        customizar a rota para esta área restrita, ou seja, a aplicação pode "fugir" do padrão estabelecido pelo AspNetCore.*/          
    }
}
