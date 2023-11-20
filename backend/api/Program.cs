using api;
using api.middelware;
using api.Middleware;
using infrastructure;
using infrastructure.repository;
using service.services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}


builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<PasswordHashRepository>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<UserService>();


builder.Services.AddJwtService();
builder.Services.AddSwaggerGenWithBearerJWT();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthentication();
app.UseAuthorization(); // Add it here

app.MapControllers();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseMiddleware<JwtBearerHandler>();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();

