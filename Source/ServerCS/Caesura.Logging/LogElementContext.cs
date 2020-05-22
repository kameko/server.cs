
namespace Caesura.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    
    public class LogElementContext : DbContext
    {
        public DbSet<LogElement> LogElements { get; set; }
        
        #nullable disable
        public LogElementContext() : base()
        {
            
        }
        
        public LogElementContext(DbContextOptions<LogElementContext> options) : base(options)
        {
            
        }
        #nullable restore
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
