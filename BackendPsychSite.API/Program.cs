using BackendPsychSite.DataAccess.Repositories;
using BackendPsychSite.Infrastructure.Services;
using BackendPsychSite.UseCases.Interfaces;
using Minio;
using Minio.AspNetCore;
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
                options.SecretKey = "nlrpP0Yd34kdB2PfbQZT0xak3H2thG3Qn05UpORo";
                options.AccessKey = "5Cq2BoIL63yfeASTnnAi";
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
