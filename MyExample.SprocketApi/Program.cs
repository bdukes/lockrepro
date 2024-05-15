//using Microsoft.EntityFrameworkCore;
//using MyExample.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSqlServer<TheDataContext>(builder.Configuration.GetConnectionString(nameof(TheDataContext)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "hello");
/*
app.MapGet("/", (TheDataContext context) => context.Sprockets.AsAsyncEnumerable())
    .WithName("GetAll")
    .WithOpenApi();

app.MapGet(
        "/{id:guid}",
        (TheDataContext context, Guid id, CancellationToken cancellationToken)
            => context.Sprockets.SingleOrDefaultAsync(x => x.Id == id, cancellationToken))
    .WithName("GetOne")
    .WithOpenApi();

app.MapGet("/one", (TheDataContext context) => context.Sprockets.Where(s => s.Type == SprocketType.One).AsAsyncEnumerable())
    .WithName("GetAllOne")
    .WithOpenApi();

app.MapGet("/two", (TheDataContext context) => context.Sprockets.Where(s => s.Type == SprocketType.Two).AsAsyncEnumerable())
    .WithName("GetAllTwo")
    .WithOpenApi();
*/
app.Run();
