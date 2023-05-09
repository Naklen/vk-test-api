using Microsoft.EntityFrameworkCore;
using vk_test_api;
using ZNetCS.AspNetCore.Authentication.Basic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiContext>(options => options.UseNpgsql(connection));

builder.Services.AddMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromSeconds(5));

builder.Services.AddScoped<AuthenticationEvents>();
builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme).
    AddBasicAuthentication(options =>
    {
        options.Realm = "User API";
        options.EventsType = typeof(AuthenticationEvents);
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
