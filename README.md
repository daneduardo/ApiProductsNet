# Product API 🚀

Esta es una API RESTful desarrollada con **.NET 9** para la gestión de catálogos de productos. 
El proyecto implementa una arquitectura moderna con persistencia en **PostgreSQL**, contenedores **Docker** y despliegue automatizado en **Azure Container Apps**.

---

## 💻 Ejecución Local

### Requisitos Previos
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [Entity Framework Core Tools](https://learn.microsoft.com/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

### Pasos para iniciar
1. **Levantar Base de Datos (Docker):**
   Ejecuta el siguiente comando para iniciar una instancia de PostgreSQL compatible con la configuración de desarrollo:
   ```bash
   docker run --name pg-products -e POSTGRES_USER=devuser -e POSTGRES_PASSWORD=devpassword -e POSTGRES_DB=product_catalog -p 5432:5432 -d postgres
   ```

2. **Aplicar Migraciones:**
   Crea la estructura de tablas necesaria en tu base de datos local:
   ```bash
   dotnet ef database update
   ```

3. **Correr la API:**
   ```bash
   dotnet run
   ```
   *La API estará disponible en `https://localhost:8080`. Puedes acceder a la documentación interactiva en `https://localhost:8080/swagger`.*

---

## 🐳 Docker e Imágenes

Para construir la imagen de producción localmente y verificar el empaquetado:

```bash
# Ejecuta el siguiente comando en la raíz del proyecto para crear la imagen localmente:
docker build -t <tu-usuario-o-registro>/<nombre-de-tu-imagen>:<tag> .

# Una vez construida la imagen, inicia el contenedor mapeando el puerto necesario (en este ejemplo, el 8080):
docker run -d -p 8080:8080 --name <nombre-del-contenedor> <tu-usuario-o-registro>/<nombre-de-tu-imagen>:<tag>
```
---

## 🚀 Despliegue y CI/CD

El proyecto cuenta con un pipeline de **Azure DevOps** (`azure-pipelines.yml`) que automatiza el ciclo de vida completo:

1. **Build & Test:** Compilación de la solución y ejecución de pruebas unitarias.
2. **Push:** Creación de imagen Docker y carga a Docker Hub.
3. **Deploy:** Despliegue automático a **Azure Container Apps** mediante Azure CLI.

### Infraestructura en la Nube
* **Container App:** `products-api` (Hospedado en Azure).
* **Base de Datos:** PostgreSQL administrado en **Neon.tech**.
* **Variables de Entorno:** Gestionadas de forma segura mediante *Variable Groups* en Azure DevOps.

---

## ⚡ Ejemplos de Uso (API Endpoints)

Puedes probar los servicios utilizando **cURL** o **Postman**. Reemplaza `URL_BASE` por tu dirección local o la URL de Azure.

### 1. Listar todos los productos
```bash
curl -X GET "https://URL_BASE/api/products" -H "accept: application/json"
```

### 2. Crear un nuevo producto
```bash
curl -X POST "https://URL_BASE/api/products" \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Monitor UltraWide 34",
       "price": 450.00,
       "stock": 10,
       "description": "Monitor curvo para productividad"
     }'
```

### 3. Consultar producto por ID
```bash
curl -X GET "https://URL_BASE/api/products/1"
```

---

## 🛠️ Stack Tecnológico
* **Lenguaje:** C# (.NET 9)
* **ORM:** Entity Framework Core
* **DB:** PostgreSQL (Neon.tech / Docker)
* **DevOps:** Azure Pipelines & Azure CLI
* **Cloud:** Azure Container Apps
```
