using Microsoft.EntityFrameworkCore;
using MyExample.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<TheDataContext>(builder.Configuration.GetConnectionString(nameof(TheDataContext)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", (TheDataContext context) => context.Widgets.AsAsyncEnumerable())
    .WithName("GetAll")
    .WithOpenApi();

app.MapGet(
        "/{id:guid}",
        (TheDataContext context, Guid id, CancellationToken cancellationToken)
            => context.Widgets.SingleOrDefaultAsync(x => x.Id == id, cancellationToken))
    .WithName("GetOne")
    .WithOpenApi();

app.MapGet("/great", (TheDataContext context) => context.Widgets.Where(s => s.Type == WidgetType.Great).AsAsyncEnumerable())
    .WithName("GetAllGreat")
    .WithOpenApi();

app.MapGet("/okay", (TheDataContext context) => context.Widgets.Where(s => s.Type == WidgetType.Okay).AsAsyncEnumerable())
    .WithName("GetAllOkay")
    .WithOpenApi();

app.MapGet("/unusual", (TheDataContext context) => context.Widgets.Where(s => s.Type == WidgetType.Unusual).AsAsyncEnumerable())
    .WithName("GetAllUnusual")
    .WithOpenApi();

app.Run();
