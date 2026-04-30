using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STEMwise.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => {
        sqlOptions.EnableRetryOnFailure();
    }));

// Add ASP.NET Core Identity (Native Azure SQL Auth)
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add CORS
builder.Services.AddCors(options =>
{
    var allowedOrigins = (builder.Configuration["ALLOWED_ORIGIN"] ?? "http://localhost:5173")
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(o => o.Trim())
        .ToArray();
        
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "STEMwise Basic Auth API", Version = "v1" });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var appDb = services.GetRequiredService<AppDbContext>();
        appDb.Database.Migrate();
        Console.WriteLine("[DB] Identity Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[DB ERROR] Could not apply migrations: {ex.Message}");
    }
}

app.MapGet("/", () => Results.Ok("STEMwise Identity API is running."));
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api").MapIdentityApi<IdentityUser>();
app.MapControllers();

app.Run();
