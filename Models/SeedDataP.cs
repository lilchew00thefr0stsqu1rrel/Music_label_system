using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMusicDistr.Data;
using System;
using System.Linq;

namespace MvcMusicDistr.Models;

public static class SeedDataP
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcMusicDistrContext(
            serviceProvider.GetRequiredService<DbContextOptions<MvcMusicDistrContext>>()))
        {


            // Look for any movies.
            if (context.Pair.Any())
            {
                return;   // DB has been seeded

            }


            context.Pair.AddRange(
                new Pair
                {
                    Name = "Передн. 6",
                    GaitID = 1,
                    OnGroundLeft = 0, 
                    OnGroundRight = 0,
                },
                new Pair
                {
                    Name = "Задн. 6",
                    GaitID = 1,
                    OnGroundLeft = 0,
                    OnGroundRight = 0,
                },
                new Pair
                {
                    Name = "Передн. 5",
                    GaitID = 1,
                    OnGroundLeft = 0,
                    OnGroundRight = 1,
                },
                new Pair
                {
                    Name = "Задн. 5",
                    GaitID = 1,
                    OnGroundLeft = 0,
                    OnGroundRight = 0,
                },
                new Pair
                {
                    Name = "Передн. 4",
                    GaitID = 1,
                    OnGroundLeft = 1,
                    OnGroundRight = 1,
                },
                new Pair
                {
                    Name = "Задн. 4",
                    GaitID = 1,
                    OnGroundLeft = 0,
                    OnGroundRight = 1,
                },
                new Pair
                {
                    Name = "Передн. 3",
                    GaitID = 1,
                    OnGroundLeft = 1,
                    OnGroundRight = 0,
                },
                new Pair
                {
                    Name = "Задн. 3",
                    GaitID = 1,
                    OnGroundLeft = 0,
                    OnGroundRight = 1,
                },
                 new Pair
                 {
                     Name = "Передн. 2",
                     GaitID = 1,
                     OnGroundLeft = 1,
                     OnGroundRight = 0,

                 },
                new Pair
                {
                    Name = "Задн. 2",
                    GaitID = 1,
                    OnGroundLeft = 1,
                    OnGroundRight = 1,
                },
                new Pair
                {
                    Name = "Передн. 1",
                    GaitID = 1,
                    OnGroundLeft = 0,
                    OnGroundRight = 0,
                },
                new Pair
                {
                    Name = "Задн. 1",
                    GaitID = 1,
                    OnGroundLeft = 1,
                    OnGroundRight = 0,
                }
            );



            context.SaveChanges();
        }


    }
}
