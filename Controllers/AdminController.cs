using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProjetoASPNET03MVCIdentityDb.Models;

namespace ProjetoASPNET03MVCIdentityDb.Controllers
{
    public class AdminController : Controller
    {
        // definir o "papel" deste controller: será responsável pelas operações CRUD do "cadastro" de dados do usuário
        // para este propósito este controller pratica o mecanismo de herança com a superclasse Controller

        //1º movimento: Definição de elementos lógicos referenciais e prática de injeção de dependência, para as operações com dados

        /* 1º passo: definir uma prop - private - para criar um elemento lógico referencial. Neste momento, é importante criar este elemento para que seja usado
         no auxílio da manipulação de dados da base, com as quais o controller vai "lidar". Para a definição deste elemento será usada a classe embarcada UserManager<> - oferexe recursos
        de operação com dados do usuário. Esta classe tem origem com AspNetCore.*/

        private UserManager<AppUser> _userManager;

        /* 2º passo:  1º passo: definir uma prop - private - para criar um elemento lógico referencial. Servirá como referência para recuperação/leitura/alteração da senha/password em estrutura hash.
           Esta prop será definida a partir do recurso de interface embarcada IPasswordHasher. */

        private IPasswordHasher<AppUser> _senhaCodificada;

        /* 3º Passo: será a definição da injeção de dependência. Para este propósito, serão usadas as props -  definidas acima - de referência 
         * a partir do construtor da classe. */

        public AdminController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> senhaCodificada)
        {
            // aqui, as props private serão acessadas e, à elas, atribuídos os valores/argumentos dados aos parametros

            _userManager = userManager;
            _senhaCodificada = senhaCodificada;
        
        }

        /* 2º Movimento: Criação das Actions - definição das operações CRUD : Create(Inserção); R:Read(leitura), U:Update(atualização / alteração), D:Delete(Exclusão).
         Aqui, serão usados recursos já definidos dentro do projeto, por exemplo:

        - AppUser: representação da table do DB, Neste contexto relação entre AppUser e User - a representação da table será responsável por "receber" do model User,
        os dados necessário para as manipulações e, posteriormente, os processo de autenticação/autorização à área restrita da aplicação.

        - User: é o model que estabelece as "regras/formato" pelos quais os dados serão operados e relacionados com o model AppUser.*/

        // 1º operação CRUD - Read (leitura) - action que será responsável pela recuperação/acesso e exibição de dados da base

        public IActionResult Index()
        {
            return View(_userManager.Users);
            /* o elemento lógico Users(método get implicito) que foi, acima, referenciado por ser um método get. Dessa forma é possível recuperar os dados da base.
            É um método exclusivo da classe UserManager<>*/       

          
        }

        // 2º operação CRUD - Create (inserção)  - action responsável pela inserção de dados na base.
        public IActionResult Create()
        {
            return View();
        }

        /* também posso usar public ViewResult Create() => View();  - chamada de expressão de função - */

        /*
         * continuando a 2º operação CRUD: sobrecarga da action para que os dados possa ser obtidos e, posteriormente,armazenados.
         * definir os atributo/requisição [HttpPost]
         */

        [HttpPost]
        // definir de forma explicít, uma tarefa assíncrona, para obter os dados e enviá-los à base

        public async Task<IActionResult> Create(User registro)
        {
            // verificar se o ModelState é válido
            if (ModelState.IsValid) // se a avaliação for TRUE
            {
                // definir um objeto - a partir do model/entity AppUser - para posteriormente, serem praticados os processos de autenticação/autorização de acesso
                // à área restrita da aplicação. Além deste propósito é preciso entender que neste action será descrito, também, o processo de armazanamento de dados na base.

                AppUser dadosUsuario = new AppUser
                {
                    UserName = registro.Name,
                    Email = registro.Email

                };

                // neste passo, será utilizado - de forma assíncrona, um método de criação/inserção de dados na base
                
                IdentityResult resultadoInsert = await _userManager.CreateAsync(dadosUsuario, registro.Password);
                // aqui está o ocnjunto de dados com as 3 props, definidas no model User

                // Agora, é neccessário, aninhar um novo if(){}, para que os rescursos embarcados de sucesso possam indicar o resultado da operação.
                if (resultadoInsert.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else {
                    // estabelecer um loop para investigar/iterar sobre eventuais erros que possam ter ocorrido
                    /* foreach (IdentityError  erro in resultadoInsert.Errors) 
                     {
                         ModelState.AddModelError("", erro.Description);

                     }*/

                    // chamada do método/ função Erros()
                    Erros(resultadoInsert);
                }
            }

            return View(registro);
        }

        /* 3º operação CRUD - Update (atualização/alteração): será responspavel pela exclusão de dados da base, pela REinserção de dados na base.
         * Desde que esteja devidamente identificado.
         Para este propóstio será necessário disponibilizar o registro.*/
        public async Task<IActionResult> Update(string idRegistro)
        {
            /* definir uma consulta - à base - para a obtenção de um registro. Para este propósito será definida uma prop para receber como valor uma consulta ao registro */

            AppUser buscarRegistro = await _userManager.FindByIdAsync(idRegistro); // avaliar o resultado da busca e verificar se o registro, realmente, existe.
            if (buscarRegistro != null)
            {
                return View(buscarRegistro);
            }
            else
            {
                return View("Index");
            
            }
        }

        // ...(continuação da 3 operação). Sobrecarga da action/método Update: para que seja, agora, possível alterar/atualizar e REenviar os dados para a base.
        [HttpPost] // atributo/requisição http que auxilia no envio de dados para  abase
        public async Task<IActionResult> Update(string idRegistro,string UserName, string Email, string password)
        {
            // repetir a consulta base
            AppUser buscarRegistro = await _userManager.FindByIdAsync(idRegistro);

            // agora é necessário lidar com as props e seus valores para serem alterados e, posteriormente, REenviados à base
            if (buscarRegistro != null)       
            {

                // observar o 1º pedaço: o valor da prop Name
                if (!string.IsNullOrEmpty(UserName))
                {
                    buscarRegistro.UserName = UserName;
                }
                else 
                {
                    ModelState.AddModelError("", "O campo nome não pode ser vazio!");
                }

                // observar o 1º pedaço: o valor da prop email
                if (!string.IsNullOrEmpty(Email))
                {
                    buscarRegistro.Email = Email;

                }
                else
                {
                    ModelState.AddModelError("", "O campo email não pode ser vazio!");                   

                }

                if (!string.IsNullOrEmpty(password))
                {
                    buscarRegistro.PasswordHash = _senhaCodificada.HashPassword(buscarRegistro, password);

                }
                else
                {
                    ModelState.AddModelError("", "O campo senha/passwaord não pode ser vazio!");
                }

                /* observar o 4º pedaço: consiste em observar e avaliar os dados de nome, email e senha, agora, conjunto.Para que seja possível 
                 * REenviá-los à base e REarmazena-los - de forma assíncrona.*/
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(password))
                {

                    IdentityResult resultadoOp = await _userManager.UpdateAsync(buscarRegistro); // neste ponto a alteração/atualização ocorre.

                    // verificar sucesso da operação 
                    if (resultadoOp.Succeeded) 
                    {
                        return RedirectToAction("Index");

                    } else 
                    {
                        // chamada do método de observação de erros
                        Erros(resultadoOp);
                    }
                }
            }
            else 
            {
                ModelState.AddModelError("", "Usuário não encontrado.");

            }
            return View(buscarRegistro);
        
        }



        /* 4º operação CRUD - Delete (exclusão) : será responsável pela exclusão de dados da base, desde que o registro seja devidamente identificado */

        [HttpPost]
        // de forma explicita será definida a tarefa assíncrona de exclusão de registro

        public async Task<IActionResult> Delete(string idRegistro)
        {
            // definir uma prop buscar o registro na base

            AppUser buscaRegistro = await _userManager.FindByIdAsync(idRegistro);

            // verificar o resultado da busca
            if (buscaRegistro != null)
            {

                IdentityResult resultadoExclusao = await _userManager.DeleteAsync(buscaRegistro);

                if (resultadoExclusao.Succeeded)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    /* no próximo passo vamos definir uma função que investiga potenciais erros na exclusão de regustro. 
                     * O nome da função/método será erros(){} */

                    Erros(resultadoExclusao);

                }
            }
            else             
            {
                ModelState.AddModelError("", "Usuário, infelizmente, não encontrado");
            
            }    

            return View("Index", _userManager.Users);
                  
            
        }

        // definir método erros()

        private void Erros(IdentityResult ocorrenciasErros) 
        {
            // estabelecer um loop para iterar sobre todas as possíveis ocorrências de eventuais erros na aplicação

            foreach (IdentityError erro in ocorrenciasErros.Errors)
            {
                ModelState.AddModelError("", erro.Description);

            }
        }




    }


}
