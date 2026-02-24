using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMusicDistr.Data;
using System;
using System.Linq;

namespace MvcMusicDistr.Models;

public static class SeedDataG
{
    public static void Initialize(IServiceProvider serviceProvider) 
    { 
        using (var context = new MvcMusicDistrContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcMusicDistrContext>>()))
        {

            
            // Look for any movies.
            if (context.Gait.Any())
            {
                return;   // DB has been seeded
                
            }


context.Gait.AddRange(
    new Gait
    {Name = "галоп фишерфолд с правой ноги"
    },
    new Gait
    {
        Name = "рысь"
    },
    new Gait
    {
        Name = "иноходь"
    },
    new Gait
    {
        Name = "полупарный галоп правый"
    },
    new Gait
    {
        Name = "четырёхтактная иноходь 67% (4*пи/3)"
    }
);



context.SaveChanges();
        }
           

    }
}
