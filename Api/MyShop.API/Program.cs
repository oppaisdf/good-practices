using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Users.Queries;
using MyShop.Application;
using MyShop.API.Middleware;
using MyShop.Application.Users.Commands;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.MapGet("/users", async ([FromServices] IMediator m) =>
        Results.Ok(await m.Send(new GetUsersQuery()))
    )
    .WithName("GetUsers")
    .WithOpenApi();

app.MapPost("/users",
    async (
        [FromServices] IMediator m,
        CreateUserCommand body
    ) =>
    {
        var id = await m.Send(body);
        return Results.Created($"/users/{id}", new { id });
    })
    .WithName("CreateUser")
    .WithOpenApi();

app.Run();
