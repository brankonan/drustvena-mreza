
using WebApplication2.Repositories;



namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔧 1. Priprema connection stringa
            var dbPath = Path.Combine(AppContext.BaseDirectory, "database", "mydatabase.db");
            var connectionString = $"Data Source={dbPath}";

            // ✅ Učitavanje konekcionog stringa u konfiguraciju
            // dodao sam ovo za user deo koda
            builder.Configuration["ConnectionStrings:SQLiteConnection"] = connectionString;

            // ✅ 2. Registruj repo sa ručno prosleđenim connection stringom
            builder.Services.AddSingleton(new GrupaDbRepository(connectionString));
            //dodao sam ovo za user deo koda
            builder.Services.AddSingleton<UserDbRepository>();

            // ✅ 3. Ostalo
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowAllOrigins");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

