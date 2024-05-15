namespace MyExample.Data;

public record Widget
{
    public required Guid Id { get; init; }

    public required WidgetType Type { get; init; }
}