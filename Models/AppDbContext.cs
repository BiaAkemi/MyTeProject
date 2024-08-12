using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjetoASPNET03MVCIdentityDb.Models
{   // esta classe proporciona o contexto/referência do DB SQL Server, aqui, representado

    // para este propósito será praticado o mecanismo de herança entre as classes: AppDbContext e a classe embarcada/nativa IdentityCintext

    // o o bjetivo é oferececer a subclasse todos os recursos necessários para o contexto/referência de integração entre as aplicações front-end e back-end

    public class AppDbContext : IdentityDbContext<AppUser>
        // a especificidade do elemento genérico IdentityDbContext<> é dado pela representação/model/entity AppUser
    {
        // definir o constutor da classe porque é necessário "priorizar" a referência do contexto, aqui, definido

        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }
    }
}
