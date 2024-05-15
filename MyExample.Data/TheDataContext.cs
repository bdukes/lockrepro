using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MyExample.Data;

public partial class TheDataContext(DbContextOptions<TheDataContext> options) : DbContext(options)
{
    public DbSet<Sprocket> Sprockets => this.Set<Sprocket>();
    public DbSet<Widget> Widgets => this.Set<Widget>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);
        ConfigureEnumTypes(configurationBuilder);
    }

    static partial void ConfigureEnumTypes(ModelConfigurationBuilder modelConfigurationBuilder);

    private static void ConfigureEnumType<T>(ModelConfigurationBuilder configurationBuilder, int maxLength)
        where T : struct, Enum
    {
        configurationBuilder
            .Properties<T>()
            .AreUnicode(false)
            .HaveMaxLength(maxLength)
            .HaveConversion<EnumToStringConverter<T>>();
    }
}