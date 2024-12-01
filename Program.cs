using System.Security.Cryptography.Xml;
using System.Text;
using LearningWebAPI;
using LearningWebAPI.Application.Mapping;
using LearningWebAPI.Application.Swagger;
using LearningWebAPI.Domain.Model.EmployeeAggregate;
using LearningWebAPI.Infraestrutura.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(DomainToDTOMapping));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Versionamento de API
builder.Services.AddApiVersioning( o => 
{
    // define que a versão padrão é a 1.0
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen( c =>
{
    c.OperationFilter<SwaggerDefaultValues>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
             new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
        },
        new List<string>()
        }
    });
});

// A injeção de dependencia é feita no arquivo Program.cs
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

// adicionando CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
        .AllowAnyHeader().AllowAnyMethod();
    });
});

var key = Encoding.ASCII.GetBytes(Key.Secret);

// Autenticação
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();
var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"LearningWebAPI - {description.GroupName.ToUpper()}");
        }
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
