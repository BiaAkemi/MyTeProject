using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetoASPNET03MVCIdentityDb.Models;
using System.Diagnostics;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    // este controller ser� respons�vel por exercer controle e influ�ncia sobre a �rea restrita da aplica��o.
    public class HomeController : Controller
    {
        // objeto referencial definido a partir da interface ILogger, seu prop�sito � auxiliar na obten��o de informa��es de log a respeito do comportamento da aplica��o

       
        private readonly ILogger<HomeController> _logger;

        // 1� passo: deifnir um auxliador, objeto referencial,  a partir da classe UserManager
        private UserManager<AppUser> _userManager;
        // este construtor da classe onde s�o definidas as DI's
       
        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager)
        {
            _logger = logger;

            _userManager = userManager;
        }
        // defini��o das actions que se relacionam com este controlle

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //3� passo: criar uma nova action para "controlar" as opera��es em rela��o a view, que ser� definida para a �rea restrita da aplica��o

        [Authorize]/* o uso do atributo [Authorize] define que a action precisa ser acessada por um contexto de autentica��o e autoriza��o dadas � um determinado
                    * conjunto de dados - credenciais de acesso.*/

        public async Task<IActionResult> Private()
        {
            AppUser consultaUser = await _userManager.GetUserAsync(HttpContext.User);

            /* o uso do recurso HttpContext -> m�todo get implic�to
             * uso do recurso User ->n m�todo set implic�to
             * 
             * criar uma nova prop para receber como valor uma mensagem de boas-vindas associado ao nome do usu�rio.*/

            string mensagem = "Ola " + consultaUser.UserName + " voc� est� na �rea restrita da aplica��o";

            return View((object)mensagem);
         
        
        
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}
