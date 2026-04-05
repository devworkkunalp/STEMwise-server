using Microsoft.EntityFrameworkCore;
using STEMwise.Infrastructure.Data;
using STEMwise.Application.Interfaces;
using STEMwise.Infrastructure.ExternalAPIs;

var builder = WebApplication.CreateBuilder(args);

// Add Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add External API Clients
builder.Services.AddHttpClient<ICollegeScorecardClient, CollegeScorecardClient>();
builder.Services.AddHttpClient<IFrankfurterClient, FrankfurterClient>();
builder.Services.AddHttpClient<IBlsOewsClient, BlsOewsClient>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
