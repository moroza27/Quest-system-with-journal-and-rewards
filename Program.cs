using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SweetBakeryQuest;
using SweetBakeryQuest.Factories;
using SweetBakeryQuest.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// Реєструація як Singleton
builder.Services.AddSingleton<QuestFactory>();
// Реєстрація нашого сервісу GameState
builder.Services.AddSingleton<GameState>();

await builder.Build().RunAsync();
