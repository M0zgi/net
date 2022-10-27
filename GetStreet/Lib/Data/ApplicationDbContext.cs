using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;
using LIB.Entities;

namespace Lib.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Street> Streets { get; set; }

        public DbSet<ZipCode> ZipCodes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // метод конфигурации подключения к БД c шифрованием строки
            // паттерн Builder
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<
                    ApplicationDbContext>()
                .Build();

            //получаем из конфигурации строку подключения
            var connectionString = configuration.GetConnectionString("MyDatabase");

            optionsBuilder
                .UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // автоматически применяем все IEntityTypeConfiguration из текущей сборки
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
