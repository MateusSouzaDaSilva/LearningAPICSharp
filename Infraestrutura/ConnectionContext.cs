using LearningWebAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace LearningWebAPI.Infraestrutura
{
    public class ConnectionContext : DbContext
    {
        // Pega a tabela employee no banco e mapeia junto com a classe
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Server=localhost;" + "Port=5432; Database=employee_sample;" +
            "User Id=postgres;" + "Password=123;");
    }
}
