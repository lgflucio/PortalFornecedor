using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Repository.Entities.PREFEITURAS_ENTITIES;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Context
{
    public class PREFEITURASContext : DbContext
    {

        #region DbSets
        //public DbSet<UsuariosPrefeituras> UsuariosPrefeituras { get; set; }
        //public DbSet<TagsXmls> TagsXmls { get; set; }
        //public DbSet<Municipios> Municipios { get; set; }
        //public DbSet<TiposXmls> TiposXmls { get; set; }
        //public DbSet<NotasServicos> NotasServicos { get; set; }
        //public DbSet<ItensNfse> ItensNfse { get; set; }
        //public DbSet<ContainerNotas> ContainerNotas { get; set; }

        #endregion

        public PREFEITURASContext(DbContextOptions<PREFEITURASContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PREFEITURASContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optBuilder.UseSqlServer(config.GetConnectionString("PREFEITURASContext"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            optBuilder.EnableSensitiveDataLogging();
        }

        private void OnBeforeSaving()
        {
            ChangeTracker.Entries().ToList().ForEach(entry =>
            {
                if (entry.Entity is Entity trackableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        trackableEntity.Modificado = DateTime.Now;
                    }
                }
            });
        }

        public override int SaveChanges(bool acceptAllChangesOnSucess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSucess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSucess, CancellationToken cancelToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSucess, cancelToken);
        }
       
    }
}
