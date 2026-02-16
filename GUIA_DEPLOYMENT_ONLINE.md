# 🚀 GUÍA COMPLETA: DESPLEGAR ONLINE TU APLICACIÓN

## Tabla de Contenidos
1. [Opciones de Hosting](#opciones-de-hosting)
2. [Cambios en la Aplicación](#cambios-en-la-aplicación)
3. [Opción 1: Azure (Recomendado)](#opción-1-azure)
4. [Opción 2: Hosting Compartido .NET](#opción-2-hosting-compartido)
5. [Opción 3: VPS (Control Total)](#opción-3-vps)
6. [Paso a Paso: Azure](#paso-a-paso-azure)

---

## 🏢 OPCIONES DE HOSTING

| Proveedor | Precio | Base Datos | SSL | Dominio | Dificultad |
|-----------|--------|-----------|-----|---------|-----------|
| **Azure** | $10-50/mes | SQL Server incluida | ✅ Incluido | Sí | Media |
| **Hosting .NET** | $5-30/mes | SQL Server | ✅ Incluido | Sí | Baja |
| **AWS** | Variable | RDS | ✅ Incluido | Sí | Alta |
| **DigitalOcean** | $5-20/mes | MySQL/PostgreSQL | ✅ Incluido | Sí | Media |
| **Heroku** | Desde gratis | PostgreSQL | ✅ Incluido | Sí | Baja |
| **VPS (Linode)** | $5-20/mes | Cualquiera | ✅ Incluido | Sí | Alta |

---

## 🔄 CAMBIOS EN LA APLICACIÓN

### Paso 1: Actualizar cadena de conexión

En `appsettings.json`, prepara dos configuraciones:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=FormacionesApp;Trusted_Connection=true;TrustServerCertificate=true;",
    "AzureConnection": "Server=tcp:servidor.database.windows.net,1433;Initial Catalog=FormacionesApp;Persist Security Info=False;User ID=usuario;Password=contraseña;Encrypt=True;Connection Timeout=30;",
    "SQLiteConnection": "Data Source=formaciones.db"
  },
  "Environment": "Development"
}
```

### Paso 2: Actualizar Program.cs para soportar SQL Server

```csharp
// Cambiar esta línea:
// options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))

// Por:
var environment = builder.Environment.IsProduction() ? "AzureConnection" : "DefaultConnection";
var connectionString = builder.Configuration.GetConnectionString(environment);

if (builder.Environment.IsProduction())
{
    // Usar SQL Server en producción
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    // Usar SQLite en desarrollo
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));
}
```

### Paso 3: Configurar variables de entorno en appsettings.Production.json

Crear archivo `appsettings.Production.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:{servidor}.database.windows.net,1433;Initial Catalog={base_datos};Persist Security Info=False;User ID={usuario};Password={contraseña};Encrypt=True;Connection Timeout=30;"
  },
  "AllowedHosts": "*"
}
```

---

## 🔵 OPCIÓN 1: AZURE (Recomendado)

**Ventajas:**
- ✅ Gratuito durante 12 meses
- ✅ SQL Server incluida
- ✅ SSL automático
- ✅ Escalable
- ✅ Integración con Visual Studio
- ✅ Dominio personalizado

**Desventajas:**
- Precio puede aumentar después del período gratuito
- Requiere tarjeta de crédito

### Paso A Paso:

#### 1. Crear cuenta en Azure
1. Ir a https://azure.microsoft.com/es-es/free/
2. Crear una cuenta (te dan $200 crédito gratis)
3. Verificar teléfono e identidad

#### 2. Preparar la aplicación

En la carpeta del proyecto:
```bash
dotnet publish -c Release -o ./publish
```

#### 3. Crear grupo de recursos en Azure
```bash
# Instalar Azure CLI desde: https://aka.ms/installazurecliwindows

az login

# Crear grupo de recursos
az group create --name FormacionesRG --location westeurope

# Crear plan de App Service
az appservice plan create --name FormacionesPlan --resource-group FormacionesRG --sku B1 --is-linux

# Crear Web App
az webapp create --resource-group FormacionesRG --plan FormacionesPlan --name formaciones-app --runtime "DOTNET|8.0"
```

#### 4. Crear Base de Datos SQL en Azure
```bash
# Crear servidor SQLServer
az sql server create \
  --name formaciones-server \
  --resource-group FormacionesRG \
  --location westeurope \
  --admin-user adminuser \
  --admin-password "SecurePassword123!"

# Crear base de datos
az sql db create \
  --resource-group FormacionesRG \
  --server formaciones-server \
  --name FormacionesApp \
  --edition Basic
```

#### 5. Configurar firewall
```bash
# Permitir acceso desde aplicación Azure
az sql server firewall-rule create \
  --resource-group FormacionesRG \
  --server formaciones-server \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

#### 6. Publicar desde Visual Studio

En Visual Studio:
1. Click derecho en proyecto → **Publicar**
2. Seleccionar **Azure**
3. Seleccionar **Azure App Service (Windows)**
4. Crear nuevo → Llenar datos:
   - App name: `formaciones-app`
   - Suscripción: Tu suscripción
   - Grupo de recursos: `FormacionesRG`
   - Plan de hospedaje: `FormacionesPlan`
5. Click **Crear**
6. Click **Siguiente** y luego **Publicar**

#### 7. Actualizar cadena de conexión en Azure

En Azure Portal:
1. Ir a tu Web App
2. **Settings** → **Configuration**
3. Agregar nueva variable `ConnectionStrings__DefaultConnection`:
```
Server=tcp:formaciones-server.database.windows.net,1433;Initial Catalog=FormacionesApp;Persist Security Info=False;User ID=adminuser;Password=SecurePassword123!;Encrypt=True;Connection Timeout=30;
```

#### 8. Vincular dominio personalizado

En Azure Portal → Web App → **Custom domains**:
1. Agregar dominio
2. Validar con DNS
3. Configurar certificado SSL (automático)

---

## 🌐 OPCIÓN 2: HOSTING COMPARTIDO .NET

**Recomendado en España:**
- **Arsys**: https://www.arsys.es
- **Cdmon**: https://www.cdmon.com
- **Hostinet**: https://www.hostinet.com
- **Strato**: https://www.strato.es

### Paso A Paso (Ejemplo Arsys):

#### 1. Comprar hosting
- Ir a Arsys.es
- Buscar "ASP.NET Core hosting"
- Elegir plan con SQL Server
- Comprar dominio o usar uno existente

#### 2. Publicar aplicación

En Visual Studio:
1. Click derecho → **Publicar**
2. Seleccionar **FTP**
3. Llenar datos:
   - Servidor: `ftp.tudominio.com`
   - Usuario: Proporcionado por hosting
   - Contraseña: Proporcionada por hosting

#### 3. Subir carpeta "publish"
```bash
dotnet publish -c Release -o ./publish
```

Subir contenido de la carpeta `publish` vía FTP a `httpdocs/`

#### 4. Configurar web.config

El hosting proporciona un `web.config`. Actualizar:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=servidor;Database=basedatos;User ID=usuario;Password=contraseña;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>
  
  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="dotnet" arguments=".\FormacionesApp.dll" stdoutLogEnabled="true" stdoutLogFile=".\logs\stdout" forwardWindowsAuthToken="false" />
  </system.webServer>
</configuration>
```

#### 5. Mi SQL Server recibida

El hosting te proporciona los datos:
- **Servidor**: servidor.base.datos.com
- **Base de datos**: BaseDatos_nombreusuario
- **Usuario**: nombreusuario_admin
- **Contraseña**: (proporcionada)

---

## 💻 OPCIÓN 3: VPS (CONTROL TOTAL)

**Proveedores recomendados:**
- **Linode**: https://www.linode.com
- **DigitalOcean**: https://www.digitalocean.com
- **Vultr**: https://www.vultr.com
- **Hetzner**: https://www.hetzner.com

### Paso A Paso (Ubuntu 22.04):

#### 1. Acceder a VPS
```bash
ssh root@tu-ip-servidor
```

#### 2. Instalar .NET 8 Runtime

```bash
# Instalar dependencias
apt-get update
apt-get install -y gcc wget curl

# Agregar repositorio Microsoft
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb

# Instalar .NET
apt-get update
apt-get install -y dotnet-sdk-8.0
```

#### 3. Instalar SQL Server (Opcional - si quieres en VPS)

```bash
# Opción 1: MySQL (más ligero)
apt-get install -y mysql-server

# Opción 2: PostgreSQL
apt-get install -y postgresql postgresql-contrib
```

#### 4. Crear usuario para la aplicación

```bash
useradd -m -s /bin/bash aplicacion
```

#### 5. Subir aplicación

```bash
# En tu máquina
dotnet publish -c Release -o ./publish

# Subir a servidor
scp -r ./publish/* root@tu-ip:/home/aplicacion/app/
```

#### 6. Instalar Nginx (Reverse Proxy)

```bash
apt-get install -y nginx

# Crear configuración
nano /etc/nginx/sites-available/default
```

Agregar:
```nginx
server {
    listen 80;
    server_name tudominio.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

```bash
systemctl restart nginx
```

#### 7. Instalar Certbot (SSL Gratis)

```bash
apt-get install -y certbot python3-certbot-nginx

certbot --nginx -d tudominio.com
```

#### 8. Crear servicio systemd

Crear archivo `/etc/systemd/system/formaciones.service`:

```ini
[Unit]
Description=Formaciones App
After=network.target

[Service]
Type=notify
User=aplicacion
WorkingDirectory=/home/aplicacion/app
ExecStart=/usr/bin/dotnet /home/aplicacion/app/FormacionesApp.dll
Restart=on-failure
RestartSec=10

[Install]
WantedBy=multi-user.target
```

```bash
systemctl daemon-reload
systemctl enable formaciones
systemctl start formaciones
```

---

## 🎯 PASO A PASO: AZURE COMPLETO

### Resumen rápido:

1. **Crear cuenta**: https://azure.microsoft.com/es-es/free/
2. **Instalar Azure CLI**: https://aka.ms/installazurecliwindows
3. **Preparar código**:
```bash
# Actualizar appsettings.json
# Crear appsettings.Production.json
dotnet publish -c Release
```

4. **Crear recursos**:
```bash
az login
az group create --name FormacionesRG --location westeurope
az appservice plan create --name FormacionesPlan --resource-group FormacionesRG --sku B1
az webapp create --resource-group FormacionesRG --plan FormacionesPlan --name formaciones-app --runtime "DOTNET|8.0"
az sql server create --name formaciones-server --resource-group FormacionesRG --admin-user adminuser --admin-password "SecurePassword123!"
az sql db create --resource-group FormacionesRG --server formaciones-server --name FormacionesApp
```

5. **Publicar desde Visual Studio**:
   - Archivo → Publicar
   - Seleccionar Azure App Service
   - Completar datos y publicar

6. **Configurar base de datos**:
   - Azure Portal → Web App → Configuration
   - Agregar cadena de conexión

7. **Dominio personalizado**:
   - Azure Portal → Web App → Custom domains
   - Agregar dominio
   - Configurar DNS en tu registrador

---

## 🔒 CONSIDERACIONES DE SEGURIDAD

### 1. Variables de entorno
**NUNCA** pongas contraseñas en code. Usa:

```csharp
var sqlPassword = Environment.GetEnvironmentVariable("SQL_PASSWORD");
var connectionString = $"...Password={sqlPassword};...";
```

### 2. HTTPS/SSL
- Azure: Automático
- Hosting: Incluido
- VPS: Certbot (gratis)

### 3. Firewall
- Permitir solo puertos 80 (HTTP) y 443 (HTTPS)
- Restringir acceso a base de datos

### 4. Backups
```bash
# Backup automático en Azure
# Scheduled backups cada 24 horas
```

---

## 📋 CHECKLIST DEPLOYMENT

- [ ] Cambiar SQLite por SQL Server
- [ ] Crear appsettings.Production.json
- [ ] Actualizar Program.cs para soportar múltiples bases de datos
- [ ] Publicar en modo Release
- [ ] Elegir proveedor de hosting
- [ ] Crear base de datos remota
- [ ] Configured cadena de conexión
- [ ] Vincular dominio personalizado
- [ ] Configurar SSL/HTTPS
- [ ] Crear backup automático
- [ ] Configurar logs
- [ ] Prueba de funcionamiento

---

## 💡 RECOMENDACIÓN

**Para empezar rápido:**
1. **Mejor inicio**: Azure (12 meses gratis)
2. **Más barato**: Hosting compartido .NET (~€5/mes)
3. **Más control**: VPS Linux (€5-20/mes)

**Para tu caso (aplicación formaciones):**
- Recomiendo **Azure** o **hosting compartido .NET**
- Presupuesto: $10-30 USD/mes después del período gratis
- Incluyendo: Dominio + Hosting + BD + SSL

---

## 🔗 RECURSOS ÚTILES

- Azure Docs: https://docs.microsoft.com/es-es/azure/
- .NET Deployment: https://docs.microsoft.com/en-us/dotnet/core/deploying
- SQL Server Connection: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/connection-strings
- Registradores de dominio:
  - GoDaddy: https://es.godaddy.com
  - Namecheap: https://www.namecheap.com
  - Arsys: https://www.arsys.es

---

**Próximos pasos:**
1. ¿Qué presupuesto tienes?
2. ¿Prefieres facilidad o control?
3. Te ayudo a implementar SQL Server en la aplicación

