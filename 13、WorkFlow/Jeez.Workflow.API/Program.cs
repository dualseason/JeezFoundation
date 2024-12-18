using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Middlewares;
using Jeez.Workflow.API.Services.implements;
using Jeez.Workflow.API.Services.interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(WorkflowFixtrue));
builder.Services.AddScoped<ISystemsService, SystemsService>();
builder.Services.AddAutoMapper(typeof(MappingProfile).GetTypeInfo().Assembly);

builder.Services.AddAuthentication().AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<HttpGlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
