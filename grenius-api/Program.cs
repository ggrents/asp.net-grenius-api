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

using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

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
            builder.Services.AddSwaggerGen(c =>
            {
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
                        Email="gggrents@gmail.com",
                        Url = new Uri("https://github.com/ggrents")
                    }
                    });
            });

            builder.Host.UseSerilog();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            
            app.MapControllers();
           
            app.Run();
        }
    }
}
