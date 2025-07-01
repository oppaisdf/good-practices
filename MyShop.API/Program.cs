using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Users.Queries;
using MyShop.Application;
using MyShop.API.Middleware;

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

app.Run();
