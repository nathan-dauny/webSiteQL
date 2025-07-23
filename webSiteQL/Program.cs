var builder = WebApplication.CreateBuilder(args);

// Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// En prod (Render), écoute sur le port 80, sur toutes les IP
if (!app.Environment.IsDevelopment())
{
    app.Urls.Add("http://*:80");
}

if (app.Environment.IsDevelopment())
{
    // Exception details en dev
    app.UseDeveloperExceptionPage();

    // Lancement automatique du navigateur local sur http://localhost:5000
    var psi = new System.Diagnostics.ProcessStartInfo
    {
        FileName = "http://localhost:5000",
        UseShellExecute = true
    };
    System.Diagnostics.Process.Start(psi);

    // Config Kestrel pour écouter localement sur 5000
    app.Urls.Add("http://localhost:5000");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Query}/{action=Index}/{id?}");

app.Run();
