using PcController.Interfaces;
using PcController.Services;

namespace PcController
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? pluginFolder = builder.Configuration["PluginPath"];

            if (pluginFolder == null)
                throw new ArgumentNullException(nameof(pluginFolder));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSingleton<IPluginManager>(x => new PluginManager(pluginFolder));

            builder.Host.UseWindowsService();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}