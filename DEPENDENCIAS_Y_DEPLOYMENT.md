# 🔧 VERIFICAR DEPENDENCIAS PARA PRODUCCIÓN

## 📦 Paquetes NuGet requeridos

Tu proyecto ya debería tener estos. Verifica en `FormacionesApp.csproj`:

```xml
<ItemGroup>
  <!-- Entity Framework Core -->
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
  
  <!-- Autenticación -->
  <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
  
  <!-- ASP.NET Core -->
  <PackageReference Include="Microsoft.AspNetCore.Mvc" Version="8.0.0" />
  <PackageReference Include="Microsoft.AspNetCore.Authentication.Cookies" Version="8.0.0" />
  
  <!-- Otros -->
  <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" />
</ItemGroup>
```

## 🚀 Comandos antes de publicar

```bash
# 1. Verificar que compila sin errores
dotnet build

# 2. Restaurar NuGet
dotnet restore

# 3. Limpiar binarios viejos
dotnet clean

# 4. Publicar en Release
dotnet publish -c Release -o ./publish

# 5. Crear carpeta uploads si es necesario
mkdir "./publish/wwwroot/uploads/videos"
mkdir "./publish/uploads/archivos"
```

## 📋 Checklist de configuración por plataforma

### Azure
- [ ] Tengo cuenta Azure (https://azure.microsoft.com/free)
- [ ] Instalé Azure CLI
- [ ] Creé App Service Plan
- [ ] Creé App Service
- [ ] Creé SQL Server + Base de datos
- [ ] Copié cadena de conexión desde Azure Portal
- [ ] Actualicé appsettings.Production.json
- [ ] Publiqué desde Visual Studio con Azure target

### Hosting compartido .NET
- [ ] Compré hosting en Arsys/Cdmon/Hostinet
- [ ] Recibí credenciales FTP
- [ ] Recibí credenciales base de datos
- [ ] Instalé FileZilla o similar
- [ ] Publiqué en modo Release
- [ ] Subí carpeta publish vía FTP
- [ ] Creé/actualicé web.config
- [ ] Configuré cadena de conexión en panel de hosting

### VPS Linux
- [ ] Tengo VPS con Ubuntu 22.04
- [ ] Instalé .NET 8 en el VPS
- [ ] Instalé MySQL/PostgreSQL
- [ ] Instalé Nginx
- [ ] Configuré firewall (puerto 80, 443)
- [ ] Compré/transferí dominio
- [ ] Creé registros DNS apuntando a VPS
- [ ] Instalé SSL con Certbot
- [ ] Subí aplicación publicada
- [ ] Creé servicio systemd
- [ ] Probé acceso en https://midominio.com

## 🔐 Seguridad pre-producción

```csharp
// En Program.cs, ASEGÚRATE de incluir:

// 1. HTTPS redirection
app.UseHsts();
app.UseHttpsRedirection();

// 2. CORS si es necesario
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// 3. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 4. Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    await next();
});
```

## 💾 Backup automático

**Azure SQL:**
```
Portal → SQL databases → tu-base-datos → Backups
Retención automática: 7 días (básico) o 35 días (Business Critical)
```

**Hosting compartido:**
```
Generalmente incluido en panel de control
Verificar en: Backups → Restaurar puntos
```

**VPS Linux:**
```bash
# Script diario de backup (crontab)
# Edita: crontab -e
0 2 * * * mysqldump -u usuario -pcontraseña BaseDatos > /backups/backup-$(date +\%Y\%m\%d).sql
```

## 📧 Variables de entorno en producción

### Azure App Service
```
Settings → Configuration → Application settings

ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://0.0.0.0:80
WEBSITE_ENABLE_SYNC_UPDATE_SITE = true
```

### Hosting compartido
```
Panel de control → Variables de entorno

ASPNETCORE_ENVIRONMENT = Production
```

### VPS Linux
```bash
# En /etc/systemd/system/formaciones.service

Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000
```

## 🧪 Test post-despliegue

```bash
# Comprobar que la app responde
curl https://tu-dominio.com

# Comprobar SSL válido
curl -vI https://tu-dominio.com | grep "SSL certificate"

# Test de base de datos
# Ir a https://tu-dominio.com/admin
# Intentar crear usuario nuevo
# Verificar en base de datos que se creó
```

## 🆘 Errores más comunes en producción

| Error | Causa | Solución |
|-------|-------|----------|
| 500 Internal Server Error | Excepción en código | Ver logs en servidor |
| 403 Forbidden | Permisos carpeta uploads | `chmod 755 wwwroot/uploads` |
| 502 Bad Gateway | Aplicación no está corriendo | Reiniciar servicio |
| SSL certificate error | Certificado expirado/inválido | Renovar con Certbot o renovador automático |
| Connection timeout | Firewall bloquea puerto | Abrir puerto en firewall |
| No puedo subir archivos | Falta carpeta uploads | Crear manualmente en servidor |

---

**¿Cuál plataforma pref prefieres? Te ayudo a configurar paso a paso.**

