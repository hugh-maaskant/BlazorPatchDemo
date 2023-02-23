using BlazorPatchDemo.Server.MongoDB;
using BlazorPatchDemo.Server.Settings;
using BlazorPatchDemo.Shared.Entities;
using Microsoft.OpenApi.Models;

const string title = "BlazorPatchDemo.Catalog";
const string version = "v1";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson();

builder.Services.AddRazorPages();

// Add default IProblemDetailsService to the container 
builder.Services.AddProblemDetails();

builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version }));

builder.Services.AddMongo().AddMongoRepository<Item>("items");

var app = builder.Build();

ServiceSettings? serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

app.Logger.LogInformation("Starting {Title}", title);
app.Logger.LogInformation("Running in the {Environment} environment", app.Environment.EnvironmentName);
app.Logger.LogInformation("Using database {Database}", serviceSettings is not null
    ? serviceSettings.ServiceName
    : throw new Exception($"{nameof(ServiceSettings)} has not been configured"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} - {version}"));
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Return a ProblemDetails when returning a non-success Status Code from Controllers
app.UseStatusCodePages();

// Do not use a path => will return ProblemDetails on Exceptions
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
