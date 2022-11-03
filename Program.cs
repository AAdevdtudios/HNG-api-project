using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HNG_api_project.Data;
using HNG_api_project.Controllers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HNG_api_projectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HNG_api_projectContext") ?? throw new InvalidOperationException("Connection string 'HNG_api_projectContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    /*options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;*/
});
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
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
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapBioEndpoints();
app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerFeature>()
        ?.Error;
    if (exception is not null)
    {
        var response = new { error = exception.Message };
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(response);
    }
}));

app.Run();
