using MediaService.Bootstraper;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCommonCases();
builder.RegisterInMemoryDatabase();
builder.RegisterMinio();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

