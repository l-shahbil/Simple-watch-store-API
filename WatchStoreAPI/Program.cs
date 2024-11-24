
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
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
//2-For Repository pattren
builder.Services.AddTransient(typeof(IRepository<>), typeof(ImplementRepo<>));
builder.Services.AddTransient<ICartRepository,CartRepoImplement>();
//3-For Cors Services
builder.Services.AddCors(builderOption =>
{
    builderOption.AddPolicy("MyPolicy", crosPolicyBuilder =>
    {
        crosPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
//4-For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequiredLength = 5;
    option.Password.RequireDigit = false;
    option.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<appDbContext>().AddDefaultTokenProviders();
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

builder.Services.AddEndpointsApiExplorer();

//swager authentication
// ÅÖÇÝÉ ÎÏãÇÊ Swagger
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "dotnetClaimAuthorization", Version = "v1" });
    c.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your valid token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});

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
//7-for images
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")),
    RequestPath = "/images"
});

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
