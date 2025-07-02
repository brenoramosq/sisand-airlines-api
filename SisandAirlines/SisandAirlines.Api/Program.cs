using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SisandAirlines.Api.Configurations;
using SisandAirlines.Api.Configurations.Middlewares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddCors();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddApiVersioningExtension();
        builder.Services.AddSwaggerExtension();
        builder.Services.AddDependencyInjectionExtension(builder.Configuration);
        builder.Services.AddJwtExtension(builder.Configuration);
        builder.Services.AddMediatorExtension();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }

            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        });
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();
        
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());

        app.UseMiddleware(typeof(ErrorHandlingMiddleware));


        app.MapControllers();

        app.Run();
    }
}