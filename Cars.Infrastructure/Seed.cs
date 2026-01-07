using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cars.Domain;
using Microsoft.AspNetCore.Identity;

namespace Cars.Infrastructure
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if (!context.Cars.Any())
            {
                var cars = new List<Car>
                {
                    new Car
                    {
                        Brand = "Mazda",
                        Model = "CX60",
                        DoorsNumber = 6,
                        LuggageCapacity = 570,
                        EngineCapacity = 2488,
                        FuelType = FuelType.Hybrid,
                        ProductionDate = DateTime.UtcNow.AddMonths(-1),
                        CarFuelConsumption = 10.1,
                        BodyType = BodyType.SUV
                    }
                };

                await context.Cars.AddRangeAsync(cars);
                await context.SaveChangesAsync();
            };

            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{DisplayName="franek", UserName="franek123", Email="franek123@test.com"},
                    new AppUser{DisplayName="maks", UserName="maks33", Email="maks33@test.com"},
                    new AppUser{DisplayName="daniel", UserName="daniel0", Email="daniel0@test.com"},
                    new AppUser{DisplayName="emilia", UserName="emilia4", Email="emilia4@test.com"}

                };

                foreach(var user in users)
                {
                    var result = await userManager.CreateAsync(user, "Has!o0123");

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"--> Utworzono użytkownika: {user.UserName}");
                    }
                    else
                    {
                        Console.WriteLine($"--> BŁĄD przy tworzeniu {user.UserName}:");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"   - {error.Description}");
                        }
                    }
                }
            }
            ;
            

        }
    }
}
