using RealEstate_Api.Data;
using RealEstate_Api.Models;
using System.Reflection;
using FluentValidation.AspNetCore;
using RealEstate_Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddFluentValidation(r => r.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PropertyRepository>();
builder.Services.AddScoped<PropertyTypeRepository>();
builder.Services.AddScoped<AgentRepository>();
builder.Services.AddScoped<AppointmentRepository>();
builder.Services.AddScoped<ContactUsRepository>();
builder.Services.AddScoped<CheckAccess>(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
