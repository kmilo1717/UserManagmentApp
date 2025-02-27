using UsersManagmentApp.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<PersonaDAL>();
builder.Services.AddScoped<PersonaService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
