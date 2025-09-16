using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => //esse opt é igual a options do AddDbContext, ou seja, é um apelido que esta sendo usado para representar as opções que 
// serão passadas para o DbContext
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();                         //Adiciona o serviço de CORS

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "https://localhost:3000"));

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync(); //Aqui aplica as migrations pendentes no banco de dados
    await DbInitializer.SeedData(context); //Aqui popula o banco de dados com dados iniciais
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "um erro ocorreu durante a migracao");
}

app.Run();
