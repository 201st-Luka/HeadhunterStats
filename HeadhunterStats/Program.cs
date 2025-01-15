using ClashOfClans.Core;
using HeadhunterStats.Components;
using HeadhunterStats.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register the configuration section
builder.Services.Configure<StaticInfo>(builder.Configuration.GetSection("StaticInfo"));

// Register ClashOfClans service
builder.Services.AddClashOfClans(options =>
{
    var clashOptions = new ClashOfClansOptionsV2();
    builder.Configuration.GetSection(ClashOfClansOptions.ClashOfClans).Bind(clashOptions);

    options.Tokens.AddRange(clashOptions.Tokens);
    options.MaxRequestsPerSecond = clashOptions.MaxRequestsPerSecond;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();