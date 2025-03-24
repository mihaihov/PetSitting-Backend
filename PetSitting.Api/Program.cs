using PetSitting.Infrastructure;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using PetSitting.Infrastructure.Services;
using PetSitting.Application;
using PetSitting.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using PetSitting.Domain.Entities.Utils;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Description = "Standard authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddCors(options  => {
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

//db boilerplate
builder.Services.RegisterDbContext(builder.Configuration);
builder.Services.RegisterRepositories();
//register frirebase service
builder.Services.RegisterFirebaseServices();
//register application srervices
builder.Services.AddApplicationServices();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.Audience = builder.Configuration["JwtSettings:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings.Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings.SecretKey"]!)),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });



//initialize firebase
var firebaseApp = FirebaseApp.Create(new AppOptions {
    Credential = GoogleCredential.FromFile("/Users/danielaarvinti/Desktop/Mihai/Documents/petsitting-development-firebase-adminsdk-fbsvc-c802b066a1.json")
});


var app = builder.Build();

//seed roles
PetSitting.Infrastructure.Utils.Seeder.SeedRoles(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("Open");
app.MapControllers();

app.Run();

