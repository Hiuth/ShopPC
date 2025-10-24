using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ShopPC.Configuration;
using ShopPC.Data;
using ShopPC.Exceptions;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Repository.InterfaceRepository;
using ShopPC.Service.ImplementationsService;
using ShopPC.Service.InterfaceService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ShopPC API",
        Version = "v1",
        Description = "API for ShopPC application"
    });
    c.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
// Configure Cloudinary
builder.Services.Configure<CloudinaryConfig>(
    builder.Configuration.GetSection("Cloudinary"));

// Add repository and service dependencies
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepostiory>();
builder.Services.AddScoped<IProductImgRepository, ProductImgRepository>();
builder.Services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
builder.Services.AddScoped<IAttributesRepository, AttributesRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IProductUnitRepository, ProductUnitRepository>();
builder.Services.AddScoped<IPcBuildItemRepository, PcBuildItemRepository>();
builder.Services.AddScoped<IPcBuildRepository, PcBuildRepository>();
builder.Services.AddScoped<IWarrantyRecordRepository, WarrnatyRecordRepository>();

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IAttributesService, AttributesService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductImgService,ProductImgService>();
builder.Services.AddScoped<IProductAttributeSerivce, ProductAttributeService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IProductUnitService, ProductUnitService>();
builder.Services.AddScoped<IPcBuildItemService, PcBuildItemService>();
builder.Services.AddScoped<IPcBuildService, PcBuildService>();
builder.Services.AddScoped<IWarrantyRecordService, WarrantyService>();
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));
builder.Services.AddScoped<EmailService>();
builder.Services.AddSingleton<OtpService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSecurityConfiguration(builder.Configuration);
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<TokenValidator>();

builder.Services.AddControllers();


var app = builder.Build();

app.UseMiddleware<TokenBlacklistMiddleware>();

app.UseMiddleware<GlobalExceptionHandler>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopPC API V1");
        //c.RoutePrefix = string.Empty;  // Đường dẫn để truy cập Swagger UI
    });
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // nếu chưa có schema, EF sẽ tạo
}


app.Run();
