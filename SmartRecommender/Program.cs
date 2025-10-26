using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartRecommender.Application.RepositoryInterfaces;
using SmartRecommender.Domain.RepositoryInterfaces;
using SmartRecommender.Infrastructure.Context;
using SmartRecommender.Infrastructure.Repository;
using SmartRecommender.AI.Interfaces;
using SmartRecommender.AI.Services;

namespace SmartRecommender.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------------------------------
            // 🧩 Configure Services
            // -------------------------------------------------
            builder.Services.AddControllers();

            // EF Core Configuration (SQL Server)
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"), 
                    sqlOptions => sqlOptions.MigrationsAssembly("SmartRecommender.Infrastructure")));

            // ⚙️ DI for generic and specialized repositories
            builder.Services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
            builder.Services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
            builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
            builder.Services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
            // DI for services
            builder.Services.AddScoped<IAiRecommenderService, AiRecommenderService>();
            // ✅ Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartRecommender API",
                    Version = "v1",
                    Description = "API documentation for SmartRecommender (EF Core + JWT, .NET 9)"
                });
            });

            var app = builder.Build();

            // -------------------------------------------------
            // 🌐 Configure HTTP request pipeline
            // -------------------------------------------------
            if (app.Environment.IsDevelopment())
            {
                // ✅ Swagger endpoint will be https://localhost:{port}/swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartRecommender API v1");
                    c.RoutePrefix = "swagger"; // show under /swagger
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
