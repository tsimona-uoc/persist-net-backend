# Persist.NET Backend - Sistema de Gestión Hotelera

API REST completa para la gestión integral de un sistema hotelero, construido con **ASP.NET Core 10.0** y **Entity Framework Core 9.0**, utilizando **SQL Server** como base de datos.

## 📋 Descripción

Sistema horizontal completo para gestión de hoteles con:

- 🔐 **Autenticación JWT** con Bearer token (JwtBearer)
- 🏨 **13 Controladores CRUD** para todas las entidades hoteleras
- 👤 **Gestión de usuarios** con roles (ADMIN, USER, etc.)
- 📍 **Sesiones con seguimiento de IP** y fecha de expiración real
- 🛡️ **Autorización granular** - Solo login es público, register requiere token
- 📚 **Arquitectura 3 capas**: Controllers → Services → Repositories → DbContext
- ⚙️ **14 Repositorios** + **14 Servicios** para acceso y lógica de datos
- 🗄️ **Migraciones automáticas** al iniciar la aplicación
- 📮 **Colección de Postman** lista con todos los endpoints

## 🎯 Características Principales

### Entidades Hoteleras
- **Hoteles**: Gestión completa de propiedades hoteleras
- **Habitaciones**: Rooms con tipos y estados
- **Reservas**: Bookings de clientes con fechas
- **Clientes**: Información de huéspedes
- **Tarifas**: Pricing dinámico por temporada, tipo y régimen
- **Temporadas**: Períodos de precios especiales
- **Facturas**: Invoices con seguimiento de pagos
- **Pagos**: Métodos de pago y transacciones
- **Servicios Extra**: Amenities adicionales (spa, etc.)
- **Estancias**: Registro de ocupación real
- **Regímenes**: Planes de comida (AD, MP, PC, TI)
- **Métodos de Pago**: Tarjeta, efectivo, transferencia, etc.

### Autenticación
- User Registration (público - sin token)
- User Login (público - sin token, retorna JWT)
- Sessions automáticas con IP y expiración real
- Token JWT con expiración configurable

## 📋 Requisitos

- **.NET 10.0 SDK** o superior
- **SQL Server 2019** o superior
- **Visual Studio Code** o **Visual Studio 2022**
- **Postman** (opcional, para testing)

## 🚀 Inicio Rápido

### 1. Clonar Repositorio

```bash
git clone <repository-url>
cd persist-net-backend
```

### 2. Configurar Base de Datos

Edita `appsettings.json` y configura tu connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Initial Catalog=persist_db;User ID=sa;Password=YourPassword;TrustServerCertificate=true;Encrypt=false;"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-with-min-32-chars-length",
    "Issuer": "persist-net-backend",
    "Audience": "persist-net-users",
    "ExpirationMinutes": 60
  }
}
```

**O usa variables de entorno:**

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;..."
$env:JWT_SECRET_KEY = "your-super-secret-key-min-32-chars"
$env:JWT_ISSUER = "persist-net-backend"
$env:JWT_AUDIENCE = "persist-net-users"
$env:JWT_EXPIRATION_MINUTES = "60"
```

### 3. Restaurar Dependencias

```bash
dotnet restore
```

### 4. Ejecutar la Aplicación

Las migraciones se aplican **automáticamente** al iniciar:

```bash
dotnet run
```

✅ **API disponible en**: `https://0.0.0.0:7151` (escucha en todos los interfaces)

#### Acceso Local vs Remoto

**Desde la misma máquina:**
```
https://localhost:7151/api
```

**Desde otra máquina en la red:**
```
https://192.168.x.x:7151/api
```

Obtén tu IP local con:
```powershell
ipconfig  # Windows
ifconfig  # Linux/Mac
```

## 📁 Estructura del Proyecto

```
persist-net-backend/
├── Controllers/              # 13 controladores CRUD + UserController
├── Data/
│   ├── AppDbContext.cs       # DbContext con 19+ DbSets
│   └── Migrations/           # EF Core migrations
├── DTOs/                     # LoginRequest, RegisterRequest
├── Models/                   # 19 modelos de entidad
├── Repositories/             # 14 interfaces + 14 implementaciones
├── Services/                 # 14 interfaces + 14 implementaciones + JwtTokenService, AuthService
├── Properties/
│   └── launchSettings.json   # Escucha en 0.0.0.0:7151
├── Postman/                  # Colección y environment JSON + README
├── Program.cs                # DI, Autenticación, Migraciones automáticas
├── appsettings.json          # Configuración por defecto
├── appsettings.Development.json
├── persist-net-backend.csproj
└── README.md
```

## 📚 Modelos de Datos

### User (Identity)
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }  // SHA-256
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public ICollection<UserSession> Sessions { get; set; }
}
```

### UserSession
```csharp
public class UserSession
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }          // JWT actual
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }    // Expiración real del JWT
    public string IpAddress { get; set; }      // IP del cliente
    public DateTime LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }
}
```

### Hotel
```csharp
public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    // Relaciones...
}
```

### Reserva
```csharp
public class Reserva
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int HabitacionId { get; set; }
    public DateTime FechaEntrada { get; set; }
    public DateTime FechaSalida { get; set; }
    public EstadoReserva Estado { get; set; }
    // Relaciones...
}
```

*Ver Models/ para más entidades*

## 🔐 Autenticación & Autorización

### Flujo de Autenticación

```
1. POST /api/user/register       (sin token - [AllowAnonymous])
   ↓
2. POST /api/user/login          (sin token - [AllowAnonymous])
   ↓ Retorna JWT Token
   
3. GET /api/hotel                (con token - [Authorize])
   Authorization: Bearer <token>
   ↓ Respuesta autorizada
```

### Configuración JWT

- **Algorithm**: HS256 (HMAC SHA-256)
- **Secret Key**: Mínimo 32 caracteres
- **Issuer**: `persist-net-backend`
- **Audience**: `persist-net-users`
- **Expiration**: Configurable (default 60 minutos)
- **Clock Skew**: 0 segundos (sin tolerancia)

### Endpoints Públicos
- `POST /api/user/login` - Sin token requerido
- `POST /api/user/register` - ⚠️ Requiere token (register no es público)

### Endpoints Protegidos
- Todos los demás endpoints requieren `Authorization: Bearer <token>`

## 📮 Postman Collection

### Uso

1. Ve a carpeta `/Postman`
2. Abre **Postman**
3. Click **Import** y selecciona:
   - `PersistNetBackend.postman_collection.json`
   - `PersistNetBackend.postman_environment.json`

4. Selecciona ambiente: **Persist.NET - Development**
5. Ve a **Authentication** → **Login** → **Send**
6. El token se guarda automáticamente en `{{jwt_token}}`

Ver [Postman/README.md](Postman/README.md) para guía completa.

## 🛠️ API Endpoints

Total: **50+ endpoints** organizados en 13 carpetas

### Autenticación
- `POST /api/user/login` - Login sin token
- `POST /api/user/register` - Register (requiere token)

### Entidades (Patrón CRUD estándar)
Para cada entidad (hotel, cliente, habitación, etc.):
- `GET /api/{entity}` - Obtener todos
- `GET /api/{entity}/{id}` - Obtener por ID
- `POST /api/{entity}` - Crear
- `PUT /api/{entity}/{id}` - Actualizar
- `DELETE /api/{entity}/{id}` - Eliminar
- `GET /api/{entity}/{filter}/...` - Filtros especiales (por hotel, cliente, etc.)

### Ejemplo: Hotels
```bash
GET    /api/hotel                  # Todos los hoteles
GET    /api/hotel/1                # Hotel 1
POST   /api/hotel                  # Crear hotel
PUT    /api/hotel/1                # Actualizar
DELETE /api/hotel/1                # Eliminar
```

## ⚙️ Configuración

### appsettings.json (Ejemplo)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Initial Catalog=persist_db;User ID=sa;Password=..."
  },
  "Jwt": {
    "SecretKey": "persist-net-super-secret-key-very-long-string",
    "Issuer": "persist-net-backend",
    "Audience": "persist-net-users",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Variables de Entorno Soportadas

| Variable | Fallback (appsettings) | Default |
|----------|----------------------|---------|
| `JWT_SECRET_KEY` | `Jwt:SecretKey` | ❌ Requerido |
| `JWT_ISSUER` | `Jwt:Issuer` | `persist-net-backend` |
| `JWT_AUDIENCE` | `Jwt:Audience` | `persist-net-users` |
| `JWT_EXPIRATION_MINUTES` | `Jwt:ExpirationMinutes` | `60` |

## 🔍 Zonas Horarias

- **DateTime.Now** se usa en toda la aplicación (hora local)
- JWT `ValidTo` se convierte a hora local con `.ToLocalTime()`
- `ExpiresAt` en sesiones = expiración real del token (no hardcoded +24h)
- Consistencia: ambas capas usan `DateTime.Now` (no UTC)

## 📝 Crear Migraciones

### Windows
```powershell
.\scripts\migrate-windows.ps1 -MigrationName "AddNewFeature"
```

### Linux/macOS
```bash
./scripts/migrate.sh AddNewFeature
```

O directamente:
```bash
dotnet ef migrations add AddNewFeature
dotnet ef database update
```

## 🔒 Seguridad

✅ **Implementado:**
- Hash SHA-256 para contraseñas (no plaintext)
- JWT con firma HMAC
- Bearer token en Authorization header
- HTTPS obligatorio (certificado autofirmado en dev)
- Entity Framework previene SQL Injection
- Autorización [Authorize] en la mayoría de endpoints
- Expiración real de tokens (no hardcoded)
- Captura de IP del cliente en sesiones

⚠️ **En Producción:**
- Cambiar secreto de JWT
- Usar certificado HTTPS válido (no autofirmado)
- Configurar CORS si es necesario
- Usar HTTPS only
- Strong password policy

## 🧪 Testing

### Flujo Completo de Ejemplo

```
1. Register nuevo usuario
   POST /api/user/register
   
2. Login para obtener token
   POST /api/user/login
   
3. Crear hotel
   POST /api/hotel
   Headers: Authorization: Bearer {token}
   
4. Crear habitación para ese hotel
   POST /api/habitacion
   
5. Crear cliente
   POST /api/cliente
   
6. Crear reserva
   POST /api/reserva (cliente + habitación)
   
7. Crear factura
   POST /api/factura
   
8. Registrar pago
   POST /api/pago
```

Ver [Postman/README.md](Postman/README.md) para casos de uso detallados.

## 🐛 Troubleshooting

### Error: "401 Unauthorized"
- Ejecutar login primero: `POST /api/user/login`
- Verificar que el token está en `Authorization: Bearer <token>`
- Verificar que el ambiente está seleccionado en Postman

### Error: "Connection String no encontrado"
```powershell
# Verificar appsettings.json
cat appsettings.json | grep ConnectionStrings

# O usar variable de entorno
$env:ConnectionStrings__DefaultConnection = "..."
```

### Error: "SSL Certificate Error"
Normal en desarrollo con certificado autofirmado.
- Postman lo acepta automáticamente
- En navegador: Advanced → Proceed anyway

### Error: "404 Not Found" en IP local
- Verificar IP con `ipconfig`
- Confirmar que el servidor está corriendo
- Usar `/api/` en la URL

### Error: "IP es ::1 en lugar de x.x.x.x"
- IPv6 localhost
- Normal en desarrollo
- Postman convierte automáticamente

## 📖 Documentación Adicional

- [Postman Collection Guide](./Postman/README.md) - Todos los endpoints
- [Swagger/OpenAPI](https://localhost:7151/swagger) - Documentación interactiva

## 📝 Release Notes

### v1.0 - March 15, 2026
✅ **Completado:**
- 13 Controladores CRUD
- 14 Repositorios + 14 Servicios
- JWT Authentication con Bearer tokens
- Autorización granular (login público, resto protegido)
- Sesiones con Ipseguimiento de IP y expiración real
- Postman Collection con 50+ endpoints
- Migraciones automáticas
- Configuración flexible (env variables + appsettings)
- Escucha en 0.0.0.0 (accesible desde red local)

## ✍️ Autor

**Temis** - UOC

## 📄 Licencia

MIT License - Ver LICENSE.txt
