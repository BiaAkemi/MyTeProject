// uso da diretiva Identity
using Microsoft.AspNetCore.Identity;

namespace ProjetoASPNET03MVCIdentityDb.Models
{
    // esta classe assume o "papel" de representaçõ/entity  da table do Db. Para tal proposito, será praticado o mecanismo de herança com a classe IdentityUser
    public class AppUser: IdentityUser //superclasse/pai/base
    {
        // a table do DB será criada a partir deste model. Portanto, toda e qualquer propriedade que, aqui descrita será definida na table do DB como uma coluna que a compõe
        // absolutamente NADA! não será definida, neste model, nenhuma propriedade. Em função do mecanismo de herança, como a IdentityUser- algumas propriedades padrão(default)
        //serão disponibilizadas - sem a necessidade de referencia-la. Por exemplo: Id, Username, Name,Email, HashPassword


    }
}
