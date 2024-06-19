using Microsoft.EntityFrameworkCore;
using Todos.MinimalAPI.Data;
using Todos.MinimalAPI.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/v1/todos", (TodoContext context) =>
{
    var todos = context.Todos;
    return todos is not null ? Results.Ok(todos) : Results.NotFound();
});

app.MapPost("/v1/todos", async (TodoContext context, CreateTodoViewModel model) =>
{
    var todo = model.MapTo();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    context.Add(todo);
    await context.SaveChangesAsync();

    return Results.Created($"/v1/todos/{todo.Id}", todo);
});

app.Run();
