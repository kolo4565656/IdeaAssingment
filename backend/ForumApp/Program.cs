using FluentValidation.AspNetCore;
using ForumApi.Middlewares.Validations;
using Microsoft.OpenApi.Models;
using ForumApplication;
using ForumPersistence;
using ForumApi.Middlewares.Filters;
using ForumApi.Middlewares;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddApplicationLayer(builder.Configuration);

builder.Services.AddControllers()
.AddFluentValidation(fv => { 
    fv.RegisterValidatorsFromAssemblyContaining<ValidationConstants>();
    fv.DisableDataAnnotationsValidation = true;
    fv.ImplicitlyValidateChildProperties = true;
})
.AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IdeaApi",
        Version = "v1",
        Description = "Idea Management System"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => {
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalException>();

app.MapControllers();

app.Run();
