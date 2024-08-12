using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetoASPNET03MVCIdentityDb.Models;

var builder = WebApplication.CreateBuilder(args);

// 1º passo: adicionar o serviço(service) que aciona a string de conexão da aplicação com o servidor e o db
// devidamento configurada no arquivo appsettings.json

// AddDbContext<>() - este recurso em uso, é um método
//UseSqlServer() - este recurso em uso, também é um método
builder.Services.AddDbContext<AppDbContext>((options) => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

//2º passo: indicar o contexto de autenticação/autorização de acesso à área restrita da aplicação
//AddIdentity<>() - este recurso é um método
//AddEntityFrameworksStores<>() - é um método. Este recurso indica que o EntityFrameWork seja o mecanismo de armazaenamentode dados da aplicação
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders(); 

// Add services to the container.
builder.Services.AddControllersWithViews();

/*acima, estão indicados os serviços necessário para que a aplicação posso funcionar devidamente
 * 
 * 
 *************/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
//3º passo: adicionar o método que auxilia na aplicação dos processos de autenticação de usuários para qualquer área restrita
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
