using Microsoft.Extensions.DependencyInjection;
using OMS;
using OMS.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.Get<Settings>();

builder.Services.AddHostedService<StartupService>();
//builder.Services.AddSingleton<IOrderRepository, SqlLiteOrderRepository>(
//    serviceProvider =>
//    {
//        return new SqlLiteOrderRepository(settings.ConnectionString);
//    });
builder.Services.AddSingleton<IOrderRepository, MemoryOrderRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
