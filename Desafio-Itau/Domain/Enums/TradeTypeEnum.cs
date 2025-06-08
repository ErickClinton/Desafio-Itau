using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DesafioInvestimentosItau.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TradeTypeEnum
{
    [EnumMember(Value = "buy")]
    Buy,

    [EnumMember(Value = "sell")]
    Sell
}