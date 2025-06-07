using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioInvestimentosItau.Domain.Entities;

public class PositionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long UserId { get; set; }
    public long AssetId { get; set; }

    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitLoss { get; set; }

    [Required] 
    public UserEntity User { get; set; } = null!;
    
    [Required]
    public AssetEntity Asset { get; set; } = null!;
}