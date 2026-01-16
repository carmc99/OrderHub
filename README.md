# OrderHub - Sistema de gestión de clientes y ordenes

## Descripción

OrderHub es una aplicación backend desarrollada en **.NET 6** que implementa un sistema de gestión de clientes y órdenes.

## Arquitectura del sistema

La aplicación usa una arquitectura pensada para el uso real del sistema. Se apoyan patrones conocidos, pero sin llevarlos al extremo, priorizando que el código sea claro y fácil de mantener

### Decisiones arquitectónicas

### Clean architecture

Se mantiene una separación de responsabilidades, evitando que la lógica principal dependa de detalles externos.

### Vertical Slice
El código se organiza por contextos de negocio (Customer, Order) en lugar de capas técnicas. Esto significa que todo lo relacionado con "clientes" vive junto, facilitando la comprensión y localización de cambios.

### CQRS
Las operaciones de lectura y escritura están separadas porque tienen necesidades diferentes. Los Commands manejan validaciones y lógica de negocio, mientras que las Queries están optimizadas solo para lectura.
Implementación:

DbContext para lectura (ReadDbContext con NoTracking)
Dbcontext de escritura, uno por concepto de negocio

### Estructura

Las capas de dominio e infraestructura están unificadas en OrderHub.Core y organizadas por contextos de negocio en lugar de capas técnicas.

```
┌─────────────────────────────────────────────────────────────┐
│                    OrderHub.Api (Presentation)              │
│  • CustomerController                                       │
│  • OrderController                                          │
└────────────────────┬────────────────────────────────────────┘
                     │ HTTP Requests/Responses
                     ▼
┌────────────────────────────────────────────────────────────┐
│               OrderHub.Core (Application Layer)            │
│                                                            │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Commands (Write Operations)                       │    │
│  │  • CreateCustomerCommand                           │    │
│  │  • UpdateCustomerCommand                           │    │
│  │  • DeleteCustomerCommand                           │    │
│  │  • CreateOrderCommand                              │    │
│  │  • CompleteOrderCommand                            │    │
│  │  • CancelOrderCommand                              │    │
│  └────────────────────────────────────────────────────┘    │
│                                                            │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Queries (Read Operations)                         │    │
│  │  • SearchCustomersQuery                            │    │
│  │  • SearchCustomerByIdQuery                         │    │
│  │  • SearchCustomerOrdersHistoryQuery                │    │
│  │  • SearchOrdersQuery                               │    │
│  │  • SearchOrderByIdQuery                            │    │
│  └────────────────────────────────────────────────────┘    │
│                                                            │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Domain Models                                     │    │
│  │  • CustomerModel                                   │    │
│  │  • OrderModel                                      │    │
│  │  • CustomerHistoryModel                            │    │
│  └────────────────────────────────────────────────────┘    │
│                                                            │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Validators (FluentValidation)                     │    │
│  │  • Request validation                              │    │
│  └────────────────────────────────────────────────────┘    │
└────────────────────┬───────────────────────────────────────┘
                     │ Repository Interfaces
                     ▼
┌─────────────────────────────────────────────────────────────┐
│            OrderHub.Core (Infrastructure Layer)             │
│                                                             │
│  ┌────────────────────────────────────────────────────┐     │
│  │  Repositories (EF Core)                            │     │
│  │  • CustomerRepository                              │     │
│  │  • OrderRepository                                 │     │
│  └────────────────────────────────────────────────────┘     │
│                                                             │
│  ┌────────────────────────────────────────────────────┐     │
│  │  Database Contexts                                 │     │
│  │  • CustomerDbContext                               │     │
│  │  • OrderDbContext                                  │     │
│  │  • ReadDbContext (Query context)                   │     │
│  └────────────────────────────────────────────────────┘     │
│                                                             │
│  ┌────────────────────────────────────────────────────┐     │
│  │  Entity Configurations                             │     │
│  │  • CustomerMapper                                  │     │
│  │  • OrderMapper                                     │     │
│  └────────────────────────────────────────────────────┘     │
└────────────────────┬────────────────────────────────────────┘
                     │ Entity Framework Core
                     ▼
┌─────────────────────────────────────────────────────────────┐
│              InMemory Database (Persistence)                │
│  • CustomerEntity                                           │
│  • OrderEntity                                              │
└─────────────────────────────────────────────────────────────┘
```

## Patrones y Principios Implementados

### 1. **CQRS (Command Query Responsibility Segregation)**
- **Commands**: Para operaciones de escritura (Create, Update, Delete)
- **Queries**: Para operaciones de lectura (Search, Get)
- **Separación clara**: Diferentes contextos para lectura y escritura

### 2. **Mediator Pattern (MediatR)**
- Desacoplamiento entre controllers y lógica de negocio
- Pipeline de requests con validación automática

### 3. **Repository Pattern**
- Abstracción del acceso a datos
- Interfaces en el dominio, implementación en infraestructura

### 4. **Dependency Injection**
- Inversión de control completa
- Gestión automática del ciclo de vida de dependencias

### 5. **FluentValidation**
- Validaciones declarativas
- Separación de validaciones del dominio

### 6. **Exception Handling Middleware**
- Manejo centralizado de excepciones
- Respuestas HTTP estandarizadas (RFC 7807)

## Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **.NET Core** | 6.0 | Framework principal |
| **ASP.NET Core** | 6.0 | Web API |
| **Entity Framework Core** | 6.0 | ORM para acceso a datos |
| **MediatR** | 13.0 | Implementación del patrón Mediator |
| **FluentValidation** | 11.12 | Validación de requests |
| **Swashbuckle** | 6.5 | Documentación API (Swagger) |
| **xUnit** | 2.4 | Framework de testing |
| **Moq** | 4.20 | Mocking para pruebas |

## Requisitos Previos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio Code](https://code.visualstudio.com/) o [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Git](https://git-scm.com/)

## Instalación y Ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/carmc99/OrderHub.git
cd OrderHub
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Compilar el proyecto

```bash
dotnet build
```

### 4. Ejecutar la aplicación

```bash
cd OrderHub.Api
dotnet run
```

### 5. Ejecutar pruebas unitarias

```bash
cd OrderHub.Test
dotnet test
```

## API Endpoints

### Clientes

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/v1/customers` | Crear nuevo cliente |
| `GET` | `/api/v1/customers` | Obtener todos los clientes |
| `GET` | `/api/v1/customers/{id}` | Obtener cliente por ID |
| `PUT` | `/api/v1/customers/{id}` | Actualizar cliente |
| `DELETE` | `/api/v1/customers/{id}` | Eliminar cliente |
| `GET` | `/api/v1/customers/{id}/orders` | Obtener historial de órdenes del cliente |

### Órdenes

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/v1/orders` | Crear nueva orden |
| `GET` | `/api/v1/orders` | Obtener todas las órdenes |
| `GET` | `/api/v1/orders/{id}` | Obtener orden por ID |
| `PATCH` | `/api/v1/orders/{id}/complete` | Completar orden |
| `PATCH` | `/api/v1/orders/{id}/cancel` | Cancelar orden |

## Ejemplos de Uso

### Crear un Cliente

```bash
curl -X POST "https://localhost:7075/api/v1/customers" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Juan Pérez",
    "email": "juan@example.com",
    "phoneNumber": "3001234567",
    "address": "Calle 10 #20-30"
  }'
```

**Respuesta (201 Created):**
```json
{
  "id": 1,
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "phoneNumber": "3001234567",
  "address": "Calle 10 #20-30"
}
```

### Crear una Orden

```bash
curl -X POST "https://localhost:7075/api/v1/orders" \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": 1,
    "total": 1500.00
  }'
```

**Respuesta (201 Created):**
```json
{
  "id": 1,
  "customerId": 1,
  "orderDate": "2026-01-15T10:30:00Z",
  "total": 1500.00,
  "status": "Pending",
  "completedDate": null,
  "cancelledDate": null
}
```

## Testing

El proyecto incluye pruebas unitarias completas siguiendo el patrón **Given-When-Then**:


## Manejo de Errores

La aplicación implementa un manejo centralizado de errores siguiendo el estándar **RFC 7807 (Problem Details)**:

### Ejemplo de Respuesta de Error (400 Bad Request)

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/api/v1/customers",
  "errors": {
    "Name": [
      "Name is required"
    ],
    "Email": [
      "Email is required",
      "Email format is invalid"
    ]
  }
}
```

### Ejemplo de Respuesta de Error (500 Internal Server Error)

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "/api/v1/orders"
}
```