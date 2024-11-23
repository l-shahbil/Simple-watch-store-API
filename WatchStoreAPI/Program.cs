
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Watchs_Store.Seeder;
using WatchStoreAPI.Data;
using WatchStoreAPI.Models;
using WatchStoreAPI.Models.DTO;
using WatchStoreAPI.Repository;
using WatchStoreAPI.Repository.Base;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
//1-Dbcontext
builder.Services.AddDbContext<appDbContext>(option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
    });
//2-For Repository
builder.Services.AddTransient(typeof(IRepository<>), typeof(ImplementRepo<>));
//3-For Cors Services
builder.Services.AddCors(builderOption =>
{
    builderOption.AddPolicy("MyPolicy", crosPolicyBuilder =>
    {
        crosPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
//4-For Identity
builder.Services.AddIdentity<AspNetUser, IdentityRole>(option =>
{
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequiredLength = 5;
    option.Password.RequireDigit = false;
    option.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<appDbContext>();
//5-[Authorized] used JWT token instead cookies
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(option =>
   {
       option.SaveToken = true;
       option.RequireHttpsMetadata = true;
       option.TokenValidationParameters = new TokenValidationParameters()
       {
           ValidateIssuer = true,
           ValidIssuer = builder.Configuration["JWT:validIssure"],
           ValidateAudience = true,
           ValidAudience = builder.Configuration["JWT:validAudience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true
       };
   });
//6-For Seed Admin
builder.Services.AddTransient<AdminSeeder>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//6-For Seed Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var seeder = services.GetRequiredService<AdminSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
