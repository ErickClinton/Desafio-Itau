using System.Text.Json;
using System.Text.Json.Serialization;
using DesafioInvestimentosItau.Application.Helpers.Interfaces;

namespace DesafioInvestimentosItau.Infrastructure.Helpers.Converters;

public class JsonStringDecimalConverter : JsonConverter<decimal>, IJsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDecimal();

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        => writer.WriteNumberValue(Math.Round(value, 8));
}