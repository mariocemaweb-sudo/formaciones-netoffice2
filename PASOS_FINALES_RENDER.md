# 🎯 PRÓXIMOS PASOS: SUBIR A GITHUB Y DESPLEGAR EN RENDER

## ✅ Cambios completados:

- ✅ Instalado Npgsql.EntityFrameworkCore.PostgreSQL 8.0.0
- ✅ Program.cs actualizado para soportar PostgreSQL en producción
- ✅ appsettings.Production.json configurado para PostgreSQL
- ✅ Proyecto compila sin errores

## 📝 PASO 1: Subir código a GitHub (5 minutos)

Abre PowerShell en la carpeta del proyecto y ejecuta:

```powershell
cd "c:\Users\mario\Desktop\Netoffice Training Center"

# Si es la primera vez:
git init
git add .
git commit -m "Initial commit - NetOffice Formaciones con soporte PostgreSQL"

# Reemplaza TU_USUARIO y TU_REPO:
git remote add origin https://github.com/TU_USUARIO/TU_REPO.git
git branch -M main
git push -u origin main

# Ingresar credenciales GitHub cuando se pida
```

## 🚀 PASO 2: Crear cuenta Render (2 minutos)

1. Ve a https://render.com
2. Click **Sign up**
3. Selecciona **GitHub** para registrarte
4. Autoriza Render a acceder a tu GitHub

## 🎯 PASO 3: Desplegar en Render (10 minutos)

1. Dashboard Render → **+ New +** → **Web Service**
2. Selecciona repositorio: `netoffice-formaciones` (o tu nombre)
3. Click **Connect**

**Configuración:**
- **Name:** `netoffice-app` (aparecerá como `netoffice-app.onrender.com`)
- **Environment:** `Dotnet`
- **Region:** `Frankfurt` (Europa)
- **Build Command:** `dotnet publish -c Release -o /app/build`
- **Start Command:** `dotnet /app/build/FormacionesApp.dll`
- **Instance Type:** `Free`

4. Click **Create Web Service**
5. Espera 3-5 minutos a que compile y despliegue...

## 🗄️ PASO 4: Crear base de datos PostgreSQL (3 minutos)

1. Dashboard Render → **+ New +** → **PostgreSQL**
2. **Name:** `netoffice-db`
3. **Region:** Frankfurt (mismo que la app)
4. **Tier:** Free
5. Click **Create Database**

Espera 1-2 minutos...

## 🔗 PASO 5: Conectar app con base de datos (3 minutos)

### 5.1 Copiar URL de la base de datos

1. Dashboard → **netoffice-db** (tu base de datos)
2. Copia la **Internal Database URL** (empieza con `postgresql://`)

### 5.2 Configurar variable de entorno

1. Dashboard → **netoffice-app** (tu web service)
2. **Environment** (abajo a la izquierda)
3. **New Environment Variable**

**Nombre:** `DATABASE_URL`
**Valor:** (pega la URL que copiaste)

4. Click **Save**
5. La app redeploy automáticamente (2-3 minutos)

## ✅ PASO 6: Test final

1. Ve a: https://netoffice-app.onrender.com (o tu nombre)
2. Deberías ver la pantalla de login
3. Ingresa credenciales:
   - **Email:** `admin@formaciones.com`
   - **Password:** `Admin123!`
4. Prueba crear un usuario nuevo
5. Prueba subir un archivo/video

---

## 🚨 SI ALGO FALLA:

**Error: "Cannot create database"**
- Espera 5 minutos más (se está creando)
- Refresca la página

**Error: "Connection timeout"**
- Verifica que `DATABASE_URL` sea correcta
- Vuelve a copiar desde la BD en Render

**Error: "Build failed"**
- Ve a **Logs** en Render
- Busca el error (generalmente falta una librería)
- Contacta si no entiendes el error

**App tarda en cargar (primer acceso)**
- Es normal en plan Free (se reinicia a los 15 min inactividad)
- Segundo acceso será rápido

---

## 📊 INFORMACIÓN FINAL:

**Tu app estará en:**
- URL: `https://netoffice-app.onrender.com`
- Base de datos: PostgreSQL en Render
- Dominio personalizado: Disponible si compras dominio

**Límites del plan Free:**
- RAM: 512 MB
- CPU: 0.5 vCPU
- BD: 256 MB
- Sleep después 15 min inactividad (gratis pero lento al reiniciar)

**Para upgradear después:**
- Web Service: $7/mes → mejor rendimiento
- DB: $15/mes → producción real
- Dominio personalizado: $3.50/mes

---

**¿Necesitas ayuda? Preguntame en qué paso te quedaste.**

