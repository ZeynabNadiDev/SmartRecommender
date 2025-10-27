using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartRecommender.AI.Interfaces;
using SmartRecommender.AI.Services;
using SmartRecommender.Application.Abstractions.ConectWithAI;
using SmartRecommender.Application.Abstractions.Repositories;
using SmartRecommender.Application.Abstractions.Services;
using SmartRecommender.Application.Services;
using SmartRecommender.Domain.Entities;

using SmartRecommender.Infrastructure.Context;
using SmartRecommender.Infrastructure.Repositories;

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
            builder.Services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();


            // DI for services
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IAiRecommenderService, AiRecommenderService>();
            builder.Services.AddScoped<IIntentExtractor, IntentExtractor>();
            builder.Services.AddScoped<IProductMatcher, ProductMatcher>();
            builder.Services.AddScoped<IResponseGenerator, ResponseGenerator>();
            builder.Services.AddScoped<IRecommenderEngine, AiRecommenderService>();
            builder.Services.AddScoped<IChatService, ChatService>();
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
