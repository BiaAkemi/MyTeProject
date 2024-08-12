using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoASPNET03MVCIdentityDb.Models;
using System.Diagnostics;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    // este controller será responsável por exercer controle e influência sobre a área restrita da aplicação.
    public class HomeController : Controller
    {
        // objeto referencial definido a partir da interface ILogger, seu propósito é auxiliar na obtenção de informações de log a respeito do comportamento da aplicação

       
        private readonly ILogger<HomeController> _logger;

        // 1º passo: deifnir um auxliador, objeto referencial,  a partir da classe UserManager
        private UserManager<AppUser> _userManager;
        // este construtor da classe onde são definidas as DI's
       
        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager)
        {
            _logger = logger;

            _userManager = userManager;
        }
        // definição das actions que se relacionam com este controlle

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //3º passo: criar uma nova action para "controlar" as operações em relação a view, que será definida para a área restrita da aplicação

        [Authorize]/* o uso do atributo [Authorize] define que a action precisa ser acessada por um contexto de autenticação e autorização dadas à um determinado
                    * conjunto de dados - credenciais de acesso.*/

        public async Task<IActionResult> Private()
        {
            AppUser consultaUser = await _userManager.GetUserAsync(HttpContext.User);

            /* o uso do recurso HttpContext -> método get implicíto
             * uso do recurso User ->n método set implicíto
             * 
             * criar uma nova prop para receber como valor uma mensagem de boas-vindas associado ao nome do usuário.*/

            string mensagem = "Ola " + consultaUser.UserName + " você está na área restrita da aplicação";

            return View((object)mensagem);
         
        
        
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}
