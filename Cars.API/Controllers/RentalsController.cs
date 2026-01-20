using Cars.Application.Core;
using Cars.Application.Rentals;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cars.API.Controllers
{
    public class RentalsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetMyRentals()
        {
            var username = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.Email);

            return HandleResult(await Mediator.Send(new ListRentals.Query { Username = username }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental(CreateRental.Command command)
        {
            command.Username = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.Email);

            return HandleResult(await Mediator.Send(command));
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(result.Error);
        }

    }
}