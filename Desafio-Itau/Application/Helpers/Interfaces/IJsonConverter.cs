using System.Text.Json;

namespace DesafioInvestimentosItau.Application.Helpers.Interfaces;

public interface IJsonConverter<T>
{
    T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
    void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
}