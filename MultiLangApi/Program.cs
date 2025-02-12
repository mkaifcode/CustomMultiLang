using MultiLangApi.Helpers.Response;
using MultiLangApi.Services.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJsonLocalization(options =>
{
    options.ResourceFilesDirectory = "Resources/"; // Folder where JSON files will be stored
});

// Add services to the container.

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<JsonLocalizationService>();

//builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure middleware for localization
//var supportedCultures = new[] { "en", "fr", "de", "es" }; // Define supported languages
//var localizationOptions = new RequestLocalizationOptions()
//    .SetDefaultCulture("en") // Default language
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);

//app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ResponseHelper.Configure(app.Services);

app.Run();
