using APIEnviaEmail.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEnviaEmail.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Envio> Envios { get; set; }
}
