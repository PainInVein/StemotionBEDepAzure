using STEMotion.Application.Extensions;
using STEMotion.Infrastructure.Configuration;
using STEMotion.Presentation.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddInfranstructureToApplication(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//payOS
builder.Services.AddPayOSConfiguration(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5175", "http://localhost:5174", "http://localhost:5173", "https://payment-testing-fe.vercel.app", "https://fe-ste-motion.vercel.app", "https://fe-ste-motion.vercel.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();
app.UseInfrastructure();
app.ApplyMigrations();

app.UseCors("AllowReactApp");

app.Run();
