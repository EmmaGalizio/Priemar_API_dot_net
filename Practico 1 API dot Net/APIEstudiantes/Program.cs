using APIEstudiantes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                    {
                        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
                        options.Audience = builder.Configuration["Auth0:Audience"];
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = ClaimTypes.NameIdentifier
                        };
                    });

builder.Services
          .AddAuthorization(options =>
              {
                  options.AddPolicy(
                    "read:estudiantes",
                    policy => policy.Requirements.Add(
                      //new HasScopeRequirement("read:estudiantes", "https://dev-gmge4jx6gng5h385.us.auth0.com/oauth/token")
                      new HasScopeRequirement("read:estudiantes", domain)
                    )
                  );
                  options.AddPolicy(
                    "write:estudiantes",
                    policy => policy.Requirements.Add(
                      //new HasScopeRequirement("write:estudiantes", "https://dev-gmge4jx6gng5h385.us.auth0.com/oauth/token")
                      new HasScopeRequirement("write:estudiantes", domain)
                    )
                  );

              });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
