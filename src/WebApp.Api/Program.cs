using UserManagement;
using WebApp.Api.DependencyInjection;
using WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterJwtAuthentication(builder.Configuration);
builder.Services.AddWebAppData(builder.Configuration);

builder.Services.RegisterTenantRepositories();
builder.Services.RegisterTenantServices();
builder.Services.RegisterTenantControllers();

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.RegisterControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
