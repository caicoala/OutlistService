using OutlistService.Application.UseCases;
using OutlistService.Domain.Interfaces;
using OutlistService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOutlistRepository, MongoOutlistRepository>();
builder.Services.AddScoped<OutlistUseCaseService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
