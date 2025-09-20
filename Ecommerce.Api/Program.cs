using Ecommerce.Api.Validators;
using Ecommerce.Data.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ecommerce.Data.Interfaces.IAuthServices;
using Ecommerce.Data.Repositories.AuthService;

var builder = WebApplication.CreateBuilder(args);




//8. ------------------------ Configure Serilog ---------------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // daily log files
    .WriteTo.Console() // optional: also log to console
    .CreateLogger();
builder.Host.UseSerilog(); // Use Serilog as the logging provider

//1---------------------------Registering DbContext ------------------------
builder.Services.AddDbContext<EcommerceStoreDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//2---------------------------Registering AutoMapper ------------------------
builder.Services.AddAutoMapper(typeof(Program));

//3---------------------------Registering Repository ------------------------
builder.Services.AddScoped<Ecommerce.Data.Interfaces.IEcommerceStoreRepository, Ecommerce.Data.Repositories.EcommerceStoreRepository>();
builder.Services.AddScoped<IAuthServiceRepository, AuthServiceRepository>();

//4---------------------------Registering FluentValidator ------------------------
builder.Services.AddValidatorsFromAssemblyContaining<CreateEcommerceStoreDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

//5 Registers controller support (API controllers using [ApiController] and attribute routing).
builder.Services.AddControllers();

//6 Adds support for endpoint discovery, so tools like Swagger,Postman can explore API endpoints.
builder.Services.AddEndpointsApiExplorer();

//7 Registers Swagger/OpenAPI generator for interactive API documentation and testing.
builder.Services.AddSwaggerGen();

// 9 ------------------ Registering API Versioning ------------------
builder.Services.AddApiVersioning(options =>
{
    // Assume default version when client does not specify
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Set the default version
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Report supported versions in response headers
    options.ReportApiVersions = true;
});

// 10 ------------------ Registering Json Web Token ------------------
//a. Am registering authentication service with the "Bearer" scheme, this tells ASP.NET Core that we will be using JWT Bearer tokens for authentication.
builder.Services.AddAuthentication("Bearer")
    //b.Configure how JWT Bearer tokens should be validated when they come in.
    .AddJwtBearer(Options =>
    {   
          var secretKey = builder.Configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
            throw new Exception("JWT SecretKey is missing in configuration");


        //c. Define validation rules for incoming JWT tokens
        Options.TokenValidationParameters = new()
        {
            // d. Ensure the token's issuer (who issued the token) matches what we expect.
            ValidateIssuer = true,
            // e. Ensure the token's audience (who the token is intended for) matches what we expect.
            ValidateAudience = true,
            // f. Ensure the token has not expired (check the 'exp' claim).
            ValidateLifetime = true,
            // g. Ensure the token signature is valid (it was signed with our secret key).
            ValidateIssuerSigningKey = true,
            // h. Only accept the issuer I specify in JwtSettings  
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            // i. “Only accept tokens that were issued for this API (the audience we set in config).”
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            // j. The key used to verify the token's signature (must match what was used to create the token).
            
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey))
        };
    });

/* 11 ------------------ Registering CORS ------------------*/
//a. Register CORS service 
builder.Services.AddCors(options =>
{
    //b. Define a CORS policy named "AllowFrontend"
    options.AddPolicy("AllowFrontend", policy => policy
    .WithOrigins("http://localhost:5239")     //c. Only allow requests from this origin ("http://localhost:4200")
    .AllowAnyHeader()                         //d. Allow any HTTP headers in the request
    .AllowAnyMethod()                         //e. Allow any HTTP method (GET, POST, PUT, DELETE, OPTIONS, etc.)
    );
});

// Building the app
var app = builder.Build();



/*------------------------------------------------------------------------------*/

// A. Enable Swagger & SwaggerUI only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//G. Enabling CORS with the "AllowFrontend" policy
app.UseCors("AllowFrontend");

// B. Enforce HTTPS
app.UseHttpsRedirection();

// C. Registering ErrorHandlingMiddleware(Custom Middleware)
app.UseMiddleware<Ecommerce.Api.MiddleWares.ErrorHandlingMiddleware>();

// D. Authentication
app.UseAuthentication();

// E. Authorization
app.UseAuthorization();

// F. Controllers
app.MapControllers();

app.Run();
