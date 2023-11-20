using APIEnviaEmail.Context;
using APIEnviaEmail.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string sqlServerConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlServerConnection));
builder.Services.AddHttpClient();
builder.Services.AddScoped<FuncionarioService>();
builder.Services.AddScoped<HoleriteService>();
builder.Services.AddScoped<EnvioService>();
builder.Services.AddScoped<StorageService>();
builder.Services.AddScoped<EmailService>(provider =>
{
    string smtpServer = "smtp.gmail.com";
    int smtpPort = 587;
    string smtpUsername = "gabrielsb1028@gmail.com";
    string smtpPassword = "gzff uxjo iizo feif";

    return new EmailService(smtpServer, smtpPort, smtpUsername, smtpPassword);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
