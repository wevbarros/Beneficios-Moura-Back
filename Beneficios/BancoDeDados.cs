using Microsoft.EntityFrameworkCore;
public class BancoDeDados : DbContext
{
    public BancoDeDados (DbContextOptions<BancoDeDados> options)
        : base(options) {}
    public DbSet<Beneficio>? beneficios {get;set;}
    public DbSet<User> Users {get;set;}
    public DbSet<Level> Levels {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<User>()
        .HasOne(u => u.Level)
        .WithMany(l => l.Users)
        .HasForeignKey(u => u.CodLevel);
    }
}