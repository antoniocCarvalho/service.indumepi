using Microsoft.EntityFrameworkCore;
using service.indumepi.Application.Service.ClientRequest;
using service.indumepi.Application.Service.EstoqueRequest;
using service.indumepi.Application.Service.FamilyRequest;
using service.indumepi.Application.Service.ItemRequest;
using service.indumepi.Application.Service.OrderRequest;
using service.indumepi.Infra.Data;
using service.indumepi.Infra.Data.Features;

var builder = WebApplication.CreateBuilder(args);

// Corrigir o uso de Configuration para builder.Configuration
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient <ClientService>();
builder.Services.AddScoped<ClientRepository>();

builder.Services.AddHttpClient<ItemService>();
builder.Services.AddScoped<ProductRepository>();


builder.Services.AddHttpClient<FamilyService>();
builder.Services.AddScoped<FamilyRepository>();

builder.Services.AddHttpClient<OrderService>();
builder.Services.AddScoped<OrderRepository>();


builder.Services.AddHttpClient<EstoqueService>();
builder.Services.AddScoped<EstoqueRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder.WithOrigins("http://127.0.0.1:5500")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();
    context.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowLocalhost");


app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();

app.MapGet("/", () => "API is running.");

app.Run();
