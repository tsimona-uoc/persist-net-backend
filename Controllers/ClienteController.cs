using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs.Cliente;
using persist_net_backend.Models;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        private ClienteResponse MapToResponse(Cliente cliente)
        {
            return new ClienteResponse
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Documentacion = cliente.Documentacion,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                FechaRegistro = cliente.FechaRegistro
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponse>> GetCliente(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
                return NotFound();

            return Ok(MapToResponse(cliente));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponse>>> GetAllClientes()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(clientes.Select(MapToResponse));
        }

        [HttpPost]
        public async Task<ActionResult<ClienteResponse>> CreateCliente([FromBody] CreateClientRequest cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCliente = await _clienteService.CreateClienteAsync(new Cliente
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Documentacion = cliente.Documentacion,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                LastModifiedBy = "system",
                LastModifiedAt = DateTime.Now
            });

            return CreatedAtAction(nameof(GetCliente), new { id = createdCliente.Id }, MapToResponse(createdCliente));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] UpdateClientRequest cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCliente = await _clienteService.GetClienteByIdAsync(id);
            if (existingCliente == null)
                return NotFound();

            existingCliente.Nombre = cliente.Nombre ?? existingCliente.Nombre;
            existingCliente.Apellido = cliente.Apellido ?? existingCliente.Apellido;
            existingCliente.Documentacion = cliente.Documentacion ?? existingCliente.Documentacion;
            existingCliente.Telefono = cliente.Telefono ?? existingCliente.Telefono;
            existingCliente.Email = cliente.Email ?? existingCliente.Email;
            existingCliente.LastModifiedBy = "system";
            existingCliente.LastModifiedAt = DateTime.Now;

            await _clienteService.UpdateClienteAsync(existingCliente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var success = await _clienteService.DeleteClienteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
