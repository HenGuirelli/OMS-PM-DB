using OMS;
using OMS.OrderSenders;
using OMS.Repositories;
using PM.Configs;
using Serilog;
using System.Configuration;

void ClearPath(string directoryPath)
{
    try
    {
        DirectoryInfo directoryInfo = new(directoryPath);
        FileInfo[] files = directoryInfo.GetFiles();

        foreach (FileInfo file in files)
        {
            file.Delete();
        }

        Log.Information("Arquivos excluídos com sucesso!");
    }
    catch (Exception ex)
    {
        Log.Error("Ocorreu um erro ao excluir os arquivos: {message}", ex.Message);
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.Get<Settings>();

//PmGlobalConfiguration.Logger.SetTarget(PmLogTarget.File, @"C:\rox\pm_tests");

var loggingTarget = GetLoggingTarget(builder);
var loggingDirectory = GetLoggingDirectory(builder);
var loggingLevel = GetLoggingLevel(builder);

PmGlobalConfiguration.Logger.SetTarget(
    loggingTarget,
    loggingLevel,
    loggingDirectory);



builder.Services.AddHostedService<StartupService>(services =>
{
    var orderProcess = new OrderProcess(services.GetRequiredService<IOrderRepository>());
    if (settings.ConnectType.ToLower() == "quickfix")
    {
        return new StartupService(
            new InitiatorOrderSender(orderProcess));
    }
    else
    {
        return new StartupService(
            new InnerOrderSender(orderProcess,
                new DropcopyGenerator.OrderGenerator(
                    settings!.SelfContainedSettings!.OrderQuantity,
                    settings!.SelfContainedSettings!.OrderExecutedQuantity)));
    }
});
builder.Services.AddSingleton<IOrderRepository>(
    serviceProvider =>
    {
        if (settings.UseTraditionalMemoryMappedFiles)
        {
            Log.Information("usando arquivos mapeados em memória tradicionais");
            PmGlobalConfiguration.PmTarget = PM.Core.PmTargets.TraditionalMemoryMappedFile;
        }
        else
        {
            Log.Information("usando PM");
            PmGlobalConfiguration.PmTarget = PM.Core.PmTargets.PM;
        }
        PmGlobalConfiguration.PmInternalsFolder = settings.Pm.InternalsFolder;
        ClearPath(PmGlobalConfiguration.PmInternalsFolder);

        Log.Information("Persistencia: {persistency}", settings.Persistency);
        if (settings.Persistency.ToLower() == "sqlite")
            return new SqlLiteOrderRepository(settings.SqlLite!.ConnectionString);

        if (settings.Persistency.ToLower() == "sqliteoptimized")
            return new SqlLiteOptimizedOrderRepository(settings.SqlLite!.ConnectionString);

        if (settings.Persistency.ToLower() == "sqlitetransaction")
            return new SqlLiteTransactionOrderRepository(settings.SqlLite!.ConnectionString);

        if (settings.Persistency.ToLower() == "postgresql")
            return new PostgreSqlOrderRepository(settings.PostgreSQL!.ConnectionString);

        if (settings.Persistency.ToLower() == "postgresqltransaction")
            return new PostgreSqlOrderRepositoryTransaction(settings.PostgreSQL!.ConnectionString);

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

static PmLogTarget GetLoggingTarget(WebApplicationBuilder builder)
{
    var target = builder.Configuration["Serilog:Target"].ToLower();
    if (target == "file")
    {
        return PmLogTarget.File;
    }
    else if (target == "console")
    {
        return PmLogTarget.Console;
    }
    else
    {
        return PmLogTarget.None;
    }
}

string? GetLoggingDirectory(WebApplicationBuilder builder)
{
    try
    {
        return builder.Configuration["Serilog:Directory"];
    }
    catch
    {
        return null;
    }
}

Serilog.Events.LogEventLevel GetLoggingLevel(WebApplicationBuilder builder)
{
    var loggingLevel = builder.Configuration["Serilog:LogLevel"];
    if (Enum.TryParse<Serilog.Events.LogEventLevel>(loggingLevel, out var result))
    {
        return result;
    }
    return Serilog.Events.LogEventLevel.Information;
}