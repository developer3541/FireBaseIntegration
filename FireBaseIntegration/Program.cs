using FirebaseAdmin;
using FireBaseIntegration.Service;

var builder = WebApplication.CreateBuilder(args);
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\dell\Downloads\project.json");
builder.Services.AddSingleton(FirebaseApp.Create());
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<FireBaseService>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
