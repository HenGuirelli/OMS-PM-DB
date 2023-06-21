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
        PmGlobalConfiguration.PmInternalsFolder = settings.Pm!.InternalsFolder;
        string os = Environment.OSVersion.Platform.ToString();

        if (os.StartsWith("Win"))
        {
            Console.WriteLine("Ambiente Windows, usando arquivos mapeados em memória tradicionais");
            PmGlobalConfiguration.PmTarget = PM.Core.PmTargets.TraditionalMemoryMappedFile;
        }
        else
        {
            Console.WriteLine("Ambiente Linux, usando PM");
            PmGlobalConfiguration.PmTarget = PM.Core.PmTargets.PM;
        }

        Console.WriteLine("Persistencia: " + settings.Persistency);
        if (settings.Persistency.ToLower() == "sqlite")
            return new SqlLiteOrderRepository(settings.SqlLite!.ConnectionString);

        if (settings.Persistency.ToLower() == "sqliteoptimized")
            return new SqlLiteOptimizedOrderRepository(settings.SqlLite!.ConnectionString);

        if (settings.Persistency.ToLower() == "sqlitetransaction")
            return new SqlLiteTransactionOrderRepository(settings.SqlLite!.ConnectionString);

        if (settings.Persistency.ToLower() == "postgresql")
            return new PostgreSqlOrderRepository(settings.PostgreSQL!.ConnectionString);

        if (settings.Persistency.ToLower() == "postgresqloptimized")
            return new PostgreSqlOptimizedOrderRepository(settings.PostgreSQL!.ConnectionString);

        if (settings.Persistency.ToLower() == "memory")
            return new MemoryOrderRepository();

        if (settings.Persistency.ToLower() == "pm")
        {
            return new PmOrderRepository(settings.Pm!.OrdersFilePath);
        }

        if (settings.Persistency.ToLower() == "pmtransaction")
        {
            return new PmTransactionOrderRepository(settings.Pm!.OrdersFilePath);
        }

        throw new ApplicationException("Invalid config " + settings.Persistency.ToLower());
    });


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
