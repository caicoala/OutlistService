using Microsoft.AspNetCore.Mvc;
using OutlistService.API.DTOs;
using OutlistService.Application.UseCases;
using OutlistService.Domain.Entities;

namespace OutlistService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutlistController : ControllerBase
    {
        private readonly OutlistUseCaseService _service;

        public OutlistController(OutlistUseCaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OutlistProduct product)
        {
            await _service.AddAsync(product);
            return Ok();
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            await _service.RemoveAsync(code);
            return NoContent();
        }

        [HttpPut("{code}/validity")]
        public async Task<IActionResult> UpdateValidity(string code, [FromBody] ValidityDto dto)
        {
            await _service.UpdateValidityAsync(code, dto.ValidFrom, dto.ValidTo);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            if (size > 200) size = 200;
            var result = await _service.GetPagedAsync(page, size);
            return Ok(result);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var result = await _service.GetByProductCodeAsync(code);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
