using csharp_mfca.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("CORSPolicy");

app.UseHealthChecks("/health");

app.UseAuthorization();

app.UseTraceMiddleware();

app.MapControllers();

app.Run();
