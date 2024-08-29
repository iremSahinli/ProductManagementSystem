using ManagmentSystem.Business.Extentions;
using ManagmentSystem.Infrastructure.Extentions;
using ManagmentSystem.Presentation.Extentions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddBusinessServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");
app.MapDefaultControllerRoute();
app.Run();
