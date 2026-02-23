using STEMotion.Presentation.Middleware;

namespace STEMotion.Presentation.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInfrastructure(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
            /* 
              app.UseRouting();
            */
            // app.UseHttpsRedirection(); // for production only
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

        }
    }
}
