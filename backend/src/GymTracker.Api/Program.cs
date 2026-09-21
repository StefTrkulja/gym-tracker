using GymTracker.Api;
using GymTracker.Api.Middleware;
using GymTracker.Application;
using GymTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();
builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// TODO: restrict AllowAnyOrigin to specific frontend URL in production
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();