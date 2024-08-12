using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetoASPNET03MVCIdentityDb.Models;

var builder = WebApplication.CreateBuilder(args);

// 1� passo: adicionar o servi�o(service) que aciona a string de conex�o da aplica��o com o servidor e o db
// devidamento configurada no arquivo appsettings.json

// AddDbContext<>() - este recurso em uso, � um m�todo
//UseSqlServer() - este recurso em uso, tamb�m � um m�todo
builder.Services.AddDbContext<AppDbContext>((options) => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

//2� passo: indicar o contexto de autentica��o/autoriza��o de acesso � �rea restrita da aplica��o
//AddIdentity<>() - este recurso � um m�todo
//AddEntityFrameworksStores<>() - � um m�todo. Este recurso indica que o EntityFrameWork seja o mecanismo de armazaenamentode dados da aplica��o
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders(); 

// Add services to the container.
builder.Services.AddControllersWithViews();

/*acima, est�o indicados os servi�os necess�rio para que a aplica��o posso funcionar devidamente
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
//3� passo: adicionar o m�todo que auxilia na aplica��o dos processos de autentica��o de usu�rios para qualquer �rea restrita
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
