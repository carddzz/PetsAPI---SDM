using Microsoft.OpenApi.Models;
using PetsApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pets API", Version = "v1" });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pets API v1"));
}

var animais = new List<Animal>();


app.MapGet("/animais", () => animais);

app.MapGet("/animais/{id}", (int id) => 
{
    var animal = animais.FirstOrDefault(a => a.Id == id);
    return animal is not null ? Results.Ok(animal) : Results.NotFound();
});

app.MapPost("/animais", (Animal animal) =>
{
    animal.Id = animais.Count > 0 ? animais.Max(a => a.Id) + 1 : 1;
    animais.Add(animal);
    return Results.Created($"/animais/{animal.Id}", animal);
});

app.MapPut("/animais/{id}", (int id, Animal updatedAnimal) =>
{
    var index = animais.FindIndex(a => a.Id == id);
    if (index == -1) return Results.NotFound();

    updatedAnimal.Id = id;
    animais[index] = updatedAnimal;
    return Results.Ok(updatedAnimal);
});

app.MapDelete("/animais/{id}", (int id) =>
{
    var index = animais.FindIndex(a => a.Id == id);
    if (index == -1) return Results.NotFound();

    animais.RemoveAt(index);
    return Results.NoContent();
});

app.Run();