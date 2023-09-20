using Microsoft.EntityFrameworkCore;
public class BancoDeDados : DbContext
{
        public DbSet<Beneficio>? beneficios {get;set;}

        public BancoDeDados(DbContextOptions<BancoDeDados> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder options) {}
}