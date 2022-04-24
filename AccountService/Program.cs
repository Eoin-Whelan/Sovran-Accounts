using Accounts.Business.Registration;
using Accounts.Data.Contracts;
using Accounts.Data.Dapper;
using Accounts.Data.Repository;
using Accounts.ServiceClients.Catalog.ApiProxy;
using Accounts.ServiceClients.Cloudinary;
using AccountService.ServiceClients.Payment.ApiProxy;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sovran.Logger;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// DBCONTEXT CONFIG
builder.Services.AddDbContext<AccountsContext>(
   options => options.UseMySQL(
                     builder.Configuration.GetConnectionString("sql")));


// DEPENDENCY INJECTION
builder.Services.AddScoped<ISovranLogger, SovranLogger>(x => new SovranLogger (
    "Accounts",
    builder.Configuration.GetConnectionString("loggerMongo"),
    builder.Configuration.GetConnectionString("loggerSql")
    )
);
builder.Services.AddScoped<Account>(x => new Account
{
    ApiKey = builder.Configuration.GetSection("cloudinaryApiKey").Value,
    ApiSecret = builder.Configuration.GetSection("cloudinaryApiSecret").Value,
    Cloud = builder.Configuration.GetSection("cloudinaryDomain").Value
});
builder.Services.AddScoped<ICatalogProxy>(x => new CatalogProxy(
    builder.Configuration.GetConnectionString("catalog"),
    new HttpClient()));
builder.Services.AddScoped<IPaymentProxy>(x => new PaymentProxy(
    builder.Configuration.GetConnectionString("payment"),
    new HttpClient()));
builder.Services.AddScoped<IRegistrationValidator, RegistrationValidator>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IImageHandler, ImageHandler>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Account Service API",
        Description =
            "<h2>AccountController is an API for all Account service business logic.</h2><br>" +
            "This functionality extends to:<ul>" +
            "<li> Handles registration flow for users, communicating with Payment + Catalog services." +
            "<li> Login validation" +
            "<li> Update a merchant's personal information." +
            "<li> Retrieving a merchant's personal information." +
            "</ul>"
        ,
        Contact = new OpenApiContact
        {
            Name = "Eoin Whelan (Farrell)",
            Email = "C00164354@itcarlow.ie",
        }

    });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseCors();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
