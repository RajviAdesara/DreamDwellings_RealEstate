//using RealEstate.Models;
//using System.Reflection;
//using FluentValidation.AspNetCore;
//using RealEstate.Services;
//using Hangfire;
//using RealEstate.Controllers;

//var builder = WebApplication.CreateBuilder(args);

////Login 
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddSession();
//builder.Services.AddHttpClient();


//builder.Services.AddLogging();

//builder.Services.AddTransient<EmailSenderService>();

//builder.Services.AddControllers().AddFluentValidation(r => r.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

//// Add Hangfire services
//builder.Services.AddHangfire(config => config
//    .UseSqlServerStorage(builder.Configuration.GetConnectionString("ConnectionString"))); // Your DB connection string
//builder.Services.AddHangfireServer();

//builder.Services.AddHttpContextAccessor();


//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}

//app.UseSession();

//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.UseHangfireDashboard();

//// Start Hangfire
//RecurringJob.AddOrUpdate<UserController>(
//    "send-promotional-emails",
//    x => x.SendPromotionalEmail(),
//    Cron.Daily); // Runs once a day


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();


using RealEstate.Models;
using System.Reflection;
using FluentValidation.AspNetCore;
using RealEstate.Services;
using Hangfire;
using RealEstate.Controllers;
;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddLogging();

// ? Change to `Scoped` since it's used in Hangfire
builder.Services.AddScoped<EmailSenderService>();

builder.Services.AddControllers().AddFluentValidation(r => r.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

// ? Hangfire Configuration
builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("ConnectionString")));
builder.Services.AddHangfireServer();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseHangfireDashboard();

// ? Register Recurring Job Correctly
using (var scope = app.Services.CreateScope())
{
    var emailService = scope.ServiceProvider.GetRequiredService<EmailSenderService>();
    RecurringJob.AddOrUpdate("send-promotional-emails",
        () => emailService.SendPromotionalEmailWithEmail("adesararajvi912@gmail.com"), 
        Cron.Daily);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
