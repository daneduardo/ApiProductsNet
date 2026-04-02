# Product API 🚀

Esta es una API RESTful desarrollada con **.NET 9** para la gestión de catálogos de productos. El proyecto implementa una arquitectura moderna con persistencia en **PostgreSQL**, contenedores **Docker** y despliegue automatizado en **Azure Container Apps**.

---

## 🛠️ Stack Tecnológico
* **Lenguaje:** C# (.NET 9)
* **ORM:** Entity Framework Core
* **DB:** PostgreSQL (Neon.tech / Docker / Local)
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

## 🗄️ Gestión de la Base de Datos

El proyecto utiliza **EF Core Code First**. Siga estos pasos para preparar su base de datos:

1. **Actualizar la cadena de conexión** en `appsettings.json` o mediante variables de entorno.
2. **Aplicar migraciones:** Ejecute el siguiente comando para crear las tablas en su instancia de PostgreSQL:
   ```bash
   dotnet ef database update
   ```
3. **Docker Compose (Opcional):** Si desea levantar una base de datos PostgreSQL rápidamente en local, puede usar una imagen oficial de Docker antes de correr la API.

---

## 📦 Despliegue con Docker

La aplicación está preparada para ser ejecutada en contenedores. Siga estos pasos para usar sus propios repositorios:

### 1. Construir la Imagen
```bash
docker build -t <su-registro>/product-api:latest .
```

### 2. Ejecutar el Contenedor
Mapee el puerto **8080** y pase su cadena de conexión:
```bash
docker run -d \
  -p 8080:8080 \
  --name product-api-prod \
  -e ConnectionStrings__DefaultConnection="<SU_CADENA_CONEXION>" \
  <su-registro>/product-api:latest
```

---

## ☁️ Despliegue en la Nube (Azure Container Apps)

Si su infraestructura utiliza Azure, puede realizar el despliegue rápido mediante la CLI:

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

## 🧪 Pruebas Unitarias e Integración

Para ejecutar el conjunto de pruebas y asegurar la calidad del código:
```bash
dotnet test
```

## 📝 Notas para Evaluadores

* **Infraestructura CI/CD:** El pipeline incluido en `azure-pipelines.yml` fue ejecutado utilizando un **Agente Local (Self-hosted Agent)**. Para su correcta ejecución en otros entornos, asegúrese de ajustar el nombre del `pool` y configurar un **Variable Group** llamado `API-Secrets` con la variable `Prod_Db_Connection`.
* **Alcance de Herramientas:** Siguiendo las indicaciones del proceso de entrevista, el despliegue se ha realizado utilizando exclusivamente herramientas nativas de **Azure (Azure CLI & Pipelines)**, omitiendo el uso de Terraform para priorizar la integración directa con los servicios de la plataforma.
* **Optimización de Costos (Base de Datos):** Para la persistencia de datos, se utilizó una instancia de **PostgreSQL en Neon.tech** en lugar de *Azure Database for PostgreSQL*. Esta decisión técnica se tomó para evitar costos innecesarios durante la fase de prueba/evaluación, manteniendo la compatibilidad total con el proveedor de Azure mediante cadenas de conexión estándar.
* **Migraciones:** La API espera una base de datos PostgreSQL ya existente. Las migraciones deben ser aplicadas antes del primer despliegue o mediante el comando `dotnet ef database update`.
