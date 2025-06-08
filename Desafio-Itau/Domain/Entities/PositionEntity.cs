using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioInvestimentosItau.Domain.Entities;

public class PositionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long UserId { get; set; }
    public string AssetCode { get; set; }

    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitLoss { get; set; }

    [Required] 
    public UserEntity User { get; set; } = null!;
    
    public void UpdatePosition(int totalQuantity, decimal totalValue)
    {
        if (totalQuantity <= 0)
            throw new ArgumentException("Total quantity must be greater than zero.");

        Quantity = totalQuantity;
        AveragePrice = totalValue / totalQuantity;
    }
}