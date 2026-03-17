using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Models;
using System.Xml.Linq;

namespace persist_net_backend.Repositories
{
    public class ExportRepository : IExportRepository
    {
        private readonly AppDbContext _context;

        public ExportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, List<Dictionary<string, object?>>>> GetAllDataAsync()
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Database",
                    await GenerateClientsXmlAsync(),
                    await GenerateHotelsXmlAsync(),
                    await GenerateUsersXmlAsync(),
                    await GenerateHabitacionesXmlAsync(),
                    await GenerateReservasXmlAsync(),
                    await GenerateEstanciasXmlAsync(),
                    await GenerateFacturasXmlAsync(),
                    await GeneratePagosXmlAsync(),
                    await GenerateConsumosExtrasXmlAsync(),
                    await GenerateTarifasXmlAsync(),
                    await GenerateMetodosPagoXmlAsync(),
                    await GenerateRegimenesXmlAsync(),
                    await GenerateServiciosExtrasXmlAsync(),
                    await GenerateTemporadasXmlAsync(),
                    await GenerateTiposHabitacionXmlAsync(),
                    await GenerateEstadosHabitacionXmlAsync(),
                    await GenerateEstadosEstanciaXmlAsync(),
                    await GenerateEstadosReservaXmlAsync()
                )
            );

            // Convertir XDocument a diccionario para compatibilidad
            return ConvertXmlToDictionary(doc);
        }

        public async Task<List<Dictionary<string, object?>>> GetTableDataAsync(string tableName)
        {
            return tableName.ToLower() switch
            {
                "cliente" or "clientes" => await ConvertToListAsync(await GenerateClientsXmlAsync(), "Cliente"),
                "hotel" or "hoteles" => await ConvertToListAsync(await GenerateHotelsXmlAsync(), "Hotel"),
                "user" or "usuario" or "usuarios" => await ConvertToListAsync(await GenerateUsersXmlAsync(), "User"),
                "habitacion" or "habitaciones" => await ConvertToListAsync(await GenerateHabitacionesXmlAsync(), "Habitacion"),
                "reserva" or "reservas" => await ConvertToListAsync(await GenerateReservasXmlAsync(), "Reserva"),
                "estancia" or "estancias" => await ConvertToListAsync(await GenerateEstanciasXmlAsync(), "Estancia"),
                "factura" or "facturas" => await ConvertToListAsync(await GenerateFacturasXmlAsync(), "Factura"),
                "pago" or "pagos" => await ConvertToListAsync(await GeneratePagosXmlAsync(), "Pago"),
                "consumoextra" or "consumosextra" => await ConvertToListAsync(await GenerateConsumosExtrasXmlAsync(), "ConsumoExtra"),
                "tarifa" or "tarifas" => await ConvertToListAsync(await GenerateTarifasXmlAsync(), "Tarifa"),
                "metodopago" or "metodospago" => await ConvertToListAsync(await GenerateMetodosPagoXmlAsync(), "MetodoPago"),
                "regimen" or "regimenes" => await ConvertToListAsync(await GenerateRegimenesXmlAsync(), "Regimen"),
                "servicioextra" or "serviciosextra" => await ConvertToListAsync(await GenerateServiciosExtrasXmlAsync(), "ServicioExtra"),
                "temporada" or "temporadas" => await ConvertToListAsync(await GenerateTemporadasXmlAsync(), "Temporada"),
                "tipohabitacion" or "tiposhabitacion" => await ConvertToListAsync(await GenerateTiposHabitacionXmlAsync(), "TipoHabitacion"),
                "estadohabitacion" or "estadoshabitacion" => await ConvertToListAsync(await GenerateEstadosHabitacionXmlAsync(), "EstadoHabitacion"),
                "estadoestancia" or "estadosestancia" => await ConvertToListAsync(await GenerateEstadosEstanciaXmlAsync(), "EstadoEstancia"),
                "estadoreserva" or "estadosreserva" => await ConvertToListAsync(await GenerateEstadosReservaXmlAsync(), "EstadoReserva"),
                _ => new List<Dictionary<string, object?>>()
            };
        }

        private async Task<XElement> GenerateClientsXmlAsync()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return new XElement("Clientes",
                clientes.Select(c => new XElement("Cliente",
                    new XElement("Id", c.Id),
                    new XElement("Nombre", c.Nombre),
                    new XElement("Apellido", c.Apellido),
                    new XElement("Documentacion", c.Documentacion),
                    new XElement("Telefono", c.Telefono),
                    new XElement("Email", c.Email),
                    new XElement("FechaRegistro", c.FechaRegistro)
                ))
            );
        }

        private async Task<XElement> GenerateHotelsXmlAsync()
        {
            var hoteles = await _context.Hotels.ToListAsync();
            return new XElement("Hoteles",
                hoteles.Select(h => new XElement("Hotel",
                    new XElement("Id", h.Id),
                    new XElement("Name", h.Name),
                    new XElement("Description", h.Description),
                    new XElement("Address", h.Address),
                    new XElement("PhoneNumber", h.PhoneNumber),
                    new XElement("Email", h.Email)
                ))
            );
        }

        private async Task<XElement> GenerateUsersXmlAsync()
        {
            var users = await _context.Users.ToListAsync();
            return new XElement("Users",
                users.Select(u => new XElement("User",
                    new XElement("Id", u.Id),
                    new XElement("Name", u.Name),
                    new XElement("Surname", u.Surname),
                    new XElement("Email", u.Email),
                    new XElement("PasswordHash", u.PasswordHash)
                ))
            );
        }

        private async Task<XElement> GenerateHabitacionesXmlAsync()
        {
            var habitaciones = await _context.Habitaciones.ToListAsync();
            return new XElement("Habitaciones",
                habitaciones.Select(h => new XElement("Habitacion",
                    new XElement("Id", h.Id),
                    new XElement("HotelId", h.HotelId),
                    new XElement("TipoHabitacionId", h.TipoHabitacionId),
                    new XElement("EstadoHabitacionId", h.EstadoHabitacionId),
                    new XElement("Planta", h.Planta),
                    new XElement("Numero", h.Numero)
                ))
            );
        }

        private async Task<XElement> GenerateReservasXmlAsync()
        {
            var reservas = await _context.Reservas.ToListAsync();
            return new XElement("Reservas",
                reservas.Select(r => new XElement("Reserva",
                    new XElement("Id", r.Id),
                    new XElement("ClienteId", r.ClienteId),
                    new XElement("HabitacionId", r.HabitacionId),
                    new XElement("RegimenId", r.RegimenId),
                    new XElement("EstadoReservaId", r.EstadoReservaId),
                    new XElement("FechaEntrada", r.FechaEntrada),
                    new XElement("FechaSalida", r.FechaSalida),
                    new XElement("PrecioActual", r.PrecioActual),
                    new XElement("FechaCreacion", r.FechaCreacion)
                ))
            );
        }

        private async Task<XElement> GenerateEstanciasXmlAsync()
        {
            var estancias = await _context.Estancias.ToListAsync();
            return new XElement("Estancias",
                estancias.Select(e => new XElement("Estancia",
                    new XElement("Id", e.Id),
                    new XElement("ReservaId", e.ReservaId),
                    new XElement("EstadoEstanciaId", e.EstadoEstanciaId),
                    new XElement("FechaCheckIn", e.FechaCheckIn),
                    new XElement("FechaCheckOut", e.FechaCheckOut)
                ))
            );
        }

        private async Task<XElement> GenerateFacturasXmlAsync()
        {
            var facturas = await _context.Facturas.ToListAsync();
            return new XElement("Facturas",
                facturas.Select(f => new XElement("Factura",
                    new XElement("Id", f.Id),
                    new XElement("EstanciaId", f.EstanciaId),
                    new XElement("ClienteId", f.ClienteId),
                    new XElement("Descuento", f.Descuento),
                    new XElement("Total", f.Total),
                    new XElement("FechaEmision", f.FechaEmision),
                    new XElement("Pagada", f.Pagada)
                ))
            );
        }

        private async Task<XElement> GeneratePagosXmlAsync()
        {
            var pagos = await _context.Pagos.ToListAsync();
            return new XElement("Pagos",
                pagos.Select(p => new XElement("Pago",
                    new XElement("Id", p.Id),
                    new XElement("FacturaId", p.FacturaId),
                    new XElement("MetodoPagoId", p.MetodoPagoId),
                    new XElement("Importe", p.Importe),
                    new XElement("FechaPago", p.FechaPago)
                ))
            );
        }

        private async Task<XElement> GenerateConsumosExtrasXmlAsync()
        {
            var consumos = await _context.ConsumosExtra.ToListAsync();
            return new XElement("ConsumosExtras",
                consumos.Select(c => new XElement("ConsumoExtra",
                    new XElement("Id", c.Id),
                    new XElement("EstanciaId", c.EstanciaId),
                    new XElement("ServicioExtraId", c.ServicioExtraId),
                    new XElement("Cantidad", c.Cantidad),
                    new XElement("PrecioUnitario", c.PrecioUnitario),
                    new XElement("Fecha", c.Fecha)
                ))
            );
        }

        private async Task<XElement> GenerateTarifasXmlAsync()
        {
            var tarifas = await _context.Tarifas.ToListAsync();
            return new XElement("Tarifas",
                tarifas.Select(t => new XElement("Tarifa",
                    new XElement("Id", t.Id),
                    new XElement("TemporadaId", t.TemporadaId),
                    new XElement("PrecioNoche", t.PrecioNoche)
                ))
            );
        }

        private async Task<XElement> GenerateMetodosPagoXmlAsync()
        {
            var metodos = await _context.MetodosPago.ToListAsync();
            return new XElement("MetodosPago",
                metodos.Select(m => new XElement("MetodoPago",
                    new XElement("Id", m.Id),
                    new XElement("Nombre", m.Nombre),
                    new XElement("Descripcion", m.Descripcion)
                ))
            );
        }

        private async Task<XElement> GenerateRegimenesXmlAsync()
        {
            var regimenes = await _context.Regimenes.ToListAsync();
            return new XElement("Regimenes",
                regimenes.Select(r => new XElement("Regimen",
                    new XElement("Id", r.Id),
                    new XElement("Nombre", r.Nombre),
                    new XElement("Descripcion", r.Descripcion)
                ))
            );
        }

        private async Task<XElement> GenerateServiciosExtrasXmlAsync()
        {
            var servicios = await _context.ServiciosExtra.ToListAsync();
            return new XElement("ServiciosExtras",
                servicios.Select(s => new XElement("ServicioExtra",
                    new XElement("Id", s.Id),
                    new XElement("Nombre", s.Nombre),
                    new XElement("Descripcion", s.Descripcion),
                    new XElement("PrecioBase", s.PrecioBase)
                ))
            );
        }

        private async Task<XElement> GenerateTemporadasXmlAsync()
        {
            var temporadas = await _context.Temporadas.ToListAsync();
            return new XElement("Temporadas",
                temporadas.Select(t => new XElement("Temporada",
                    new XElement("Id", t.Id),
                    new XElement("Nombre", t.Nombre),
                    new XElement("FechaInicio", t.FechaInicio),
                    new XElement("FechaFin", t.FechaFin)
                ))
            );
        }

        private async Task<XElement> GenerateTiposHabitacionXmlAsync()
        {
            var tipos = await _context.TiposHabitacion.ToListAsync();
            return new XElement("TiposHabitacion",
                tipos.Select(t => new XElement("TipoHabitacion",
                    new XElement("Id", t.Id),
                    new XElement("Nombre", t.Nombre),
                    new XElement("Descripcion", t.Descripcion)
                ))
            );
        }

        private async Task<XElement> GenerateEstadosHabitacionXmlAsync()
        {
            var estados = await _context.EstadosHabitacion.ToListAsync();
            return new XElement("EstadosHabitacion",
                estados.Select(e => new XElement("EstadoHabitacion",
                    new XElement("Id", e.Id),
                    new XElement("Nombre", e.Nombre),
                    new XElement("Descripcion", e.Descripcion)
                ))
            );
        }

        private async Task<XElement> GenerateEstadosEstanciaXmlAsync()
        {
            var estados = await _context.EstadosEstancia.ToListAsync();
            return new XElement("EstadosEstancia",
                estados.Select(e => new XElement("EstadoEstancia",
                    new XElement("Id", e.Id),
                    new XElement("Nombre", e.Nombre),
                    new XElement("Descripcion", e.Descripcion)
                ))
            );
        }

        private async Task<XElement> GenerateEstadosReservaXmlAsync()
        {
            var estados = await _context.EstadosReserva.ToListAsync();
            return new XElement("EstadosReserva",
                estados.Select(e => new XElement("EstadoReserva",
                    new XElement("Id", e.Id),
                    new XElement("Nombre", e.Nombre),
                    new XElement("Descripcion", e.Descripcion)
                ))
            );
        }

        private Dictionary<string, List<Dictionary<string, object?>>> ConvertXmlToDictionary(XDocument doc)
        {
            var result = new Dictionary<string, List<Dictionary<string, object?>>>();

            foreach (var element in doc.Root?.Elements() ?? Enumerable.Empty<XElement>())
            {
                var tableName = element.Name.LocalName;
                var items = new List<Dictionary<string, object?>>();

                foreach (var item in element.Elements())
                {
                    var dict = new Dictionary<string, object?>();
                    foreach (var field in item.Elements())
                    {
                        var value = field.Value;
                        dict[field.Name.LocalName] = ParseValue(value);
                    }
                    items.Add(dict);
                }

                if (items.Count > 0)
                {
                    result[tableName] = items;
                }
            }

            return result;
        }

        private async Task<List<Dictionary<string, object?>>> ConvertToListAsync(XElement element, string entityName)
        {
            var items = new List<Dictionary<string, object?>>();

            foreach (var item in element.Elements())
            {
                var dict = new Dictionary<string, object?>();
                foreach (var field in item.Elements())
                {
                    var value = field.Value;
                    dict[field.Name.LocalName] = ParseValue(value);
                }
                items.Add(dict);
            }

            return items;
        }

        private object? ParseValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (int.TryParse(value, out var intValue))
                return intValue;

            if (decimal.TryParse(value, out var decimalValue))
                return decimalValue;

            if (bool.TryParse(value, out var boolValue))
                return boolValue;

            if (DateTime.TryParse(value, out var dateValue))
                return dateValue;

            return value;
        }
    }
}

