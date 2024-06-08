using Kolokwium_s24242.Context;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApbdContext>();

var app = builder.Build();


if (app.Environment.IsDevelopment()) 

{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseHttpsRedirection();
app.MapControllers();

app.Run(); 