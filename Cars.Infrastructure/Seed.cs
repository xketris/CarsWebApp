using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cars.Domain;

namespace Cars.Infrastructure
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (context.Cars.Any()) return;

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
        }
    }
}
