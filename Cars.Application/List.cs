using Cars.Domain;
using Cars.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Application
{
    public class List
    {

        public class Query: IRequest<Result<List<Car>>>
        {


        }

        public class Handler: IRequestHandler<Query, Result<List<Car>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Car>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Cars.ToListAsync();
                return Result<List<Car>>.Success(result);
            }
        }

    }
}
