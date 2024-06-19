using Microsoft.EntityFrameworkCore;
using Todos.MinimalAPI.Data;
using Todos.MinimalAPI.Models;
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

app.MapGet("/v1/todos/{id}", async (TodoContext context, Guid id) =>
{
    var todo = await context.Todos.FindAsync(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/v1/todos", async (TodoContext context, CreateTodoViewModel model) =>
{
    var todo = model.MapTo();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    context.Add(todo);
    await context.SaveChangesAsync();

    return Results.Created($"/v1/todos/{todo.Id}", todo);
}).Produces<Todo>();

app.MapPut("/v1/todos/{id}", async (TodoContext context, Guid id, Todo todoItem) =>
{
    var todo = await context.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    context.Todos.Update(todoItem);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("v1/todos/{id}", async (TodoContext context, Guid id) =>
{
    if (await context.Todos.FindAsync(id) is Todo todo)
    {
        context.Todos.Remove(todo);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
