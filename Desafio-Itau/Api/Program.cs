using DesafioInvestimentosItau.Application.Investment.Investment.Client;
using DesafioInvestimentosItau.Application.Investment.Investment.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Position.Position.Client;
using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Client;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Trade.Trade.Client;
using DesafioInvestimentosItau.Application.Trade.Trade.Client.Strategy;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Infrastructure.Data;
using DesafioInvestimentosItau.Infrastructure.Helpers.Converters;
using DesafioInvestimentosItau.Infrastructure.Http;
using DesafioInvestimentosItau.Infrastructure.Messaging;
using DesafioInvestimentosItau.Infrastructure.Messaging.Quotation;
using DesafioInvestimentosItau.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Services
builder.Services.AddScoped<IQuoteService, QuoteService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IUserService, UserService>();


//Factory
builder.Services.AddScoped<ITradeFactory, TradeFactory>();

//Strategy
builder.Services.AddScoped<BuyTradeStrategy>();
builder.Services.AddScoped<SellTradeStrategy>();


//Helpers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringDecimalConverter());
    });
// Internal
builder.Services.AddHttpClient<IQuoteInternalService, QuoteInternalService>(client =>
    {
        client.BaseAddress = new Uri("https://b3api.vercel.app/");
    })
    .AddPolicyHandler(HttpPolicies.GetCircuitBreakerPolicy())
    .AddPolicyHandler(HttpPolicies.GetFallbackPolicy());

// Repositories
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInvestmentService, InvestmentService>();

// Kafka
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
builder.Services.AddHostedService<KafkaQuotationWorker>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();


var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Desafio Investimentos API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();