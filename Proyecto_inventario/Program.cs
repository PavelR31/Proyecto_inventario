using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor.
builder.Services.AddControllersWithViews();

// Configura el contexto de la base de datos MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0)) // Aseg�rate de que coincida con la versi�n de tu servidor MySQL
    )

);

// Configuraci�n de autenticaci�n
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Index";
    });

var app = builder.Build();

// Configura el pipeline HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Habilita la autenticaci�n
app.UseAuthentication();

// Habilita la autorizaci�n
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=View}/{id?}");

app.Run();

