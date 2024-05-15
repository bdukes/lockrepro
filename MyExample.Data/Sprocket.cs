namespace MyExample.Data;

public record Sprocket
{
    public required Guid Id { get; init; }

    public required SprocketType Type { get; init; }
}