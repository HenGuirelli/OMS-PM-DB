using Microsoft.Extensions.DependencyInjection;
using OMS;
using OMS.Repositories;
using PM.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.Get<Settings>();

builder.Services.AddHostedService<StartupService>();
builder.Services.AddSingleton<IOrderRepository>(
    serviceProvider =>
    {
        if (settings.Persistency.ToLower() == "sqlite")
            return new SqlLiteOrderRepository(settings.SqlLite!.ConnectionString);

        if (settings.Persistency.ToLower() == "memory")
            return new MemoryOrderRepository();

        if (settings.Persistency.ToLower() == "pm")
        {
            PmGlobalConfiguration.PmInternalsFolder = settings.Pm!.InternalsFolder;
            PmGlobalConfiguration.PmTarget = PM.Core.PmTargets.PM;

            return new PmOrderRepository(settings.Pm!.OrdersFilePath);
        }

        throw new ApplicationException("Invalid config " + settings.Persistency.ToLower());
    });
//builder.Services.AddSingleton<IOrderRepository, MemoryOrderRepository>();


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
