using Callapp.UserManagement.Application.Interfaces;
using Callapp.UserManagement.Application.Services;
using Callapp.UserManagement.Data;
using Callapp.UserManagement.Web.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "Bearer",
            Name = "Bearer",
            In = ParameterLocation.Header,
        },
        new List<string>()
        }
    });
});
builder.Services.AddDbContext<CallappContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagement")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddHttpClient("Resources", o =>
{
    o.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});
builder.Services
    .AddAuthentication()
    .AddBearerToken();
builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    // Set a default policy that requires all users to be authenticated
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

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

app.MapAuthRoutes();

app.MapUserRoutes();

app.MapResourceRoutes();

app.Run();
