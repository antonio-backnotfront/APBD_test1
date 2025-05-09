using Application;
using Microsoft.AspNetCore.Mvc;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("UniversityDatabase");
builder.Services.AddSingleton<ITavernService, TavernService>(tavernService => new TavernService(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/adventurers", async ([FromServices] ITavernService tavernService) =>
    {
        try
        {
            var adventurers = await tavernService.GetAdventurers();
            return Results.Ok(adventurers);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }).WithName("GetAdventurers")
    .WithOpenApi();

app.MapGet("/api/adventurers", async ([FromServices] ITavernService tavernService, int id) =>
    {
        try
        {
            var adventurer = await tavernService.GetAdventurer(id);
            return Results.Ok(adventurer);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }).WithName("GetAdventurerr")
    .WithOpenApi();

app.MapPost("/api/adventurers", async ([FromServices] ITavernService tavernService, Adventurer adventurer) =>
    {
        try
        {
            Adventurer newAdventurer = await tavernService.CreateAdventurer(adventurer);
            return Results.Ok(newAdventurer);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }).WithName("AddAdventurer")
    .WithOpenApi();


app.Run();
