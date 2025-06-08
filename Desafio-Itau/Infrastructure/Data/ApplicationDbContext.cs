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
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150).HasColumnName("email");
            entity.Property(e => e.BrokerageFee).HasColumnType("decimal(5,2)").HasColumnName("brokerage_fee");
        });

        modelBuilder.Entity<AssetEntity>(entity =>
        {
            entity.ToTable("asset");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10).HasColumnName("code");
        });

        modelBuilder.Entity<TradeEntity>(entity =>
        {
            entity.ToTable("trade");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Quantity).IsRequired().HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,4)").IsRequired().HasColumnName("unit_price");
            entity.Property(e => e.Type).IsRequired().HasColumnName("type");
            entity.Property(e => e.BrokerageFee).HasColumnType("decimal(10,2)").HasColumnName("brokerage_fee");
            entity.Property(e => e.Timestamp).IsRequired().HasColumnName("timestamp");
            
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            
            entity.Property(e => e.AssetId)
                .HasColumnName("asset_id");
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Trades)
                  .HasForeignKey(e => e.UserId)
                  .HasConstraintName("fk_trade_user_id");

            entity.HasOne(e => e.Asset)
                  .WithMany(a => a.Trades)
                  .HasForeignKey(e => e.AssetId)
                  .HasConstraintName("fk_trade_asset_id");
        });

        modelBuilder.Entity<QuoteEntity>(entity =>
        {
            entity.ToTable("quote");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,4)").IsRequired().HasColumnName("unite_price");
            entity.Property(e => e.Timestamp).IsRequired().HasColumnName("timestamp");

            entity.Property(e => e.AssetId)
                .HasColumnName("asset_id");
            
            entity.HasOne(e => e.Asset)
                  .WithMany(a => a.Quotes)
                  .HasForeignKey(e => e.AssetId)
                  .HasConstraintName("fk_quote_asset_id");
        });

        modelBuilder.Entity<PositionEntity>(entity =>
        {
            entity.ToTable("position");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Quantity).IsRequired().HasColumnName("quantity");
            entity.Property(e => e.AveragePrice).HasColumnType("decimal(18,4)").HasColumnName("average_price");
            entity.Property(e => e.ProfitLoss).HasColumnType("decimal(18,4)").HasColumnName("profit_loss");
            
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.AssetId)
                .HasColumnName("asset_id");
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Positions)
                  .HasForeignKey(e => e.UserId)
                  .HasConstraintName("fk_position_user_id");

            entity.HasOne(e => e.Asset)
                  .WithMany(a => a.Positions)
                  .HasForeignKey(e => e.AssetId)
                  .HasConstraintName("fk_position_asset_id");
        });
    }
}