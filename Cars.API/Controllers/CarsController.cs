using Cars.Application;
using Cars.Domain;
using Cars.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cars.API.Controllers
{
    public class CarsController: BaseApiController
    {

        public CarsController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetCars()
        {
            var result = await Mediator.Send(new List.Query());
            if(result.IsSuccess) return Ok(result.Value);
            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(Guid id)
        {
            var result = await Mediator.Send(new Details.Query { Id = id });

            if (result == null || result.Value == null)
            {
                return NotFound();
            }
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car)
        {
            var result = await Mediator.Send(new Create.Command { Car = car });

            if (result == null)
            {
                return BadRequest();
            }
            if (result.IsSuccess && result.Value != null)
            {
                return CreatedAtAction(nameof(GetCar), new { id = result.Value.Id }, result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCar(Guid id, Car car)
        {
            car.Id = id;
            await Mediator.Send(new Edit.Command { Car = car });
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            await Mediator.Send(new Delete.Command { Id = id });
            return NoContent();
        }
    }
}
