
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using StoreManagement.API;
using StoreManagement.Application.Auth;
using StoreManagement.Application.Categories;
using StoreManagement.Application.Contracts;
using StoreManagement.Application.Customers;
using StoreManagement.Application.Interfaces;
using StoreManagement.Application.InventoryManagement;
using StoreManagement.Application.Orders;
using StoreManagement.Application.Products;
using StoreManagement.Infrastructure;
using StoreManagement.Infrastructure.Auth;
using StoreManagement.Infrastructure.Data;
using StoreManagement.Infrastructure.DataAccess;
using StoreManagement.Infrastructure.Identity;


// ========================================
// 1. Create Builder
// ========================================

var builder = WebApplication.CreateBuilder(args);


// ========================================
// 2. Controllers
// ========================================

builder.Services.AddControllers();


// ========================================
// 3. Database
// ========================================

builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================================
// 4. ASP.NET Core Identity
// ========================================

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<StoreDbContext>();


// ========================================
// 5. JWT Authentication
// ========================================

var jwtKey = builder.Configuration["Jwt:Key"]?? throw new InvalidOperationException("JWT key is not configured.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
    });


// ========================================
// 6. Authorization
// ========================================

builder.Services.AddAuthorization();


// ========================================
// 7. Product Services
// ========================================

builder.Services.AddScoped<GetAllProducts>();
builder.Services.AddScoped<GetProductById>();
builder.Services.AddScoped<AddProduct>();
builder.Services.AddScoped<UpdateProduct>();
builder.Services.AddScoped<DeleteProduct>();

builder.Services.AddScoped<IProductData, ProductData>();


// ========================================
// 8. Category Services
// ========================================

builder.Services.AddScoped<AddCategory>();
builder.Services.AddScoped<GetAllCategories>();
builder.Services.AddScoped<GetCategoryById>();
builder.Services.AddScoped<UpdateCategory>();
builder.Services.AddScoped<DeleteCategory>();

builder.Services.AddScoped<ICategoryData, CategoryData>();


// ========================================
// 9. Customer Services
// ========================================

builder.Services.AddScoped<AddCustomer>();
builder.Services.AddScoped<GetAllCustomers>();
builder.Services.AddScoped<GetCustomerById>();
builder.Services.AddScoped<UpdateCustomer>();
builder.Services.AddScoped<DeleteCustomer>();

builder.Services.AddScoped<ICustomerData, CustomerData>();


// ========================================
// 10. Order Services
// ========================================

builder.Services.AddScoped<IOrderData, OrderData>();

builder.Services.AddScoped<CreateOrder>();
builder.Services.AddScoped<GetAllOrders>();
builder.Services.AddScoped<GetOrderById>();
builder.Services.AddScoped<GetMyOrders>();
builder.Services.AddScoped<CancelMyOrder>();
builder.Services.AddScoped<UpdateOrderStatus>();
builder.Services.AddScoped<DeleteOrder>();


// ========================================
// 11. Inventory Services
// ========================================

builder.Services.AddScoped<IInventoryData, InventoryData>();

builder.Services.AddScoped<GetAllInventory>();
builder.Services.AddScoped<GetInventoryById>();
builder.Services.AddScoped<AddInventory>();
builder.Services.AddScoped<UpdateInventory>();
builder.Services.AddScoped<DeleteInventory>();


// ========================================
// 12. Authentication Services
// ========================================

builder.Services.AddScoped<IAuthService, AuthService>();


// ========================================
// 13. Build Application
// ========================================

var app = builder.Build();


// ========================================
// 14. Seed Roles and Admin Account
// ========================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager =services.GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeeder.SeedRolesAsync(roleManager);

    var userManager =services.GetRequiredService<UserManager<ApplicationUser>>();

    await IdentitySeeder.SeedAdminAsync(userManager, builder.Configuration);
}


// ========================================
// 15. HTTP Request Pipeline
// ========================================

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
