using Video.Web.Provider;
using Video.Web.Services;

namespace Video.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IVideoService, VideoProvider>();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 700_000_000; // 100 MB
            });

            builder.Services.AddRazorPages();
            builder.Services.AddEndpointsApiExplorer();            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}