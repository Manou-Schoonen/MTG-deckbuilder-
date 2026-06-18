using Microsoft.AspNetCore.Cors.Infrastructure;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.API.Services;
using WebAppMTGLogic.Interfaces;
using WebAppMTGDAL.ScryfallAPI.Services;
using WebAppMTGDAL.Database.Repos;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Services;
using WebAppMTGLogic.FormatRules.Interface;
using WebAppMTGLogic.FormatRules.Formats;

namespace WebAppMTG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddHttpClient<IScryfallService, ScryfallService>(client =>
            {
                client.BaseAddress = new Uri("https://api.scryfall.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "WebAppMTG/1.0");
                client.DefaultRequestHeaders.Add("Accept", "application/json;q=0.9,*/*;q=0.8");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddScoped<ICardLogicService, CardLogicService>();
            builder.Services.AddScoped<ILegalityRule, Standard>();
            builder.Services.AddScoped<IUserDeckService, UserDeckService>();

            builder.Services.AddScoped<IMySQLDeckRepo>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                return new SqlServerDeckRepo(connectionString!);
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

            app.UseSession();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapRazorPages();

            app.Run();
        }
    }
}
