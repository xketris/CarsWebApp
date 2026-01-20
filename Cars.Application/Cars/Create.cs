using Cars.Application.Core;
using Cars.Domain;
using Cars.Infrastructure;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.Application.Cars
{
    public class Create
    {
        public class Command : IRequest<Result<Car>>
        {

            public required Car Car { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Car>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Car>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Cars.Add(request.Car);
                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                {
                    return Result<Car>.Failure("Nie udało się zapisać samochodu");
                }
                return Result<Car>.Success(request.Car);
            }
        }

        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Car).SetValidator(new CarValidator());
            }

        }
    }
}
