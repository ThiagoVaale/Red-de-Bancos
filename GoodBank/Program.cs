using Application.Abstractions;
using Application.Services;
using Domine.Enums;
using GoodBank.Application.Interfaces;
using GoodBank.Infrastructure.Strategies;
using GoodBank.Middleware;
using Infrastructure.Gateways;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Strategies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ExceptionMiddleware>();


builder.Services.AddScoped<IExternalTransferStrategy, BadBankStrategy>();
builder.Services.AddScoped<IExternalTransferStrategy, WorseBankStrategy>();
builder.Services.AddScoped<IExternalTransferStrategy, WorstBankStrategy>();

builder.Services.AddScoped<StrategyResolver>(sp =>
{
    return (BankCode bankCode) =>
    {
        return bankCode switch
        {
            BankCode.BadBank => sp.GetRequiredService<BadBankStrategy>(),
            BankCode.WorseBank => sp.GetRequiredService<WorseBankStrategy>(),
            BankCode.WorstBank => sp.GetRequiredService<WorstBankStrategy>(),

            BankCode.GoodBank => throw new ArgumentOutOfRangeException(
                nameof(bankCode),
                "GoodBank no usa estrategia externa."
            ),

            _ => throw new ArgumentOutOfRangeException(
                nameof(bankCode),
                "BankCode no soportado para transferencia externa."
            )
        };
    };
});

builder.Services.AddHttpClient("BadBank", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalBanks:BadBank:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddHttpClient("WorseBank", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalBanks:WorseBank:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddHttpClient("WorstBank", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalBanks:WorstBank:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(3);
});

// El StrategyResolver ya lo habrá registrado el Desarrollador 1

builder.Services.AddScoped<IExternalTransferGateway, ExternalTransferGateway>();
builder.Services.AddDbContext<GoodBankDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<InternalTransferService>();
builder.Services.AddScoped<ExternalTransferService>();
builder.Services.AddScoped<IncomingTransferService>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GoodBankDbContext>();
    Console.WriteLine("[DB PATH RUNTIME] => " + db.Database.GetDbConnection().DataSource);
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"[REQ] {context.Request.Method} {context.Request.Path}{context.Request.QueryString} Content-Type:{context.Request.ContentType}");
    await next();
    Console.WriteLine($"[RESP] {context.Response.StatusCode} for {context.Request.Method} {context.Request.Path}");
});

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
