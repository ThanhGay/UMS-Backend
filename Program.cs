using Microsoft.EntityFrameworkCore;
using Server.DbContexts;
using Server.Services.Implements;
using Server.Services.Interfaces;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    }
                );
            });

            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<IBuildingService, BuildingService>();
            builder.Services.AddScoped<IChuongTrinhKhungService, ChuongTrinhKhungService>();
            builder.Services.AddScoped<ILopHPService, LopHPService>();
            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            // http://192.168.59.245:7122/swagger/index.html
            app.UseStaticFiles();
            app.Run();
        }
    }
}
