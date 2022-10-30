using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HNG_api_project.Data;
using HNG_api_project.Controllers;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HNG_api_projectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HNG_api_projectContext") ?? throw new InvalidOperationException("Connection string 'HNG_api_projectContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapBioEndpoints();

app.Run();
