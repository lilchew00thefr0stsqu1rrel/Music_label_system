using Microsoft.EntityFrameworkCore;
using MvcMusicDistr.Data;

namespace MvcMusicDistr.Models;

public class SeedDataRT
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcMusicDistrContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcMusicDistrContext>>()))
        {


           


            context.SaveChanges();
        }


    }
}
