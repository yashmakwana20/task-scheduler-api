using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceStack.OrmLite;
using System.Security.Claims;
using System.Text;
using TaskManagement.Data;
using TaskManagement.Repositories;
using TaskManagement.Services;
using TaskManagement.Services.Common;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
//var connStr = "server=127.0.0.1;port=3306;database=TaskManagementDB;user=root;password=Y@sh2010;SslMode=None;AllowPublicKeyRetrieval=True;";
// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(option =>
option.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    // This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagement API", Version = "v1" });

    // To Enable authorization using Swagger (JWT) 
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddScoped<BLTaskItemHandler>();
builder.Services.AddScoped<DBTaskItemContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtService>();
builder.Services.AddControllers().AddNewtonsoftJson();

//builder.Services.AddSingleton<OrmLiteConnectionFactory>(sp =>
//{
//    var config = sp.GetRequiredService<IConfiguration>();
//    var connStr = config.GetConnectionString("DefaultConnection");

//    return new OrmLiteConnectionFactory(connStr, MySqlDialect.Provider);
//});

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", option =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        RoleClaimType = ClaimTypes.Role,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
