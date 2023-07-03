using Library.Api.ApplicationServices;
using Library.Domain;
using Library.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// my registration
builder.Services.AddSingleton<ITitleReverser, TitleReverser>();
builder.Services.AddScoped<IBookApplicationService, BookApplicationService>();

builder.Services.AddDbContext<LibraryDbContext>((serviceProvider, options) =>
{
    var config = serviceProvider.GetService<IConfiguration>();
    var connectionString = config!.GetConnectionString(nameof(LibraryDbContext));
    options.UseSqlServer(connectionString);
});

using var serviceScope = builder.Services.BuildServiceProvider().CreateScope();
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

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
