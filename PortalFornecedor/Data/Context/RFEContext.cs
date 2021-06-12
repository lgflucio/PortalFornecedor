using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Repository.Entities.RFE_ENTITIES;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Context
{
    public class RFEContext : DbContext
    {
        public RFEContext(DbContextOptions<RFEContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        #region DbSet's
        public DbSet<RFE_CIA> RFE_CIA { get; set; }
        public DbSet<RFE_CERTIFICADOS> RFE_CERTIFICADOS { get; set; }
        public DbSet<RFE_HISTORICO> RFE_HISTORICO { get; set; }
        public DbSet<RFE_ITEM_NFSE> RFE_ITEM_NFSE { get; set; }
        public DbSet<RFE_LOG> RFE_LOG { get; set; }
        public DbSet<RFE_NFSE> RFE_NFSE { get; set; }
        public DbSet<RFE_REPOSITORIO> RFE_REPOSITORIO { get; set; }
        public DbSet<RFE_MUNICIPIOS_NFSE> RFE_MUNICIPIOS_NFSE { get; set; }
        public DbSet<RFE_PORTAL_HEADER_NOTA> RFE_PORTAL_HEADER_NOTA { get; set; }
        public DbSet<RFE_PORTAL_ITEM_NOTA> RFE_PORTAL_ITEM_NOTA { get; set; }
        public DbSet<RFE_PORTAL_FORNECEDOR> RFE_PORTAL_FORNECEDOR { get; set; }
        public DbSet<RFE_ANEXOS_NFSE> RFE_ANEXOS_NFSE { get; set; }
        public DbSet<RFE_PORTAL_CONVERSOES> RFE_PORTAL_CONVERSOES { get; set; }


        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RFEContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optBuilder.UseSqlServer(config.GetConnectionString("RFEContext"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            
            optBuilder.UseLoggerFactory(LoggerFactory.Create(x=>x.AddConsole())).EnableSensitiveDataLogging();
        }

        private void OnBeforeSaving()
        {
            ChangeTracker.Entries().ToList().ForEach(entry =>
            {
                if (entry.Entity is BASE_TABLE trackableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        trackableEntity.EDITOR = "Busca_Automatica_Municipios";
                        trackableEntity.MODIFICADO = DateTime.Now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        trackableEntity.MODIFICADO = DateTime.Now;
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
