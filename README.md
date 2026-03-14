# Persist Net Backend

Backend para la aplicación Persist construido con ASP.NET Core 9.0 y Entity Framework Core, utilizando SQL Server como base de datos.

## Descripción

Este proyecto proporciona una API REST para la gestión de usuarios y autenticación con las siguientes características:

- 🔐 **Autenticación segura** con hash SHA-256 de contraseñas
- 👤 **Gestión de usuarios** (User)
- 📋 **Sesiones de usuario** (UserSession)
- 🗄️ **Base de datos relacional** con Entity Framework Core
- 🔄 **Migraciones automáticas** de Entity Framework

## Requisitos

- .NET 9.0 SDK o superior
- SQL Server 2019 o superior
- macOS, Windows o Linux

## Instalación

### 1. Clonar el repositorio

```bash
git clone <repository-url>
cd persist-net-backend
```

### 2. Configurar variables de entorno

Crea un archivo `.env` en la raíz del proyecto (o copia el template):

```bash
cp .env.example .env
```

Edita `.env` y configura tu connection string de SQL Server:

```
CONNECTION_STRING=Server=localhost;Initial Catalog=persist_db;User ID=sa;Password=YourPassword;TrustServerCertificate=true;Encrypt=false;
```

Ajusta los valores según tu servidor SQL Server:
- `Server`: Host del servidor (ej: `localhost`, `127.0.0.1`, `persistnet.database.windows.net`)
- `Initial Catalog`: Nombre de la base de datos (se creará automáticamente si no existe)
- `User ID`: Usuario de SQL Server
- `Password`: Contraseña

> **Nota:** El archivo `.env` está excluido de git para proteger credenciales sensibles.

### 3. Restaurar dependencias

```bash
dotnet restore
```

### 4. Ejecutar la aplicación

Las migraciones se ejecutan **automáticamente** al iniciar la aplicación:

```bash
dotnet run
```

La base de datos se creará y todas las migraciones se aplicarán automáticamente. ✨

> **Nota:** Solo necesitas tener la `CONNECTION_STRING` correcta en el archivo `.env`

## Estructura del Proyecto

```
persist-net-backend/
├── Controllers/          # Controladores de la API
├── Data/                 # DbContext y migraciones
├── DTOs/                 # Data Transfer Objects
├── Models/               # Modelos de entidad (User, UserSession)
├── Repositories/         # Capa de acceso a datos
├── Services/             # Lógica de negocio (AuthService)
├── Program.cs            # Configuración de la aplicación
├── appsettings.json      # Configuración por defecto
├── .env                  # Variables de entorno (no subir a git)
└── persist-net-backend.sln
```

## Uso

### Ejecutar la aplicación

```bash
dotnet run
```

La API estará disponible en `https://localhost:5001` (o el puerto configurado).

**Las migraciones se aplican automáticamente** durante el startup. No necesitas hacer nada extra.

### Crear nuevas migraciones (desarrollo)

Usa los scripts incluidos:

**Windows:**
```powershell
.\migrate-windows.ps1 -MigrationName "AddCategory"
```

**Linux/macOS:**
```bash
./migrate.sh AddCategory
```

## Modelos Principales

### User

```csharp
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }  // SHA-256
    public string LastModifiedBy { get; set; }
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
    public string JwtToken { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime LastModifiedAt { get; set; }
}
```

## Servicios

### AuthService

Proporciona funcionalidad de autenticación:

- **LoginAsync(email, password)**: Autentica un usuario validando credenciales con SHA-256

```csharp
var (success, token) = await authService.LoginAsync("user@example.com", "password");
```

## Endpoints

### POST `/api/user/login`

Autentica un usuario y retorna un JWT token.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response (2xx):**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

## Seguridad

- Las contraseñas se almacenan como **hash SHA-256** (no en texto plano)
- Las credenciales sensibles se gestionan mediante variables de entorno (`.env`)
- Entity Framework previene SQL Injection mediante consultas parametrizadas
- Se utiliza HTTPS en producción

## Variables de Entorno

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `CONNECTION_STRING` | Cadena de conexión a SQL Server | `Server=...;Password=...` |

## Desarrollo

### Agregar un nuevo modelo

1. Crea la clase en `Models/`
2. Agrega el DbSet a `AppDbContext`
3. Crea una migración: `dotnet ef migrations add AddNewModel`
4. Ejecuta: `dotnet ef database update`

### Agregar un nuevo servicio

1. Crea la interfaz en `Services/`
2. Crea la implementación
3. Registra en `Program.cs`: `builder.Services.AddScoped<IService, Service>();`

## Troubleshooting

### Connection String no se encuentra
Asegurate de que:
- El archivo `.env` existe en la raíz del proyecto
- La variable `CONNECTION_STRING` está definida
- El servidor SQL Server es accesible

### Migraciones fallidas
```bash
# Ver estado de migraciones
dotnet ef migrations list

# Revertir última migración (cuidado)
dotnet ef database update <previous-migration-name>
```

## Autor

Temis

## Licencia

MIT

---

**Última actualización:** 14 de marzo de 2026
