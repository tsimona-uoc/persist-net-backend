# 📮 Postman Collection - Persist.NET Backend API

Colección completa de Postman con todos los endpoints de la API REST del sistema de gestión hotelera.

## 📋 Contenido

- **13 carpetas de API** con operaciones CRUD
- **50+ endpoints** configurados
- **Autenticación JWT** automática
- **Variables de entorno** preconfigu­radas
- **Scripts de test y validación**

## 🚀 Instalación

### Opción 1: Importar desde archivos (recomendado)

1. Abre Postman
2. Click en **Import** (arriba a la izquierda)
3. Selecciona **Upload Files**
4. Carga estos dos archivos:
   - `PersistNetBackend.postman_collection.json`
   - `PersistNetBackend.postman_environment.json`

### Opción 2: Importar desde URL

1. Click en **Import**
2. Pega esta URL (reemplaza con tu repo si aplica)
3. Espera a que se importe

## ⚙️ Configuración

### Paso 1: Seleccionar el Ambiente

En la esquina superior derecha de Postman, selecciona:
- **Environment**: `Persist.NET - Development`

### Paso 2: Verificar Variables

Ve a **Environments** → **Persist.NET - Development** y verifica:

| Variable | Valor | Descripción |
|----------|-------|-------------|
| `base_url` | `https://localhost:7170/api` | URL base de la API |
| `email` | `admin@persist.net` | Email para login |
| `password` | `Admin@123456` | Contraseña para login |
| `jwt_token` | (se auto-llena) | Token JWT (se obtiene al login) |

### Paso 3: Ajustar si es necesario

Si tu API está en otra URL/puerto, edita `base_url`:
- Local: `https://localhost:7170/api`
- Otro puerto: `https://localhost:XXXX/api`
- Remoto: `https://api.tudominio.com`

## 📲 Cómo usar

### 1️⃣ Obtener Token JWT

Antes de hacer cualquier petición:

1. Abre carpeta **Authentication**
2. Selecciona request **Login**
3. Click en **Send**
4. El token se guardará automáticamente en la variable `jwt_token`

```
Token se almacena automáticamente ✅
```

### 2️⃣ Usar cualquier endpoint

Todos los endpoints ya tienen el header `Authorization: Bearer {{jwt_token}}`:

1. Selecciona cualquier carpeta (ej: **Hoteles**)
2. Selecciona una operación (ej: **Obtener todos**)
3. Click en **Send**

El token se envía automáticamente.

### 3️⃣ Variables dinámicas

Puedes usar variables en tu ambiente:

```
{{base_url}}    → https://localhost:7170/api
{{jwt_token}}   → Token JWT actual
{{hotel_id}}    → 1
{{cliente_id}}  → 1
```

Úsalas en URLs o body:
```
GET {{base_url}}/hotel/{{hotel_id}}
```

## 📚 Estructura de API

```
Authentication
├── Register          → POST /user/register
└── Login             → POST /user/login (obtiene token)

Hoteles
├── Obtener todos     → GET /hotel
├── Obtener por ID    → GET /hotel/:id
├── Crear             → POST /hotel
├── Actualizar        → PUT /hotel/:id
└── Eliminar          → DELETE /hotel/:id

Clientes             (mismo patrón CRUD)
Habitaciones         (incluye GET por hotel)
Tipos de Habitación
Reservas             (incluye GET por cliente/habitación)
Tarifas
Temporadas
Regímenes
Métodos de Pago
Facturas             (incluye GET por cliente)
Pagos                (incluye GET por factura)
Servicios Extra
Estancias            (incluye GET por reserva)
```

## 🧪 Tests Automáticos

El endpoint **Login** tiene un script que:
- ✅ Captura el token JWT
- ✅ Lo guarda en `{{jwt_token}}`
- ✅ Valida que sea un string válido

Los demás endpoints validarán automáticamente que recibieron respuesta exitosa.

## 💾 Guardar Datos para Reutilizar

Después de crear un registro, copia el ID y úsalo:

```javascript
// En un test, puedes guardar el ID:
var jsonData = pm.response.json();
pm.environment.set("nuevo_hotel_id", jsonData.id);

// Luego úsalo:
GET {{base_url}}/hotel/{{nuevo_hotel_id}}
```

## 🔒 Seguridad

- ✅ El token JWT se almacena en variable (no visible en logs)
- ✅ Las credenciales están en el ambiente (no en git)
- ✅ HTTPS es obligatorio
- ✅ El certificado autofirmado es aceptado en desarrollo

Si necesitas cambiar credenciales:

1. Ve a **Environments** → **Persist.NET - Development**
2. Edita `email` y `password`
3. Guarda
4. El siguiente login usará las nuevas credenciales

## ⚡ Atajos útiles

| Atajo | Acción |
|-------|--------|
| `Ctrl+Shift+C` | Enviar petición |
| `Ctrl+E` | Seleccionar ambiente |
| `Ctrl+I` | Importar |
| `Ctrl+S` | Guardar |

## 🐛 Troubleshooting

### ❌ "401 Unauthorized"
- Haz login primero (Authentication → Login)
- Verifica que el ambiente está seleccionado
- Comprueba que `jwt_token` no está vacío

### ❌ "Connection Refused"
- Verifica que la API está corriendo: `dotnet run`
- Comprueba la URL en `base_url`
- Revisa el puerto (por defecto 7170)

### ❌ "SSL Certificate Error"
- Es normal en desarrollo con certificado autofirmado
- Postman lo acepta automáticamente
- En producción, usar certificado válido

### ❌ "404 Not Found"
- Verifica que el ID existe
- Comprueba el endpoint en la documentación
- Revisa la variable `base_url`

## 📝 Flujo de Ejemplo: Crear Reserva

```
1. Authorization → Login
   ↓ Obtiene token
   
2. Hoteles → Obtener todos
   ↓ Copia ID del hotel
   
3. Clientes → Crear
   ↓ Crea nuevo cliente
   ↓ Copia ID del cliente
   
4. Habitaciones → Obtener por Hotel
   ↓ Copia ID de habitación
   
5. Reservas → Crear
   ↓ Usa los IDs obtenidos
   ↓ ¡Reserva creada!
```

## 🎯 Casos de uso comunes

### Crear una reserva completa

```
POST /cliente          → Crear cliente
POST /hotel            → Crear hotel
POST /habitacion       → Usar hotel_id
POST /temporada        → Crear temporada
POST /tipohabitacion   → Crear tipo
POST /tarifa           → Usar temporada, tipo
POST /reserva          → Usar cliente, habitación
POST /factura          → Usar cliente, estancia
POST /pago             → Usar factura, método pago
```

### Consultar datos de cliente

```
GET /cliente/:id                    → Datos del cliente
GET /reserva/cliente/:clienteId     → Sus reservas
GET /factura/cliente/:clienteId     → Sus facturas
```

## 📖 Documentación API

Swagger/OpenAPI: `https://localhost:7170/swagger`

## 🤝 Support

Para problemas:
1. Verifica el endpoint en Swagger
2. Revisa los logs de la API
3. Comprueba variables de ambiente
4. Valida el JSON enviado

---

**Última actualización**: March 15, 2026
**Versión**: 1.0
**Estado**: Completo ✅
