using MISA.AMIS.BL.BaseBL;
using MISA.CUKCUK.DL.BaseDL;
using MISA.CUKCUK.DL;
using MISA.AMIS.BL.MaterialBL;
using MISA.CUKCUK.DL.MaterialDL;
using MISA.AMIS.BL.StockBL;
using MISA.CUKCUK.DL.StockDL;
using MISA.AMIS.BL.UnitBL;
using MISA.CUKCUK.DL.UnitDL;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));

builder.Services.AddScoped<IMaterialBL, MaterialBL>();
builder.Services.AddScoped<IMaterialDL, MaterialDL>();

builder.Services.AddScoped<IStockBL, StockBL>();
builder.Services.AddScoped<IStockDL, StockDL>();

builder.Services.AddScoped<IUnitBL, UnitBL>();
builder.Services.AddScoped<IUnitDL, UnitDL>();
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
