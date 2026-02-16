# 📋 INSTRUCCIONES RÁPIDAS PARA CADA PLATAFORMA

## 🔵 AZURE (Recomendado - 12 meses GRATIS)

### 1. Registro y Setup Inicial (5 min)
```
1. Ir a: https://azure.microsoft.com/es-es/free/
2. Crear cuenta con email
3. Agregar método de pago (se cobra si excedes límite gratuito)
4. Esperar confirmación
```

### 2. Preparar código (10 min)
```bash
# En la carpeta del proyecto
cd "c:\Users\mario\Desktop\Netoffice Training Center"

# Publicar en modo Release
dotnet publish -c Release -o ./publish

# Verificar que se creó:
# - Si existe carpeta ./publish está listo
```

### 3. Instalar herramienta Azure CLI (5 min)
```
Descargar: https://aka.ms/installazurecliwindows
Ejecutar instalador
Reiniciar terminal
```

### 4. Crear recursos en Azure (10 min)

Abrir PowerShell y ejecutar:
```powershell
# Copiá y pega cada linea una por una

az login

# Crear grupo de recursos
az group create --name FormacionesRG --location westeurope

# Crear plan de App Service (B1 = $15 USD/mes después del período gratuito)
az appservice plan create `
  --name FormacionesPlan `
  --resource-group FormacionesRG `
  --sku B1 `
  --is-linux

# Crear App Service
az webapp create `
  --resource-group FormacionesRG `
  --plan FormacionesPlan `
  --name formaciones-app `
  --runtime "DOTNET|8.0"

# Crear servidor SQL Server
az sql server create `
  --name formaciones-server `
  --resource-group FormacionesRG `
  --location westeurope `
  --admin-user adminuser `
  --admin-password "SecurePassword123!cambio"

# Crear base de datos
az sql db create `
  --resource-group FormacionesRG `
  --server formaciones-server `
  --name FormacionesApp `
  --edition Basic `
  --max-size 2GB

# Permitir conexión desde Azure
az sql server firewall-rule create `
  --resource-group FormacionesRG `
  --server formaciones-server `
  --name AllowAzureServices `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0
```

### 5. Publicar desde Visual Studio (5 min)

1. En Visual Studio: **Archivo** → **Publicar**
2. Seleccionar **Azure**
3. Seleccionar **Azure App Service (Windows)**
4. Seleccionar tu suscripción
5. Elegir grupo de recursos: **FormacionesRG**
6. Elegir App Service: **formaciones-app**
7. Click **Siguiente**
8. Click **Publicar**

Esperar a que termine (aparecerá URL como: `https://formaciones-app.azurewebsites.net`)

### 6. Configurar base de datos (3 min)

1. Ir a Azure Portal: https://portal.azure.com
2. Buscar: **formaciones-app** (tu Web App)
3. **Settings** → **Configuration**
4. **New connection string**
5. Nombre: `DefaultConnection`
6. Valor: (Copiar de abajo y completar)
7. Tipo: **SQL Server**
8. Click **OK**
9. Click **Save** arriba

**Cadena de conexión (reemplazar valores):**
```
Server=tcp:formaciones-server.database.windows.net,1433;Initial Catalog=FormacionesApp;Persist Security Info=False;User ID=adminuser;Password=SecurePassword123!cambio;Encrypt=True;Connection Timeout=30;
```

### 7. Vincular dominio (10 min)

Opción A: Comprar dominio en Azure
1. Portal → formaciones-app → **Custom domains**
2. **+ Add custom domain**
3. Pagar anualmente

Opción B: Usar dominio existente
1. Portal → formaciones-app → **Custom domains**
2. **+ Add custom domain**
3. Verificar con DNS (seguir instrucciones)
4. Azure autoasigna certificado SSL

### URLs después de publicar:
- Sin dominio: `https://formaciones-app.azurewebsites.net`
- Con dominio: `https://tudominio.com`

---

## 🌐 HOSTING COMPARTIDO .NET (Más económico)

**Proveedores en español:**
- Arsys: https://www.arsys.es (comprar "ASP.NET Core Hosting")
- Cdmon: https://www.cdmon.com
- Hostinet: https://www.hostinet.com

### Pasos (Ejemplo Arsys):

1. **Comprar hosting ASP.NET Core** con:
   - SQL Server incluida
   - 5GB de espacio mínimo
   - Dominio incluido o personalizador

2. **Recibir datos de acceso:**
   - FTP: ftp.tudominio.com
   - Usuario FTP: tuusuario
   - Contraseña FTP: xxxxxx
   - Base de datos: BaseDatos_tuusuario
   - Usuario BD: usuario_admin
   - Contraseña BD: xxxxxx

3. **Publicar:**
```bash
dotnet publish -c Release -o ./publish
```

4. **Subir vía FTP:**
   - Programa recomendado: FileZilla (gratuito)
   - Descargar: https://filezilla-project.org
   - Conectar con datos FTP
   - Subir contenido de `./publish` a `httpdocs/`

5. **Configurar web.config:**

El hosting suele proporcionar uno. Si no, crear en `published/`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=servidor-sql.com;Database=BaseDatos_tuusuario;User ID=usuario_admin;Password=contraseña;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>

  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="dotnet" arguments=".\FormacionesApp.dll" 
                stdoutLogEnabled="true" 
                stdoutLogFile=".\logs\stdout" 
                forwardWindowsAuthToken="false" />
  </system.webServer>
</configuration>
```

6. **Actualizar appsettings.Production.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=servidor-sql.com;Database=BaseDatos_tuusuario;User ID=usuario_admin;Password=contraseña;"
  }
}
```

---

## 🐧 VPS LINUX (Control total - más técnico)

**Proveedores:**
- Linode: https://www.linode.com ($5/mes)
- DigitalOcean: https://www.digitalocean.com ($5/mes)
- Vultr: https://www.vultr.com ($2.50/mes)

### Pasos rápidos:

1. **Crear Droplet/Linode:**
   - OS: Ubuntu 22.04 LTS
   - RAM mínimo: 2GB
   - Almacenamiento: 40GB

2. **Conectar:**
```bash
ssh root@tu-ip-del-vps
```

3. **Instalar herramientas:**
```bash
# Actualizar sistema
apt-get update && apt-get upgrade -y

# Instalar .NET 8
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 8.0

# Instalar MySQL/PostgreSQL (elige uno)
apt-get install -y mysql-server    # O: postgresql postgresql-contrib
```

4. **Crear usuario para app:**
```bash
useradd -m -s /bin/bash appuser
```

5. **Subir aplicación:**
```bash
# En tu máquina local
dotnet publish -c Release -o ./publish
scp -r ./publish/* root@tu-ip:/home/appuser/app/
```

6. **Instalar Nginx:**
```bash
apt-get install -y nginx

# Editar configuración
nano /etc/nginx/sites-available/default
```

Reemplaza el contenido con:
```nginx
server {
    listen 80;
    server_name tudominio.com www.tudominio.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

```bash
systemctl restart nginx
```

7. **SSL Gratis (HTTPS):**
```bash
apt-get install -y certbot python3-certbot-nginx
certbot --nginx -d tudominio.com -d www.tudominio.com
```

8. **Crear servicio systemd:**

Crear `/etc/systemd/system/formaciones.service`:
```ini
[Unit]
Description=Formaciones Application
After=network.target

[Service]
Type=notify
User=appuser
WorkingDirectory=/home/appuser/app
ExecStart=/root/.dotnet/dotnet /home/appuser/app/FormacionesApp.dll
Restart=on-failure
RestartSec=10

Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

```bash
systemctl daemon-reload
systemctl enable formaciones
systemctl start formaciones
systemctl status formaciones
```

9. **Configurar DNS:**
- Panel del proveedor de dominio
- Crear registro A apuntando a tu IP del VPS
- Esperar 5-30 minutos para propagación

---

## 📊 TABLA COMPARATIVA

| Aspecto | Azure | Hosting .NET | VPS |
|---------|-------|-------------|-----|
| Precio/mes | $10-50 | €5-30 | €5-20 |
| Base de datos | SQL Server | SQL Server | Libre |
| SSL/HTTPS | ✅ Automático | ✅ Gratis | ✅ Certbot gratis |
| Dominio personalizado | ✅ Sí | ✅ Sí | ✅ Sí |
| Backups automáticos | ✅ Sí | ✅ Generalmente | ⚠️ Manual |
| Escalabilidad | ✅ Muy fácil | ⚠️ Limitada | ✅ Completa |
| Dificultad setup | Media | Baja | Alta |
| Mantenimiento | Mínimo | Mínimo | Máximo |
| Uptime SLA | 99.95% | 99.5% | Depende |

---

## ✅ CHECKLIST FINAL

Antes de publicar:
- [ ] Cambié SQLite por SQL Server en Program.cs
- [ ] Crée appsettings.Production.json
- [ ] Publiqué en modo Release
- [ ] Subí archivos a servidor/hosting
- [ ] Configuré cadena de conexión en servidor
- [ ] Vinculé dominio personalizado
- [ ] Habilitálices SSL/HTTPS
- [ ] Probé login con usuarios
- [ ] Probé subida y descarga de archivos
- [ ] Configuré backups automáticos

---

## 🚨 PROBLEMAS COMUNES Y SOLUCIONES

**"No puedo conectar a la base de datos"**
- Verificar cadena de conexión en appsettings.Production.json
- Comprobar firewall permite acceso
- Verificar usuario/contraseña son correctos

**"Archivos no se guardan"**
- Verificar permisos en carpeta wwwroot/uploads
- En Azure: usar Azure Storage en lugar de sistema de archivos local

**"Aplicación lenta"**
- Aumentar RAM del servidor
- Habilitar caching en Nginx
- Agregar índices en base de datos

**"SSL no funciona"**
- Esperar a que se propague DNS (5-30 min)
- Limpiar caché dns: `ipconfig /flushdns`
- Forzar HTTPS en Program.cs

---

**¿Necesitas ayuda con alguna plataforma específica?**
Puedo ayudarte paso a paso.

