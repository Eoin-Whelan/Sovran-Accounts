using Accounts.Business;
using Accounts.Business.Login;
using Accounts.Business.Registration;
using Accounts.Data.Contracts;
using Accounts.Data.Dapper;
using Accounts.ServiceClients.Catalog.ApiProxy;
using Accounts.ServiceClients.Cloudinary;
using AccountService.ServiceClients.Payment.ApiProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// DBCONTEXT CONFIG
builder.Services.AddDbContext<AccountsContext>(
   options => options.UseMySQL(
                     builder.Configuration.GetConnectionString("sql")));


// DEPENDENCY INJECTIONconfig

//Web.config test


builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILoginRequestValidator, LoginRequestValidator>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
//builder.Services.AddScoped<Accounts.Business.Repository.IRepository<MerchantAccount, int>, OldAccountRepository>();


builder.Services.AddScoped<ICatalogProxy>(x => new CatalogProxy("https://sovran-catalog.azurewebsites.net", new HttpClient()));
builder.Services.AddScoped<IPaymentProxy>(x => new PaymentProxy("https://sovran-payment.azurewebsites.net", new HttpClient()));
builder.Services.AddScoped<IRegistrationValidator, RegistrationValidator>();

builder.Services.AddScoped<IImageHandler, ImageHandler>(x => new ImageHandler());


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
    //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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
