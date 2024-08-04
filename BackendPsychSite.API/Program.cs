using Minio.AspNetCore;
using BackendPsychSite.UseCases.Interfaces;
using BackendPsychSite.Infrastructure.Services;
using BackendPsychSite.DataAccess.Repositories;
using Minio;
namespace BackendPsychSite.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IDataSetRepository, DataSetRepository>();
            builder.Services.AddTransient<IDataSetService, DataSetService>();
            builder.Services.AddMinio(options =>
            {
                options.Endpoint = "localhost:9000";
                options.SecretKey = "vXO4eIFOGL0FgOykmn9n7nRmEtoDEAlC0C01lLOq";
                options.AccessKey = "ZuqvpoNqcyxn9XGsFlxG";
            });
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
