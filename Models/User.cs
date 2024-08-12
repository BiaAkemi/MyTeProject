using System.ComponentModel.DataAnnotations;

namespace ProjetoASPNET03MVCIdentityDb.Models
{
    // esta classe assume o "papel" de model do main da aplicação, significa que aqui, serão estabelecidadas as regras de manipulação dos dados que circularão pela aplicação
    // dados referentes à estrutura de validação/autorização de acesso à área restrita da aplicação
    public class User
    {
        // 1º passo: definir 3 props - o propósito é auxiliar na criação de um shema que possa refletir algumas da table do DB

        // este atributo/DataAnnotation é uma indicação de obrigatoriedade do valor atribuido à propriedade
        [Required]
        public string ? Name { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9])+\\.+[a-zA-Z]{2,6}$")]
        
        // 1º [] é a parte antes do @ do email, exemplo: "maydias"
        // 2º [] é a parte após o @, exemplo: "gmail"
        // 3º [] é a parte final, exemplo: ".com"
        // "\\." concatenando o ponto 
        // {2,6}$ indicar que podemos ter caracteres repetidos
        public string? Email { get; set; }
        public string? Password { get; set; }


    }
}
