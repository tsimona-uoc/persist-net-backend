using System.Xml.Linq;
using persist_net_backend.Data;
using persist_net_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace persist_net_backend.Services
{
    public class ImportService : IImportService
    {
        private readonly AppDbContext _context;

        public ImportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ImportResult> ImportFromXmlAsync(string xmlContent)
        {
            var result = new ImportResult { TableSummary = new Dictionary<string, int>() };

            try
            {
                var doc = XDocument.Parse(xmlContent);
                var root = doc.Root;

                if (root == null)
                {
                    result.Success = false;
                    result.Message = "El documento XML está vacío";
                    return result;
                }

                foreach (var tableElement in root.Elements("Table"))
                {
                    var tableName = tableElement.Attribute("Name")?.Value;
                    if (string.IsNullOrEmpty(tableName)) continue;

                    var tableResult = await ImportTableFromXmlElementAsync(tableName, tableElement);
                    result.CreatedRecords += tableResult.CreatedRecords;
                    result.UpdatedRecords += tableResult.UpdatedRecords;
                    result.FailedRecords += tableResult.FailedRecords;
                    result.Errors.AddRange(tableResult.Errors);
                    if (result.TableSummary != null)
                        result.TableSummary[tableName] = tableResult.CreatedRecords + tableResult.UpdatedRecords;
                }

                result.TotalRecords = result.CreatedRecords + result.UpdatedRecords;
                result.Success = result.FailedRecords == 0;
                result.Message = $"Importación completada. Creados: {result.CreatedRecords}, Actualizados: {result.UpdatedRecords}, Fallidos: {result.FailedRecords}";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al importar desde XML: {ex.Message}";
                result.Errors.Add(ex.Message);
            }

            return result;
        }



        public async Task<ImportResult> ImportTableFromXmlAsync(string tableName, string xmlContent)
        {
            try
            {
                var doc = XDocument.Parse(xmlContent);
                var tableElement = doc.Root;
                return tableElement == null 
                    ? new ImportResult { Success = false, Message = "El documento XML está vacío" }
                    : await ImportTableFromXmlElementAsync(tableName, tableElement);
            }
            catch (Exception ex)
            {
                return new ImportResult { Success = false, Message = $"Error al importar tabla desde XML: {ex.Message}", Errors = new() { ex.Message } };
            }
        }



        private async Task<ImportResult> ImportTableFromXmlElementAsync(string tableName, XElement tableElement)
        {
            var result = new ImportResult();

            try
            {
                foreach (var row in tableElement.Elements("Row"))
                {
                    try
                    {
                        var rowData = new Dictionary<string, object?>();
                        foreach (var column in row.Elements("Column"))
                        {
                            var columnName = column.Attribute("Name")?.Value;
                            var columnValue = column.Value;
                            if (!string.IsNullOrEmpty(columnName))
                                rowData[columnName] = string.IsNullOrEmpty(columnValue) ? null : columnValue;
                        }

                        await ImportRowAsync(tableName, rowData, result);
                    }
                    catch (Exception ex)
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error al importar fila en tabla {tableName}: {ex.Message}");
                    }
                }

                result.Success = result.FailedRecords == 0;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al procesar tabla {tableName}: {ex.Message}";
                result.Errors.Add(ex.Message);
            }

            return result;
        }



        private async Task ImportRowAsync(string tableName, Dictionary<string, object?> rowData, ImportResult result)
        {
            switch (tableName.ToLower())
            {
                case "cliente" or "clientes":
                    await ImportClienteAsync(rowData, result);
                    break;
                case "hotel" or "hoteles":
                    await ImportHotelAsync(rowData, result);
                    break;
                case "user" or "users":
                    await ImportUserAsync(rowData, result);
                    break;
                case "habitacion" or "habitaciones":
                    await ImportHabitacionAsync(rowData, result);
                    break;
                case "reserva" or "reservas":
                    await ImportReservaAsync(rowData, result);
                    break;
                case "estancia" or "estancias":
                    await ImportEstanciaAsync(rowData, result);
                    break;
                case "factura" or "facturas":
                    await ImportFacturaAsync(rowData, result);
                    break;
                case "pago" or "pagos":
                    await ImportPagoAsync(rowData, result);
                    break;
                case "consumoextra" or "consumosextras":
                    await ImportConsumoExtraAsync(rowData, result);
                    break;
                case "tarifa" or "tarifas":
                    await ImportTarifaAsync(rowData, result);
                    break;
                case "metodopago" or "metodospago":
                    await ImportMetodoPagoAsync(rowData, result);
                    break;
                case "regimen" or "regimenes":
                    await ImportRegimenAsync(rowData, result);
                    break;
                case "servicioextra" or "serviciosextras":
                    await ImportServicioExtraAsync(rowData, result);
                    break;
                case "temporada" or "temporadas":
                    await ImportTemporadaAsync(rowData, result);
                    break;
                case "tipohabitacion" or "tiposhabitacion":
                    await ImportTipoHabitacionAsync(rowData, result);
                    break;
                case "estadohabitacion" or "estadoshabitacion":
                    await ImportEstadoHabitacionAsync(rowData, result);
                    break;
                case "estadoestancia" or "estadosestancia":
                    await ImportEstadoEstanciaAsync(rowData, result);
                    break;
                case "estadoreserva" or "estadosreserva":
                    await ImportEstadoReservaAsync(rowData, result);
                    break;
                default:
                    result.FailedRecords++;
                    result.Errors.Add($"Tabla desconocida: {tableName}");
                    break;
            }
        }

        #region Import Methods

        private async Task ImportClienteAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Clientes.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    var nombre = GetValue(rowData, "Nombre");
                    if (nombre != null) existing.Nombre = nombre;
                    
                    var apellido = GetValue(rowData, "Apellido");
                    if (apellido != null) existing.Apellido = apellido;
                    
                    var email = GetValue(rowData, "Email");
                    if (email != null) existing.Email = email;
                    
                    var telefono = GetValue(rowData, "Telefono");
                    if (telefono != null) existing.Telefono = telefono;
                    
                    var documentacion = GetValue(rowData, "Documentacion");
                    if (documentacion != null) existing.Documentacion = documentacion;
                    
                    _context.Clientes.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    var nombre = GetValue(rowData, "Nombre");
                    var apellido = GetValue(rowData, "Apellido");
                    
                    if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Cliente ID {id}: Nombre y Apellido son requeridos");
                        return;
                    }

                    await _context.Clientes.AddAsync(new Cliente
                    {
                        Id = id,
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = GetValue(rowData, "Email") ?? "",
                        Telefono = GetValue(rowData, "Telefono") ?? "",
                        Documentacion = GetValue(rowData, "Documentacion") ?? ""
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Cliente: {ex.Message}"); }
        }

        private async Task ImportHotelAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Hotels.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    var name = GetValue(rowData, "Name");
                    if (name != null) existing.Name = name;
                    
                    var description = GetValue(rowData, "Description");
                    if (description != null) existing.Description = description;
                    
                    var address = GetValue(rowData, "Address");
                    if (address != null) existing.Address = address;
                    
                    var phone = GetValue(rowData, "PhoneNumber");
                    if (phone != null) existing.PhoneNumber = phone;
                    
                    var email = GetValue(rowData, "Email");
                    if (email != null) existing.Email = email;
                    
                    _context.Hotels.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    var name = GetValue(rowData, "Name");
                    
                    if (string.IsNullOrEmpty(name))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Hotel ID {id}: Name es requerido");
                        return;
                    }

                    await _context.Hotels.AddAsync(new Hotel
                    {
                        Id = id,
                        Name = name,
                        Description = GetValue(rowData, "Description") ?? "",
                        Address = GetValue(rowData, "Address") ?? "",
                        PhoneNumber = GetValue(rowData, "PhoneNumber") ?? "",
                        Email = GetValue(rowData, "Email") ?? ""
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Hotel: {ex.Message}"); }
        }

        private async Task ImportUserAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!Guid.TryParse(GetValue(rowData, "Id"), out Guid id)) { result.FailedRecords++; return; }

                var existing = await _context.Users.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    var name = GetValue(rowData, "Name");
                    if (name != null) existing.Name = name;
                    
                    var surname = GetValue(rowData, "Surname");
                    if (surname != null) existing.Surname = surname;
                    
                    var email = GetValue(rowData, "Email");
                    if (email != null) existing.Email = email;
                    
                    var passwordHash = GetValue(rowData, "PasswordHash");
                    if (passwordHash != null) existing.PasswordHash = passwordHash;
                    
                    if (int.TryParse(GetValue(rowData, "UserRoleId"), out int roleId))
                        existing.UserRoleId = roleId;
                    
                    _context.Users.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    var name = GetValue(rowData, "Name");
                    var surname = GetValue(rowData, "Surname");
                    var email = GetValue(rowData, "Email");
                    var passwordHash = GetValue(rowData, "PasswordHash");
                    
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || 
                        string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordHash))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en User ID {id}: Name, Surname, Email y PasswordHash son requeridos");
                        return;
                    }

                    await _context.Users.AddAsync(new User
                    {
                        Id = id,
                        Name = name,
                        Surname = surname,
                        Email = email,
                        PasswordHash = passwordHash,
                        UserRoleId = int.TryParse(GetValue(rowData, "UserRoleId"), out int rid) ? rid : null
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en User: {ex.Message}"); }
        }

        private async Task ImportHabitacionAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Habitaciones.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    if (int.TryParse(GetValue(rowData, "Planta"), out int planta)) existing.Planta = planta;
                    if (int.TryParse(GetValue(rowData, "Numero"), out int numero)) existing.Numero = numero;
                    if (int.TryParse(GetValue(rowData, "HotelId"), out int hotelId)) existing.HotelId = hotelId;
                    if (int.TryParse(GetValue(rowData, "TipoHabitacionId"), out int tipoId)) existing.TipoHabitacionId = tipoId;
                    if (int.TryParse(GetValue(rowData, "EstadoHabitacionId"), out int estadoId)) existing.EstadoHabitacionId = estadoId;
                    _context.Habitaciones.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    if (!int.TryParse(GetValue(rowData, "Planta"), out int planta) ||
                        !int.TryParse(GetValue(rowData, "Numero"), out int numero) ||
                        !int.TryParse(GetValue(rowData, "HotelId"), out int hotelId) ||
                        !int.TryParse(GetValue(rowData, "TipoHabitacionId"), out int tipoId) ||
                        !int.TryParse(GetValue(rowData, "EstadoHabitacionId"), out int estadoId))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Habitacion ID {id}: Planta, Numero, HotelId, TipoHabitacionId y EstadoHabitacionId son requeridos");
                        return;
                    }

                    await _context.Habitaciones.AddAsync(new Habitacion
                    {
                        Id = id,
                        Planta = planta,
                        Numero = numero,
                        HotelId = hotelId,
                        TipoHabitacionId = tipoId,
                        EstadoHabitacionId = estadoId
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Habitacion: {ex.Message}"); }
        }

        private async Task ImportReservaAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Reservas.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    if (int.TryParse(GetValue(rowData, "ClienteId"), out int cId)) existing.ClienteId = cId;
                    if (int.TryParse(GetValue(rowData, "HabitacionId"), out int hId)) existing.HabitacionId = hId;
                    if (DateOnly.TryParse(GetValue(rowData, "FechaEntrada"), out var fE)) existing.FechaEntrada = fE;
                    if (DateOnly.TryParse(GetValue(rowData, "FechaSalida"), out var fS)) existing.FechaSalida = fS;
                    if (int.TryParse(GetValue(rowData, "RegimenId"), out int rId)) existing.RegimenId = rId;
                    if (int.TryParse(GetValue(rowData, "EstadoReservaId"), out int eRId)) existing.EstadoReservaId = eRId;
                    if (decimal.TryParse(GetValue(rowData, "PrecioActual"), out decimal pa)) existing.PrecioActual = pa;
                    _context.Reservas.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    if (!int.TryParse(GetValue(rowData, "ClienteId"), out int cId) ||
                        !int.TryParse(GetValue(rowData, "HabitacionId"), out int hId) ||
                        !DateOnly.TryParse(GetValue(rowData, "FechaEntrada"), out var fE) ||
                        !DateOnly.TryParse(GetValue(rowData, "FechaSalida"), out var fS) ||
                        !int.TryParse(GetValue(rowData, "RegimenId"), out int rId) ||
                        !int.TryParse(GetValue(rowData, "EstadoReservaId"), out int eRId) ||
                        !decimal.TryParse(GetValue(rowData, "PrecioActual"), out decimal pa))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Reserva ID {id}: ClienteId, HabitacionId, FechaEntrada, FechaSalida, RegimenId, EstadoReservaId y PrecioActual son requeridos y deben ser válidos");
                        return;
                    }

                    await _context.Reservas.AddAsync(new Reserva
                    {
                        Id = id,
                        ClienteId = cId,
                        HabitacionId = hId,
                        FechaEntrada = fE,
                        FechaSalida = fS,
                        RegimenId = rId,
                        EstadoReservaId = eRId,
                        PrecioActual = pa
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Reserva: {ex.Message}"); }
        }

        private async Task ImportEstanciaAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Estancias.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    if (int.TryParse(GetValue(rowData, "ReservaId"), out int rId)) existing.ReservaId = rId;
                    if (DateTime.TryParse(GetValue(rowData, "FechaCheckIn"), out var fCI)) existing.FechaCheckIn = fCI;
                    if (DateTime.TryParse(GetValue(rowData, "FechaCheckOut"), out var fCO)) existing.FechaCheckOut = fCO;
                    if (int.TryParse(GetValue(rowData, "EstadoEstanciaId"), out int eeId)) existing.EstadoEstanciaId = eeId;
                    _context.Estancias.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    if (!int.TryParse(GetValue(rowData, "ReservaId"), out int rId) ||
                        !DateTime.TryParse(GetValue(rowData, "FechaCheckIn"), out var fCI) ||
                        !int.TryParse(GetValue(rowData, "EstadoEstanciaId"), out int eeId))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Estancia ID {id}: ReservaId, FechaCheckIn y EstadoEstanciaId son requeridos");
                        return;
                    }

                    await _context.Estancias.AddAsync(new Estancia
                    {
                        Id = id,
                        ReservaId = rId,
                        FechaCheckIn = fCI,
                        FechaCheckOut = DateTime.TryParse(GetValue(rowData, "FechaCheckOut"), out var fCO) ? (DateTime?)fCO : null,
                        EstadoEstanciaId = eeId
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Estancia: {ex.Message}"); }
        }

        private async Task ImportFacturaAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Facturas.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    if (int.TryParse(GetValue(rowData, "EstanciaId"), out int eId)) existing.EstanciaId = eId;
                    if (int.TryParse(GetValue(rowData, "ClienteId"), out int cId)) existing.ClienteId = cId;
                    if (decimal.TryParse(GetValue(rowData, "Total"), out decimal total)) existing.Total = total;
                    if (decimal.TryParse(GetValue(rowData, "Descuento"), out decimal desc)) existing.Descuento = desc;
                    if (DateTime.TryParse(GetValue(rowData, "FechaEmision"), out var fE)) existing.FechaEmision = fE;
                    if (bool.TryParse(GetValue(rowData, "Pagada"), out bool pagada)) existing.Pagada = pagada;
                    _context.Facturas.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    if (!int.TryParse(GetValue(rowData, "EstanciaId"), out int eId) ||
                        !int.TryParse(GetValue(rowData, "ClienteId"), out int cId) ||
                        !decimal.TryParse(GetValue(rowData, "Total"), out decimal total) ||
                        !DateTime.TryParse(GetValue(rowData, "FechaEmision"), out var fE))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Factura ID {id}: EstanciaId, ClienteId, Total y FechaEmision son requeridos");
                        return;
                    }

                    await _context.Facturas.AddAsync(new Factura
                    {
                        Id = id,
                        EstanciaId = eId,
                        ClienteId = cId,
                        Total = total,
                        Descuento = decimal.TryParse(GetValue(rowData, "Descuento"), out decimal desc) ? desc : 0,
                        FechaEmision = fE,
                        Pagada = bool.TryParse(GetValue(rowData, "Pagada"), out bool pagada) ? pagada : false
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Factura: {ex.Message}"); }
        }

        private async Task ImportPagoAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Pagos.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    if (int.TryParse(GetValue(rowData, "FacturaId"), out int fId)) existing.FacturaId = fId;
                    if (int.TryParse(GetValue(rowData, "MetodoPagoId"), out int mpId)) existing.MetodoPagoId = mpId;
                    if (decimal.TryParse(GetValue(rowData, "Importe"), out decimal imp)) existing.Importe = imp;
                    if (DateTime.TryParse(GetValue(rowData, "FechaPago"), out var fP)) existing.FechaPago = fP;
                    _context.Pagos.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    if (!int.TryParse(GetValue(rowData, "FacturaId"), out int fId) ||
                        !int.TryParse(GetValue(rowData, "MetodoPagoId"), out int mpId) ||
                        !decimal.TryParse(GetValue(rowData, "Importe"), out decimal imp) ||
                        !DateTime.TryParse(GetValue(rowData, "FechaPago"), out var fP))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Pago ID {id}: FacturaId, MetodoPagoId, Importe y FechaPago son requeridos");
                        return;
                    }

                    await _context.Pagos.AddAsync(new Pago
                    {
                        Id = id,
                        FacturaId = fId,
                        MetodoPagoId = mpId,
                        Importe = imp,
                        FechaPago = fP
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Pago: {ex.Message}"); }
        }

        private async Task ImportConsumoExtraAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.ConsumosExtra.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    if (int.TryParse(GetValue(rowData, "EstanciaId"), out int eId)) existing.EstanciaId = eId;
                    if (int.TryParse(GetValue(rowData, "ServicioExtraId"), out int sId)) existing.ServicioExtraId = sId;
                    if (int.TryParse(GetValue(rowData, "Cantidad"), out int cant)) existing.Cantidad = cant;
                    if (decimal.TryParse(GetValue(rowData, "PrecioUnitario"), out decimal pu)) existing.PrecioUnitario = pu;
                    if (DateTime.TryParse(GetValue(rowData, "Fecha"), out var f)) existing.Fecha = f;
                    _context.ConsumosExtra.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    if (!int.TryParse(GetValue(rowData, "EstanciaId"), out int eId) ||
                        !int.TryParse(GetValue(rowData, "ServicioExtraId"), out int sId) ||
                        !int.TryParse(GetValue(rowData, "Cantidad"), out int cant) ||
                        !decimal.TryParse(GetValue(rowData, "PrecioUnitario"), out decimal pu) ||
                        !DateTime.TryParse(GetValue(rowData, "Fecha"), out var f))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en ConsumoExtra ID {id}: EstanciaId, ServicioExtraId, Cantidad, PrecioUnitario y Fecha son requeridos");
                        return;
                    }

                    await _context.ConsumosExtra.AddAsync(new ConsumoExtra
                    {
                        Id = id,
                        EstanciaId = eId,
                        ServicioExtraId = sId,
                        Cantidad = cant,
                        PrecioUnitario = pu,
                        Fecha = f
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en ConsumoExtra: {ex.Message}"); }
        }

        private async Task ImportTarifaAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Tarifas.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    if (int.TryParse(GetValue(rowData, "TemporadaId"), out int tId)) existing.TemporadaId = tId;
                    if (decimal.TryParse(GetValue(rowData, "PrecioNoche"), out decimal pr)) existing.PrecioNoche = pr;
                    existing.LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import";
                    existing.LastModifiedAt = DateTime.Now;
                    _context.Tarifas.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    if (!int.TryParse(GetValue(rowData, "TemporadaId"), out int tId) ||
                        !int.TryParse(GetValue(rowData, "TipoHabitacionId"), out int thId) ||
                        !int.TryParse(GetValue(rowData, "RegimenId"), out int rId) ||
                        !decimal.TryParse(GetValue(rowData, "PrecioNoche"), out decimal pr))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Tarifa ID {id}: TemporadaId, TipoHabitacionId, RegimenId y PrecioNoche son requeridos");
                        return;
                    }

                    await _context.Tarifas.AddAsync(new Tarifa
                    {
                        Id = id,
                        TemporadaId = tId,
                        PrecioNoche = pr,
                        LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import",
                        LastModifiedAt = DateTime.Now
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Tarifa: {ex.Message}"); }
        }

        private async Task ImportMetodoPagoAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.MetodosPago.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    var nombre = GetValue(rowData, "Nombre");
                    if (nombre != null) existing.Nombre = nombre;
                    
                    var descripcion = GetValue(rowData, "Descripcion");
                    if (descripcion != null) existing.Descripcion = descripcion;
                    
                    _context.MetodosPago.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    var nombre = GetValue(rowData, "Nombre");
                    
                    if (string.IsNullOrEmpty(nombre))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en MetodoPago ID {id}: Nombre es requerido");
                        return;
                    }

                    await _context.MetodosPago.AddAsync(new MetodoPago 
                    { 
                        Id = id, 
                        Nombre = nombre, 
                        Descripcion = GetValue(rowData, "Descripcion") ?? "" 
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en MetodoPago: {ex.Message}"); }
        }

        private async Task ImportRegimenAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Regimenes.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    var nombre = GetValue(rowData, "Nombre");
                    if (nombre != null) existing.Nombre = nombre;
                    
                    var descripcion = GetValue(rowData, "Descripcion");
                    if (descripcion != null) existing.Descripcion = descripcion;
                    
                    _context.Regimenes.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    var nombre = GetValue(rowData, "Nombre");
                    
                    if (string.IsNullOrEmpty(nombre))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en Regimen ID {id}: Nombre es requerido");
                        return;
                    }

                    await _context.Regimenes.AddAsync(new Regimen 
                    { 
                        Id = id, 
                        Nombre = nombre, 
                        Descripcion = GetValue(rowData, "Descripcion") ?? "" 
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Regimen: {ex.Message}"); }
        }

        private async Task ImportServicioExtraAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.ServiciosExtra.FindAsync(id);
                if (existing != null)
                {
                    // Actualizar: solo si el campo viene informado
                    var nombre = GetValue(rowData, "Nombre");
                    if (nombre != null) existing.Nombre = nombre;
                    
                    var descripcion = GetValue(rowData, "Descripcion");
                    if (descripcion != null) existing.Descripcion = descripcion;
                    
                    if (decimal.TryParse(GetValue(rowData, "PrecioBase"), out decimal pr)) existing.PrecioBase = pr;
                    
                    existing.LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import";
                    existing.LastModifiedAt = DateTime.Now;
                    _context.ServiciosExtra.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    // Crear: validar campos requeridos
                    var nombre = GetValue(rowData, "Nombre");
                    
                    if (string.IsNullOrEmpty(nombre) || !decimal.TryParse(GetValue(rowData, "PrecioBase"), out decimal pr))
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en ServicioExtra ID {id}: Nombre y PrecioBase son requeridos");
                        return;
                    }

                    await _context.ServiciosExtra.AddAsync(new ServicioExtra
                    {
                        Id = id,
                        Nombre = nombre,
                        Descripcion = GetValue(rowData, "Descripcion") ?? "",
                        PrecioBase = pr,
                        LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import",
                        LastModifiedAt = DateTime.Now
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en ServicioExtra: {ex.Message}"); }
        }

        private async Task ImportTemporadaAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.Temporadas.FindAsync(id);
                if (existing != null)
                {
                    existing.Nombre = GetValue(rowData, "Nombre") ?? "";
                    if (DateOnly.TryParse(GetValue(rowData, "FechaInicio"), out var fi)) existing.FechaInicio = fi;
                    if (DateOnly.TryParse(GetValue(rowData, "FechaFin"), out var ff)) existing.FechaFin = ff;
                    existing.LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import";
                    existing.LastModifiedAt = DateTime.Now;
                    _context.Temporadas.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    await _context.Temporadas.AddAsync(new Temporada
                    {
                        Id = id,
                        Nombre = GetValue(rowData, "Nombre") ?? "",
                        FechaInicio = DateOnly.TryParse(GetValue(rowData, "FechaInicio"), out var fi) ? fi : DateOnly.FromDateTime(DateTime.Now),
                        FechaFin = DateOnly.TryParse(GetValue(rowData, "FechaFin"), out var ff) ? ff : DateOnly.FromDateTime(DateTime.Now),
                        LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import",
                        LastModifiedAt = DateTime.Now
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en Temporada: {ex.Message}"); }
        }

        private async Task ImportTipoHabitacionAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.TiposHabitacion.FindAsync(id);
                if (existing != null)
                {
                    existing.Nombre = GetValue(rowData, "Nombre") ?? "";
                    existing.Descripcion = GetValue(rowData, "Descripcion") ?? "";
                    existing.LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import";
                    existing.LastModifiedAt = DateTime.Now;
                    _context.TiposHabitacion.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    await _context.TiposHabitacion.AddAsync(new TipoHabitacion
                    {
                        Id = id,
                        Nombre = GetValue(rowData, "Nombre") ?? "",
                        Descripcion = GetValue(rowData, "Descripcion") ?? "",
                        LastModifiedBy = GetValue(rowData, "LastModifiedBy") ?? "Import",
                        LastModifiedAt = DateTime.Now
                    });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en TipoHabitacion: {ex.Message}"); }
        }

        private async Task ImportEstadoHabitacionAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.EstadosHabitacion.FindAsync(id);
                if (existing != null)
                {
                    existing.Nombre = GetValue(rowData, "Nombre") ?? "";
                    existing.Descripcion = GetValue(rowData, "Descripcion") ?? "";
                    _context.EstadosHabitacion.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    await _context.EstadosHabitacion.AddAsync(new EstadoHabitacion { Id = id, Nombre = GetValue(rowData, "Nombre") ?? "", Descripcion = GetValue(rowData, "Descripcion") ?? "" });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en EstadoHabitacion: {ex.Message}"); }
        }

        private async Task ImportEstadoEstanciaAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.EstadosEstancia.FindAsync(id);
                if (existing != null)
                {
                    existing.Nombre = GetValue(rowData, "Nombre") ?? "";
                    existing.Descripcion = GetValue(rowData, "Descripcion") ?? "";
                    _context.EstadosEstancia.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    await _context.EstadosEstancia.AddAsync(new EstadoEstancia { Id = id, Nombre = GetValue(rowData, "Nombre") ?? "", Descripcion = GetValue(rowData, "Descripcion") ?? "" });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en EstadoEstancia: {ex.Message}"); }
        }

        private async Task ImportEstadoReservaAsync(Dictionary<string, object?> rowData, ImportResult result)
        {
            try
            {
                if (!int.TryParse(GetValue(rowData, "Id"), out int id)) { result.FailedRecords++; return; }

                var existing = await _context.EstadosReserva.FindAsync(id);
                if (existing != null)
                {
                    existing.Nombre = GetValue(rowData, "Nombre") ?? "";
                    existing.Descripcion = GetValue(rowData, "Descripcion") ?? "";
                    _context.EstadosReserva.Update(existing);
                    result.UpdatedRecords++;
                }
                else
                {
                    await _context.EstadosReserva.AddAsync(new EstadoReserva { Id = id, Nombre = GetValue(rowData, "Nombre") ?? "", Descripcion = GetValue(rowData, "Descripcion") ?? "" });
                    result.CreatedRecords++;
                }
            }
            catch (Exception ex) { result.FailedRecords++; result.Errors.Add($"Error en EstadoReserva: {ex.Message}"); }
        }

        #endregion

        private string? GetValue(Dictionary<string, object?> data, string key)
        {
            return data.TryGetValue(key, out var value) ? value?.ToString() : null;
        }
    }
}
