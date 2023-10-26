using MeterPro.DATA.DAL;
using MeterPro.DATA.DataContexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MeterProDataContext>();

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MeterCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
               //.AllowCredentials(); // If you need to support credentials (e.g., cookies)
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
