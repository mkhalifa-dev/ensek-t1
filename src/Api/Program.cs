using Api;
using Lib.Domain;
using Lib.Interfaces;
using Lib.Models;
using Lib.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConfig = new DbConfig { DataPath = "data/db.sqlite" };

builder.Services.AddSingleton(dbConfig);
builder.Services.AddScoped<ICsvParser<MeterReading>, MeterReadingsParser>();
builder.Services.AddScoped<IValidator<ParsedItem<MeterReading>>, MeterReadingValidator>();
builder.Services.AddScoped<IAccountsRepository, SqliteAccountsRepository>();
builder.Services.AddScoped<IMeterReadingsRepository, SqliteMeterReadingsRepository>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
     builder =>
     {
         builder
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
     }));

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

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

await Seed.AddAcounts(dbConfig);
app.Run();
