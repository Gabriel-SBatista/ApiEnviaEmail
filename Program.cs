using APIEnviaEmail.Context;
using APIEnviaEmail.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
    string smtpServer = builder.Configuration.GetSection("SMTP").GetRequiredSection("Server").Value;
    int smtpPort = int.Parse(builder.Configuration.GetSection("SMTP").GetRequiredSection("Porta").Value);
    string smtpUsername = builder.Configuration.GetSection("SMTP").GetRequiredSection("Username").Value;
    string smtpPassword = builder.Configuration.GetSection("SMTP").GetRequiredSection("Password").Value;

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
