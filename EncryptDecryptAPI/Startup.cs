using BAL.Interface;
using BAL.Service;
using Common.Service;
using Common.Extension;
using DAL;
using Encrpt_Decrpt_API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace Encrpt_Decrpt_API
{
    public class Startup
    {

        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddHttpClient();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDecryptService, DecryptService>();
            services.AddScoped<IEncryptService, EncryptService>();
            services.AddScoped<IKeyRepository, KeyRepository>();
            services.AddScoped<SessionService>();
            services.AddEndpointsApiExplorer();
            services.AddMvcCore();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Eicore - Encryption Decryption APIs",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.OperationFilter<AuthResponsesOperationFilter>();

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(option =>
           {
               option.RequireHttpsMetadata = false;
               option.SaveToken = true;
               option.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = _configuration["Jwt:Issuer"],
                   ValidAudience = _configuration["Jwt:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey configuration is missing.")))
               };

               option.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = async context =>
                   {
                       context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                       context.Response.ContentType = "application/json";

                       var error = new
                       {
                           Message = "Token validation failed.",
                           Reason = context.Exception.Message,
                           Timestamp = DateTime.UtcNow,
                           TraceId = context.HttpContext.TraceIdentifier
                       };

                       var json = JsonSerializer.Serialize(error);
                       await context.Response.WriteAsync(json);
                   }
               };

           });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors?.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    string message = "Validation Error - (One or more validation errors occurred)\"";
                    return new BadRequestObjectResult(new { message, errors });
                };
            });

            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Customize as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddCustomFileLogger(_configuration);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ENC_Dec Web APIs v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
