using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.JsonConverts;

public class ShoppingCartConverter : JsonConverter<ShoppingCart>
{
public override ShoppingCart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
{
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var rootElement = jsonDocument.RootElement;

    var id = rootElement.GetProperty("id").GetGuid();
    var userName = rootElement.GetProperty("userName").GetString()!;
    
    var shoppingCart = ShoppingCart.Create(id, userName);
    
    // Debug: List ALL fields
    var allFields = typeof(ShoppingCart).GetFields(
        BindingFlags.Public | BindingFlags.NonPublic | 
        BindingFlags.Instance | BindingFlags.Static);
    
    // Try to find _items with all possible combinations
    var itemsField = typeof(ShoppingCart).GetField("_items", 
        BindingFlags.NonPublic | BindingFlags.Instance);
    
    if (itemsField == null)
    {
        itemsField = allFields.FirstOrDefault(f => 
            f.Name.Contains("items") && 
            f.FieldType == typeof(List<ShoppingCartItem>));
    }
        
    if (itemsField == null)
        throw new InvalidOperationException($"Could not find _items field. Available fields: {string.Join(", ", allFields.Select(f => f.Name))}");
        
    var itemsList = (List<ShoppingCartItem>)itemsField.GetValue(shoppingCart)!;
    
    // Deserialize items
    if (rootElement.TryGetProperty("items", out var itemsElement) && 
        itemsElement.ValueKind == JsonValueKind.Array)
    {
        foreach (var itemElement in itemsElement.EnumerateArray())
        {
            var itemJson = itemElement.GetRawText();
            var item = JsonSerializer.Deserialize<ShoppingCartItem>(itemJson, options);
            if (item != null)
            {
                itemsList.Add(item);
            }
        }
    }

    return shoppingCart;
}
    public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        writer.WriteString("id", value.Id.ToString());
        writer.WriteString("userName", value.UserName);
        
        writer.WritePropertyName("items");
        writer.WriteStartArray();
        
        foreach (var item in value.Items)
        {
            JsonSerializer.Serialize(writer, item, options);
        }
        
        writer.WriteEndArray();
        
        writer.WriteEndObject();
    }
}