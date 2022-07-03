using MicroServices.LogService.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.LogService.DAL
{
    public class LogContext : DbContext
    {

        public DbSet<Log>? Logs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlite(@$"DataSource={AppDomain.CurrentDomain.BaseDirectory}/Logger.db;");
        }
    }
}
