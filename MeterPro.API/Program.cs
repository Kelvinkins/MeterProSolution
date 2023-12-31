using MeterPro.API.Controllers;
using MeterPro.DATA.DAL;
using MeterPro.DATA.DataContexts;
using MeterPro.MQTT.Logics;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using uPLibrary.Networking.M2Mqtt;

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

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors(options => options
            .AllowAnyOrigin()
            .AllowAnyMethod()
           .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
BrokerService.StartMQTT();
BrokerService.StartListening(BrokerService.client);
app.Run();
