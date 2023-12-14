
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
{
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<FileStorage> FileStorages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        
        base.OnModelCreating(modelBuilder);
    }

}
