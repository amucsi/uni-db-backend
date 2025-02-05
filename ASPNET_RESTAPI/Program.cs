
using ASPNET_RESTAPI.DAL;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_RESTAPI {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<UniDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 30))));

            builder.Services.AddScoped<StudentRepository>();
            builder.Services.AddScoped<CourseRepository>();
            builder.Services.AddScoped<MajorRepository>();

            builder.Services.AddControllers();

            //swashbuckle swagger
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //swashbuckle
            //if (app.Environment.IsDevelopment()) {
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseCors(policy =>
                policy.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
