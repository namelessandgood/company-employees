using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;

var builder = WebApplication.CreateBuilder(args);


#pragma warning disable CS0618 // Type or member is obsolete
LogManager.LoadConfiguration(string.Concat
        (Directory.GetCurrentDirectory(), "./nlog.config"));
#pragma warning restore CS0618 // Type or member is obsolete

builder.Services.ConfigureCors()
    .ConfigureIisIntegration()
    .ConfigureLoggerService()
    .ConfigureRepositoryManager()
    .ConfigureServiceManager()
    .AddAutoMapper(typeof(Program));




NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First();

builder.Services.ConfigureSqlContext(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    }).
    //AddXmlDataContractSerializerFormatters().
    //AddCustomCsvFormatter()

    AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
//fix for the error: System.InvalidOperationException: No service for type 'Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager' has been registered.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");
app.UseAuthorization();

/*
app.Use(async (context, next) =>
{
    Console.WriteLine("Logic before executing the next delegate in the Use method");
    await next.Invoke();
    Console.WriteLine("Logic after executing the next delegate in the Use method");
});

app.Map("/usingmapbranch", builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("Map branch logic in the Use method before the next delegate");
        await next.Invoke();
        Console.WriteLine("Map branch logic in the Use method after the next delegate");
    });
    builder.Run(async context =>
    {
        Console.WriteLine("Map branch response to the client in the Run method");
        await context.Response.WriteAsync("Hello from the map branch.");
    });
});

app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"),
    applicationBuilder =>
    {
        applicationBuilder.Run(
            async context => { await context.Response.WriteAsJsonAsync("hello there"); });
    });

app.Run(async context =>
{
    Console.WriteLine("Writing the response to the client in the Run method");
    await context.Response.WriteAsync("Hello from the middleware component.");
});
*/
app.MapControllers();
app.Run();