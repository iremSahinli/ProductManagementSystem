using ManagmentSystem.Business.Extentions;
using ManagmentSystem.Business.Services.MailService;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.Extentions;
using ManagmentSystem.Presentation.Extentions;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Token create etme.
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddBusinessServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();


//Mail Activation conf.
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

//middleware
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error/500");
    app.UseStatusCodePagesWithReExecute("/Error/{0}"); // Diðer hatalar için yönlendirme.
    app.UseHsts();
}
else
{
    // Geliþtirme ortamýnda hatalarý görmek için developer exception page aktif edilir.
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");
app.MapDefaultControllerRoute();
app.Run();
