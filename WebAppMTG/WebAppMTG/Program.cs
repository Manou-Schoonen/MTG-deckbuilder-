using DAL.Services;
namespace WebAppMTG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient<ScryfallService>(client =>
            {
                client.BaseAddress = new Uri("https://api.scryfall.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "WebAppMTG/1.0");
                client.DefaultRequestHeaders.Add("Accept", "application/json;q=0.9,*/*;q=0.8");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
