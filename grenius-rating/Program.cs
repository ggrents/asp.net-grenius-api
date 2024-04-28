using grenius_rating.Application.Repository;
using grenius_rating.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDataRepository, DataRepository>();

builder.Services.AddRabbitMassTransit(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();

app.Run();

