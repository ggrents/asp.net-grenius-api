//
//            God Bless         No Bugs
//
//
//
//                      _oo0oo_
//                     o8888888o
//                     88" . "88
//                     (| -_- |)
//                     0\  =  /0
//                   ___/`---'\___
//                 .' \\|     |// '.
//                / \\|||  :  |||// \
//               / _||||| -:- |||||- \
//              |   | \\\  -  /// |   |
//              | \_|  ''\---/''  |_/ |
//              \  .-\__  '-'  ___/-. /
//            ___'. .'  /--.--\  `. .'___
//         ."" '<  `.___\_<|>_/___.' >' "".
//        | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//        \  \ `_.   \_ __\ /__ _/   .-` /  /
//    =====`-.____`.___ \_____/___.-`___.-'=====
//                      `=---='
//
//  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//

using grenius_api.Application.Extensions;
using grenius_api.Application.Services;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

namespace grenius_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .CreateLogger();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<GreniusContext>();
            builder.Services.Configure<SecurityOptions>(
            builder.Configuration.GetSection(SecurityOptions.Security));
            
            
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddStackExchangeRedisCache((options) =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "redis-dev";
            });

            builder.Services.AddScoped<IAnnotationService, AnnotationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var securitySection = builder.Configuration.GetSection("Security");
                    options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = securitySection["Issuer"],
                    
                    ValidateAudience = false,
                    ValidAudience = securitySection["Audience"],
                    ValidateLifetime = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySection["Secret"]!)),
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Grenius-API", 
                    Description = @"
            A public API inspired by the world-famous Genius service. 
            Get access to a huge database of song lyrics,
            artist and album archives, as well as analysis 
            and annotations to enrich your applications, websites or services 
            with the highest quality music information.
                    ",
                    Version = "0.0.1",
                    Contact = new OpenApiContact
                    {
                        Name = "Grents Artem",
                        Url = new Uri("https://github.com/ggrents")
                    }
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
              new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
            });


            builder.Host.UseSerilog();

            
            var app = builder.Build();

            app.UseApiExceptionHandling();
            
            app.UseSerilogRequestLogging();

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
        }
    }
}
