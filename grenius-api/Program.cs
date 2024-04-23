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
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .CreateLogger();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddDbContext<GreniusContext>();
            builder.Services.AddStackExchangeRedisCache((options) =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "redis-dev";
            });

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });


            builder.Services.Configure<SecurityOptions>(
            builder.Configuration.GetSection(SecurityOptions.Security));
            builder.Services.AddScoped<IAnnotationService, AnnotationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var securitySection = builder.Configuration.GetSection("Security");
                    options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = securitySection["Issuer"],
                    
                    ValidateAudience = true,
                    ValidAudience = securitySection["Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySection["Secret"]!)),
                };
            });

            builder.Services.AddAuthorization();

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

                var securityScheme = new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new [] { "Bearer"} } 
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
