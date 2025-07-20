using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlistService.API.DTOs;
using OutlistService.Application.UseCases;
using OutlistService.Domain.Entities;

namespace OutlistService.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OutlistController : ControllerBase
    {
        private readonly OutlistUseCaseService _service;

        public OutlistController(OutlistUseCaseService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OutlistProduct product)
        {
            try
            {
                await _service.AddAsync(product);
                return CreatedAtAction(nameof(Get), new { code = product.ProductCode, version = "1.0" }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            await _service.RemoveAsync(code);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{code}/validity")]
        public async Task<IActionResult> UpdateValidity(string code, [FromBody] ValidityDto dto)
        {
            try
            {
                await _service.UpdateValidityAsync(code, dto.ValidFrom, dto.ValidTo);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            if (size > 200) size = 200;
            var result = await _service.GetPagedAsync(page, size);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var result = await _service.GetByProductCodeAsync(code);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
