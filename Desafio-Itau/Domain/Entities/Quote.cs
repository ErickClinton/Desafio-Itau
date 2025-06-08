using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioInvestimentosItau.Domain.Entities;

public class QuoteEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string AssetCode { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime Timestamp { get; set; }

}