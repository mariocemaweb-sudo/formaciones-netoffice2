# 🚀 DESPLIEGUE GRATIS EN RENDER.COM

**Tiempo total:** 15-20 minutos | **Costo:** $0/mes | **Dominio gratis:** tuapp.onrender.com

---

## ✅ REQUISITOS PREVIOS

- [ ] Cuenta GitHub (https://github.com - es gratis)
- [ ] Tu proyecto subido a GitHub
- [ ] Cuenta Render (https://render.com - gratis)

## 📌 PASO 1: Subir proyecto a GitHub

### 1.1 Crear repositorio en GitHub

1. Ve a https://github.com/new
2. Nombre repo: `netoffice-formaciones` (o lo que prefieras)
3. Descripción: "Plataforma de formaciones NetOffice"
4. Privado o Público (elig lo que prefieras)
5. Click **Create repository**

### 1.2 Subir código desde Visual Studio

```powershell
# Abre PowerShell en la carpeta del proyecto
cd "c:\Users\mario\Desktop\Netoffice Training Center"

# Inicializar git
git init

# Agregar todos los archivos
git add .

# Hacer commit inicial
git commit -m "Initial commit - Netoffice Formaciones"

# Agregar remote (reemplaza TU_USUARIO y TU_REPO)
git remote add origin https://github.com/TU_USUARIO/TU_REPO.git

# Cambiar rama a main
git branch -M main

# Subir a GitHub
git push -u origin main

# Ingresar credenciales GitHub cuando se pida
```

**Si pide "Device flow":**
1. Abre el link que aparece en la terminal
2. Ingresa el código que te muestra
3. Autoriza en GitHub
4. Vuelve a la terminal y presiona Enter

---

## 🗄️ PASO 2: Cambiar a PostgreSQL

Por defecto usabas SQLite. Para Render necesitas PostgreSQL (gratis y mejor para producción).

### 2.1 Instalar paquete NuGet

En Visual Studio Package Manager Console:

```
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -Version 8.0.0
```

O por terminal:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### 2.2 Actualizar appsettings.Development.json

Abre el archivo `appsettings.json` (o crea otro llamado `appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=formaciones_dev;Username=postgres;Password=postgres"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### 2.3 Crear appsettings.Production.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host={HOST};Database={DATABASE};Username={USER};Password={PASSWORD};SSL Mode=Require"
  }
}
```

Render los valores de `{HOST}`, `{DATABASE}`, `{USER}`, `{PASSWORD}` automáticamente.

### 2.4 Actualizar Program.cs

Reemplaza la sección de configuración de base de datos:

**ANTES:**
```csharp
if (builder.Environment.IsProduction())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException(...);
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, sqlOptions => { ... }));
}
else
{
    var sqliteConnection = builder.Configuration.GetConnectionString("SQLiteConnection") ?? "Data Source=formaciones.db";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(sqliteConnection));
}
```

**DESPUÉS:**
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (builder.Environment.IsProduction())
{
    // PostgreSQL para producción (Render)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelaySeconds: 3);
        }));
}
else
{
    // SQLite para desarrollo local
    var sqliteConnection = "Data Source=formaciones.db";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(sqliteConnection));
}
```

### 2.5 Crear .gitignore

Crea archivo `.gitignore` en raíz del proyecto:

```
bin/
obj/
.vs/
*.user
*.suo
.vscode/
formaciones.db
appsettings.local.json
```

Luego en PowerShell:
```powershell
git add .gitignore
git add appsettings.*.json
git add Program.cs
git commit -m "Add PostgreSQL support and Render configuration"
git push
```

---

## 🎯 PASO 3: Crear servicio en Render

### 3.1 Conectar GitHub

1. Ve a https://render.com
2. Click **Sign up** (crea cuenta con GitHub)
3. Autoriza Render para acceder a tu GitHub
4. Dashboard → **+ New +** → **Web Service**

### 3.2 Conectar repositorio

1. Selecciona **GitHub**
2. Busca y selecciona `netoffice-formaciones`
3. Click **Connect**

### 3.3 Configurar servicio

**Name:** `netoffice-formaciones` (aparecerá como `netoffice-formaciones.onrender.com`)

**Environment:** `Dotnet`

**Region:** `Frankfurt (EU Central - eu-central-1)` (la más cercana a España)

**Build Command:**
```
dotnet publish -c Release -o /app/build
```

**Start Command:**
```
dotnet /app/build/FormacionesApp.dll
```

**Instance Type:** `Free` (0.5 CPU, 512MB RAM)

Scroll abajo → **Create Web Service**

Espera 3-5 minutos a que compile y despliegue...

---

## 🗄️ PASO 4: Crear base de datos PostgreSQL

### 4.1 En Render dashboard

1. **+ New +** → **PostgreSQL**
2. **Name:** `netoffice-db`
3. **PostgreSQL Version:** 15
4. **Region:** Frankfurt (mismo que la app)
5. **Tier:** Free
6. Click **Create Database**

Espera 1-2 minutos...

### 4.2 Copiar credenciales

Cuando esté lista, verás:
- **Hostname**
- **Database name**
- **Username**
- **Password**
- **Internal Database URL**

Copia: **Internal Database URL** (empieza con `postgresql://`)

---

## 🔗 PASO 5: Conectar app con base de datos

### 5.1 En el servicio Web (netoffice-formaciones)

1. Dashboard → **netoffice-formaciones** (tu web service)
2. **Environment** (abajo a la izquierda)
3. Variables de entorno:

Agrega estas variables:

**Nombre:** `DATABASE_URL`
**Valor:** (pega la URL internal que copiaste, ej: `postgresql://user:pass@hostname/dbname`)

Click **Save Changes**

⏳ Espera 2-3 minutos a que redeploy...

### 5.2 Verificar conexión

En Render dashboard:
- Debe aparecer "Live" con check verde
- Si aparece error, ve a **Logs** para ver el problema

---

## 📝 PASO 6: Tests finales

### 6.1 Acceder a tu aplicación

1. Ve a: https://netoffice-formaciones.onrender.com (o tu nombre)
2. Deberías ver la pantalla de login
3. Ingresa credenciales:
   - Email: `admin@formaciones.com`
   - Password: `Admin123!`

### 6.2 Crear usuario de prueba

1. Panel Admin → Crear Usuario
2. Email: `test@formaciones.com`
3. Password: `Test123!`
4. Empresa: NetOffice
5. Rol: Usuario

### 6.3 Probar subida de archivos

1. Crear formación con videos y archivos
2. Descargar archivo
3. Ver que funciona correctamente

---

## 🔄 PASO 7: Deploy automático en el futuro

Ahora cada vez que hagas `push` a GitHub:

```powershell
git add .
git commit -m "Tu mensaje"
git push origin main
```

Render automáticamente:
1. Detecta el cambio
2. Re-compila tu app
3. La redeploy (2-3 minutos)

---

## 🌐 DOMINIO PERSONALIZADO (Opcional)

Si quieres usar tu dominio en lugar de `.onrender.com`:

### Opción 1: Comprar dominio barato
- Namecheap: ~$2/año
- DonDominio: ~€8/año (España)
- Godaddy: ~$2/año

### Opción 2: Ya tienes dominio

1. Render dashboard → **netoffice-formaciones** → **Settings**
2. Scroll a **Domains**
3. **+ Add Custom Domain**
4. Ingresa: `tudominio.com`
5. Copia los registros DNS que aparecen
6. Vuelve a tu registrador de dominio
7. Pega los registros DNS
8. Espera 5-30 min a que propague

---

## 🆘 PROBLEMAS COMUNES

### "Deploy falló"
- Ve a **Logs** en Render
- Busca el error (generalmente conexión DB)
- Verifica que `DATABASE_URL` esté configurada correctamente

### "Página no carga"
- Espera 5 minutos (puede estar deployando)
- Recarga la página (Ctrl+F5)
- Revisa logs en Render

### "No puedo subir archivos"
- Render tiene sistema de archivos efímero (temporal)
- Para solucionar: Usar **Render Disk** (~$7/mes) o **AWS S3** (gratis primeros 12 meses)
- Por ahora, los archivos se guardan en la BD, no en el filesystem

### "Base de datos ocupa demasiado"
- Plan Free tiene límite de 256MB
- Si creces: cambiar a plan pagado ($15/mes) o usar AWS RDS

---

## 📊 LÍMITES DEL PLAN GRATUITO

| Recurso | Límite |
|---------|--------|
| Apps web | Ilimitadas |
| RAM | 512 MB |
| CPU | 0.5 vCPU |
| Almacenamiento (BD) | 256 MB |
| Inactividad | 15 min sin sleep |
| Transferencia | Ilimitada |
| Dominio | .onrender.com |

**Nota:** Si nadie accede por 15 minutos, la app se pausa. Al siguiente acceso tarda 20-30 seg en iniciar.

---

## 🎯 PRÓXIMOS PASOS opcionales

1. **Dominio personalizado:** Comprar dominio y conectar
2. **Almacenamiento:** Si necesitas guardar videos/archivos, usar AWS S3
3. **Email:** Configurar notificaciones de error
4. **Backups:** Render hace backups automáticos en SQLite (PostgreSQL manual)

---

**¿Necesitas ayuda con algún paso? Pregunta el número del paso y te ayudo.**

