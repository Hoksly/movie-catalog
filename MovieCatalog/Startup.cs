using Microsoft.EntityFrameworkCore;
using MovieCatalog.DataAccess;
using Microsoft.Extensions.Configuration; 

namespace MovieCatalog
{
    public class Startup
    {
        static public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FilmDbContext>(options => 
                options.UseNpgsql("Host=localhost:5433;Database=movie-db;Username=postgres;Password=postgres") 
            );

            services.AddControllersWithViews();
            services.AddRazorPages();
        }
    }
}
