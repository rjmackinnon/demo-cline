namespace Dynacorp.Core.Models;

/// <summary>
/// Represents a widget manufactured by Dynacorp.
/// </summary>
public class Widget
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Material Material { get; set; }
    public WidgetType Type { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Material} {Type}) - ${Price:F2}";
    }
}