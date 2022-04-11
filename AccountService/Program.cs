using Accounts.Business;
using Accounts.Business.Login;
using Accounts.Business.Registration;
using Accounts.Business.Repository;
using Accounts.Data.Contracts;
using Accounts.Data.Dapper;
using Accounts.Model;
using Accounts.ServiceClients.Catalog.ApiProxy;
using Accounts.ServiceClients.Cloudinary;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// DBCONTEXT CONFIG
builder.Services.AddDbContext<AccountsContext>(
   options => options.UseMySQL(
                     @"Server=sovran-accounts.cihpzkqwv66o.eu-west-1.rds.amazonaws.com;
                        Database=sovran_accounts;
                        User=notary;
                        Password=B4rth0l0m3w!;
                        Convert Zero Datetime=True; Allow User Variables=True;"));


// DEPENDENCY INJECTION

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILoginRequestValidator, LoginRequestValidator>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
//builder.Services.AddScoped<Accounts.Business.Repository.IRepository<MerchantAccount, int>, OldAccountRepository>();


builder.Services.AddScoped<IApiProxy>(x => new ApiProxy("https://sovran-catalog.azurewebsites.net", new HttpClient()));
builder.Services.AddScoped<IRegistrationValidator, RegistrationValidator>();

builder.Services.AddScoped<IImageHandler, ImageHandler>(x => new ImageHandler());


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Catalog Service API",
        Description =
            "<h2>CatalogController is an API for all Catalog service business logic.</h2><br>" +
            "This functionality extends to:<ul>" +
            "<li> Inserting a new catalog(Registration flow)" +
            "<li> Inserting a new item for a given merchant." +
            "<li> Update an existing item's details." +
            "<li> Updating a merchant's public details (e.g.Support information)" +
            "<li> Retrieving a given merchant's entire catalog (Browsing purposes).</ul>"
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

//  Enable CORS for local testing.
builder.Services.AddCors(o => o.AddPolicy("Dev", builder =>
{
    builder.WithOrigins("http://localhost.com")
           .AllowAnyMethod()
           .AllowAnyHeader();

}));


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
