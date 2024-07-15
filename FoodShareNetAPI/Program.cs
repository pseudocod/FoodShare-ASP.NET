
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FoodShareNet.Repository.Data;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;

namespace FoodShareNetAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins("*")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICourierService, CourierService>();
            builder.Services.AddScoped<IDonationService, DonationService>();
            builder.Services.AddScoped<IDonorService, DonorService>();
            builder.Services.AddScoped<IFoodShareDbContext, FoodShareNetDbContext>();

            // Add services to the container.
            builder.Services.AddDbContext<FoodShareNetDbContext>(options =>
                options.UseSqlServer(builder.Configuration.
                GetConnectionString("DefaultConnection")));


            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            app.UseCors();

            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}