# Product API 🚀

Esta es una API RESTful desarrollada con **.NET 9** para la gestión de catálogos de productos. 
El proyecto implementa una arquitectura moderna con persistencia en **PostgreSQL**, contenedores **Docker** y 
despliegue automatizado en **Azure Container Apps**.

---

## 🛠️ Stack Tecnológico
* **Lenguaje:** C# (.NET 9)
* **ORM:** Entity Framework Core
* **DB:** PostgreSQL (Neon.tech / Docker)
* **DevOps:** Azure Pipelines & Azure CLI
* **Cloud:** Azure Container Apps

---

## 💻 Ejecución Local

### Requisitos Previos
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [Entity Framework Core Tools](https://learn.microsoft.com/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

### Configuración del Entorno
Para que la aplicación funcione en su infraestructura, debe configurar las siguientes variables de entorno o actualizar el archivo `appsettings.json`:

| Variable | Descripción |
| :--- | :--- |
| `ConnectionStrings__DefaultConnection` | Cadena de conexión a su base de datos. |
| `AzureAd__ClientId` | ID de cliente para autenticación OIDC/Azure AD. |
| `AzureAd__TenantId` | ID de inquilino de su directorio de identidad. |

---

## 📦 Despliegue con Docker

La aplicación está preparada para ser ejecutada en contenedores. Siga estos pasos para usar sus propios repositorios:

### 1. Construir la Imagen
Sustituya los valores con su propio registro (Docker Hub, Azure ACR, etc.):
```bash
docker build -t <su-registro>/product-api:latest .
```

### 2. Ejecutar el Contenedor (Local)
Mapee el puerto **8080** para acceder a la API:
```bash
docker run -d \
  -p 8080:8080 \
  --name product-api-prod \
  -e ConnectionStrings__DefaultConnection="<SU_CADENA_CONEXION>" \
  <su-registro>/product-api:latest
```

---

## ☁️ Despliegue en la Nube (Azure Container Apps)

Si su infraestructura utiliza Azure, puede realizar el despliegue rápido mediante la CLI. Asegúrese de haber iniciado sesión y seleccionado su suscripción:

```powershell
az containerapp up `
  --name <nombre-app> `
  --resource-group <su-grupo-recursos> `
  --environment <su-entorno-container-apps> `
  --image <su-registro>/product-api:latest `
  --target-port 8080 `
  --ingress external `
  --env-vars "ConnectionStrings__DefaultConnection=<SU_CADENA_CONEXION>"
```

---

## 🛣️ Endpoints Principales

Una vez desplegada, la aplicación expone los siguientes puntos de acceso (asumiendo puerto 8080 en local):

* **Swagger UI:** `http://localhost:8080/swagger` (Documentación interactiva).
* **Health Check:** `http://localhost:8080/health` (Verificación de estado del sistema).
* **API Base:** `http://localhost:8080/api/products`

---

## 🧪 Pruebas Unitarias

Para ejecutar el conjunto de pruebas integradas y asegurar la calidad del código:
```bash
dotnet test
```

---

> **Nota para evaluadores:** El pipeline de CI/CD incluido en `azure-pipelines.yml` es una referencia de automatización. Para su ejecución, requiere configurar un **Variable Group** llamado `API-Secrets` que contenga la variable `Prod_Db_Connection`.

---
