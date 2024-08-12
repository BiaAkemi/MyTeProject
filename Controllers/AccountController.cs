using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProjetoASPNET03MVCIdentityDb.Models;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    // este controller será responsável pela modaludade de autenticação/autorização de usuários para acesso à área restrita da aplicação

    [Authorize] // este atributo faz com que as estruturas lógicas e instruções relacionadas à esta classe se tornem inacessíveis por qualquer outra instrução sem autorização.
    // Significa que qualquer instrução que não faça parte desta classe, não pode acessar nada que, está descrita.
    public class AccountController : Controller
    {
        /*        
         * 1º MOVIMENTO : CONFIGURAÇÃO / DISPONIBILIDADE DOS RECURSOS DE ACESSO À ÁREA RESTRITA - LOGIN        
         */

        //1º passo: definir 2 "auxiliadores" - objetos referenciais - para a DI (dependence injection)

        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _gerenciadorAcesso;  // este 2º objeto referencial nada mais é do que um "gerenciador de recursos de acesso" à áreas restritas de uma aplicação.

        // 2º passo: estabelecer o construtor da classe, de forma customizada; definindo as DI's (dependences injections)

        public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> gerenciadorAcesso)
        {
            _userManager = userManager;
            _gerenciadorAcesso = gerenciadorAcesso;

        }

        /* 
         * 2º MOVIMENTO: DEFINIÇÃO DAS ACTIONS E OPERAÇÕES PARA O FUNCIONAMENTO DO LOGIN 
         */

        // 1º passo: estabelecer a action Login(){} para gerar um objeto e fazer acesso as "endereço/rota" no qual será indicada a necessidade da inserção das credenciais de acesso - 1º estágio da estrutura de login

        [AllowAnonymous] // este atributo/annotation permite o acesso as funcionalidades descritas na action sem a necessidade de autenticação ou autorização prévia

        public IActionResult Login(string returnUrl)
        {
            // praticar a instância direta do model login e gerar um objeto do qual se faça uso de suas propriedades
            Login login = new Login();

            // fazer uso do objeto para acessar a prop de url da classe Login

            login.ReturnUrl = returnUrl;

            return View(login);
        }

        //2º passo: definir, de forma explicita, a tarefa assincrona para o envio de dados e mais um "estágio" da estrutura de login

        [HttpPost] // atribu/requisição de envio de daos
        [AllowAnonymous]
        [ValidateAntiForgeryToken] /* este atributo "impede" a autenticação/autorização desta funcionalidade entre elementos lógicos anônimos - 
                                * significa que o processo processo de autenticação lógicos automatizados, robos - está "barrado"*/
        public async Task<IActionResult> Login(Login logar)
        {
            // observar as validações do model login, aplicados à View Login
            if (ModelState.IsValid)
            {
                AppUser consulta = await _userManager.FindByEmailAsync(logar.Email);/* consulta estabelecida para observar se o email dado pelo usuário - oriundo
                                                                                     * do parametro logar - existe na base, devidamente armazenado.*/


                // avaliar a consulta 
                if (consulta != null)
                {
                    // se a consulta for considerada TRUE - no momento em que ocorre a consulta, o email consultado está logado no sistema.
                    // se existe um email que está "logado" no sistema, o método abaixo encerrará deste email/usuário.
                    // Dessa forma o processo de autenticação pode ocorrer sem problemas.

                    await _gerenciadorAcesso.SignOutAsync();

                    // fazer uso da classe embarcada SingInResult para então operar com o resultado do processo de autenticação do usuário
                    Microsoft.AspNetCore.Identity.SignInResult resultado = await _gerenciadorAcesso.PasswordSignInAsync(consulta,logar.Password,false, false);

                    /* aqui acima, temos as seguintes referências:
                     * uso da prop email (com referência à prop consulta), a partir do modelLogin
                     * uso da prop Password, a apartir do model Password, observado se ambos, email e senha estão em conformidade com o model login
                     * o 1º false é para indicar que não é necessário persistir à sessão de acesso
                     * o 2º false impede qualquer bloqueio de autenticação/acesso - caso ocorra falha ao tentar autenticar qualquer usuário.*/

                    // fazer acesso a var resultado e verificar se o valor atribuido resulta em sucesso - a autenticação
                     
                    if(resultado.Succeeded) 
                    {
                        /*abaixo, está indicado o "endereço" da área restrita da aplicaç, será uma action que, porteriormente, trabalharemos no HomeController
                         com sua respectiva view. Neste passo é dado o acesso/autorização, depois da autenticação, à área restrita.
                         ?? este é  o operador de coalescencia nula: prop ?? "----" - se o calor do retorno da operação for diferente de nulo, o operador de 
                         coalescencia retorna o valor referenciado do lado esquerdo, caso contrário, retorna o valor do lado direito.*/

                        return Redirect(logar.ReturnUrl ?? "/Home/Private"); // aqui somos redirecionados a partir de uma rota/endereço específico.

                    }
                    
                }
                ModelState.AddModelError(nameof(logar.Email), "Sua autenticação falhou. Tente novamente!");

            }

            return View(logar);
        }


        //3º passo: definir uma nova action para que o usuário, uma vez logado, possa sair da área restrita e ser redirecionado para uma área pública.

        public async Task<IActionResult> Logout() 
        {
            await _gerenciadorAcesso.SignOutAsync();

            // indicar a " rota" pela qual o usuário será redirecionado quando escolher sair da área restrita.

            return RedirectToAction("Index", "Home"); // aqui, somos redirecionados  para uma action e seu controller 
        
        }

         
    }
}
