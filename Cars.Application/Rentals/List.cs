using Cars.Application.Core;
using Cars.Domain;
using Cars.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cars.Application.Rentals
{
    public class List
    {
        public class Query : IRequest<Result<List<RentalDTO>>>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<RentalDTO>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<RentalDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var rentals = await _context.Rentals
                    .Include(r => r.Car)
                    .Where(r => r.AppUser.UserName == request.Username)
                    .OrderByDescending(r => r.StartDate)
                    .ToListAsync(cancellationToken);

                var rentalsDto = rentals.Select(r => new RentalDTO
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    TotalPrice = r.TotalPrice,
                    Status = r.Status.ToString(),
                    CarId = r.Car.Id,
                    CarBrand = r.Car.Brand,
                    CarModel = r.Car.Model
                }).ToList();

                return Result<List<RentalDTO>>.Success(rentalsDto);
            }
        }
    }
}