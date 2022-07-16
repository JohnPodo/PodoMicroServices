using Microsoft.EntityFrameworkCore;
using PodoMicroServices.Models.LogModels;

namespace PodoMicroServices.DAL
{
    public class PodoMicroServiceContext : DbContext
    {
        public PodoMicroServiceContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
        {

        }

        //public PodoMicroServiceContext()
        //{

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-D03UDVQ\\SQLEXPRESS;Database=MicroServices;Trusted_Connection=True;");
        //}

        public DbSet<Log>? Logs { get; set; }
        public DbSet<Models.FileModels.File>? Files { get; set; }
    }
}
