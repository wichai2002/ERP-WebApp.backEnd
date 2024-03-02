using Yinnaxs_BackEnd.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Yinnaxs_BackEnd.Utility;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// Jwt configuration start
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = jwtIssuer,
        //ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
}
   );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connect MYSQL Server
var connectionStringMySql = builder.Configuration.GetConnectionString("DefaultMySQLConnention");

// connect DepartmentContext with MySQL Server
builder.Services.AddDbContext<ApplicationDbContext>
    (option => option.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();