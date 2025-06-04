using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioInvestimentosItau.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<AssetEntity> Assets => Set<AssetEntity>();
    public DbSet<TradeEntity> Trades => Set<TradeEntity>();
    public DbSet<QuoteEntity> Quotes => Set<QuoteEntity>();
    public DbSet<PositionEntity> Positions => Set<PositionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.BrokerageFee).HasColumnType("decimal(5,2)");
        });

        modelBuilder.Entity<AssetEntity>(entity =>
        {
            entity.ToTable("asset");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<TradeEntity>(entity =>
        {
            entity.ToTable("trade");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.BrokerageFee).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Timestamp).IsRequired();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Trades)
                  .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Asset)
                  .WithMany(a => a.Trades)
                  .HasForeignKey(e => e.AssetId);
        });

        modelBuilder.Entity<QuoteEntity>(entity =>
        {
            entity.ToTable("quote");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();

            entity.HasOne(e => e.Asset)
                  .WithMany(a => a.Quotes)
                  .HasForeignKey(e => e.AssetId);
        });

        modelBuilder.Entity<PositionEntity>(entity =>
        {
            entity.ToTable("position");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.AveragePrice).HasColumnType("decimal(18,4)");
            entity.Property(e => e.ProfitLoss).HasColumnType("decimal(18,4)");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Positions)
                  .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Asset)
                  .WithMany(a => a.Positions)
                  .HasForeignKey(e => e.AssetId);
        });
    }
}