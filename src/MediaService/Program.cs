using MediaService.Bootstraper;
using MediaService.Endpoint;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCommonCases();
builder.RegisterInMemoryDatabase();
builder.RegisterMinio();
builder.RegisterBroker();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMediaServiceEndpoints();

app.Run();

