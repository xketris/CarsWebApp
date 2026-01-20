using Cars.Domain;
using Cars.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Cars.Application.Core;

namespace Cars.Application.Rentals
{
    public class CreateRental
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid CarId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
                var car = await _context.Cars.FindAsync(request.CarId);

                if (car == null) return Result<Unit>.Failure("Car not found");

                var isOccupied = await _context.Rentals
                    .AnyAsync(r =>
                        r.CarId == request.CarId &&
                        r.Status != RentalStatus.Cancelled &&
                        r.StartDate < request.EndDate &&
                        request.StartDate < r.EndDate,
                        cancellationToken);

                if (isOccupied)
                {
                    return Result<Unit>.Failure("Car is already rented in this time period.");
                }

                var days = (request.EndDate - request.StartDate).Days;
                var price = days * 100;

                var rental = new Rental
                {
                    AppUser = user,
                    Car = car,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    TotalPrice = price,
                    Status = RentalStatus.Confirmed
                };

                _context.Rentals.Add(rental);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create rental");

                return Result<Unit>.Success(Unit.Value);
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.StartDate).NotEmpty();
                RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate)
                    .WithMessage("End date must be after start date");
            }
        }
    }
}
