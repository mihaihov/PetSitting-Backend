using PetSitting.Infrastructure;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using PetSitting.Infrastructure.Services;
using PetSitting.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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



//initialize firebase
var firebaseApp = FirebaseApp.Create(new AppOptions {
    Credential = GoogleCredential.FromFile("/Users/danielaarvinti/Desktop/Mihai/Documents/petsitting-development-firebase-adminsdk-fbsvc-c802b066a1.json")
});


var app = builder.Build();

//seed roles
//Seeder.SeedRoles(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Open");
app.MapControllers();

app.Run();

