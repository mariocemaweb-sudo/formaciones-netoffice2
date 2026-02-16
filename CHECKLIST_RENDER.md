# ✅ CHECKLIST RENDER.COM

## 📦 INSTALACIÓN Y CÓDIGO ✅ COMPLETADA

- [x] Instalar Npgsql.EntityFrameworkCore.PostgreSQL
- [x] Actualizar Program.cs para PostgreSQL
- [x] Crear appsettings.Production.json
- [x] Compilar proyecto sin errores
- [x] .gitignore configurado

**Estado:** Todo listo para GitHub

---

## 📤 GITHUB (PRÓXIMO)

- [ ] Crear repositorio en https://github.com/new
- [ ] `git init` en carpeta del proyecto
- [ ] `git add .` todos los archivos
- [ ] `git commit -m "Initial commit"`
- [ ] `git remote add origin https://github.com/TU_USUARIO/TU_REPO.git`
- [ ] `git push -u origin main`

**Tiempo aproximado:** 5 minutos

---

## 🚀 RENDER WEB SERVICE

- [ ] Crear cuenta https://render.com (con GitHub)
- [ ] Crear nuevo **Web Service**
- [ ] Conectar repositorio GitHub
- [ ] Nombre: `netoffice-app`
- [ ] Environment: Dotnet
- [ ] Region: Frankfurt
- [ ] Build: `dotnet publish -c Release -o /app/build`
- [ ] Start: `dotnet /app/build/FormacionesApp.dll`
- [ ] Type: Free
- [ ] Esperar a que termine el deploy (3-5 min)

**URL resultado:** https://netoffice-app.onrender.com

**Tiempo aproximado:** 10 minutos (+ 3-5 espera)

---

## 🗄️ RENDER DATABASE

- [ ] En Render → **+ New +** → **PostgreSQL**
- [ ] Name: `netoffice-db`
- [ ] Region: Frankfurt
- [ ] Tier: Free
- [ ] Crear base de datos
- [ ] Copiar **Internal Database URL**

**Tiempo aproximado:** 5 minutos (+ 1-2 espera)

---

## 🔗 CONECTAR APP CON BD

- [ ] En tu Web Service → **Environment**
- [ ] Nueva variable: `DATABASE_URL`
- [ ] Pegar la URL de la BD
- [ ] Save
- [ ] Esperar redeploy (2-3 min)

**Tiempo aproximado:** 3 minutos

---

## 🧪 TEST FINAL

- [ ] Ir a https://netoffice-app.onrender.com
- [ ] Ver pantalla de login
- [ ] Entrar con `admin@formaciones.com` / `Admin123!`
- [ ] Ver menu de admin
- [ ] Crear usuario de prueba
- [ ] Crear formación de prueba
- [ ] Subir video/archivo
- [ ] Descargar el archivo

**Tiempo aproximado:** 5 minutos

---

## 📊 RESUMEN TIEMPOS

| Tarea | Tiempo | Esperado |
|-------|--------|----------|
| GitHub | 5 min | 5 min |
| Render WS | 10 min | 3-5 min |
| Render DB | 5 min | 1-2 min |
| Conectar | 3 min | 2-3 min |
| Test | 5 min | - |
| **TOTAL** | **28 min** | **11-15 min** |

---

## 💡 COMANDOS RÁPIDOS

**Para subir a GitHub:**
```powershell
cd "c:\Users\mario\Desktop\Netoffice Training Center"
git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/TU_USUARIO/netoffice-formaciones.git
git branch -M main
git push -u origin main
```

**Para compilar localmente:**
```powershell
dotnet build
dotnet run
```

**Para publicar localmente:**
```powershell
dotnet publish -c Release -o ./publish
```

---

## 🔑 CREDENCIALES DE TEST

**Admin:**
- Email: `admin@formaciones.com`
- Password: `Admin123!`

**Usuario:**
- Email: `usuario@formaciones.com`
- Password: `Usuario123!`

---

## 🚨 ERRORES COMUNES Y SOLUCIONES

| Error | Solución |
|-------|----------|
| "Build failed" | Ver logs en Render, probablemente falta compilar |
| "Connection timeout BD" | Esperar unos minutos, volver a intentar |
| "Page not found" | Aún está deployando, esperar 5 min |
| "Cannot access files" | Los archivos se guardan en BD, no en filesystem |

---

## 📱 DESPUÉS DE DESPLEGAR

**Opcional - Dominio personalizado:**
1. Comprar dominio (Namecheap, DonDominio, etc)
2. En Render → **Custom domains**
3. Agregar DNS records
4. Esperar 5-30 min

**Opcional - Almacenamiento de archivos:**
- Render tiene filesystem efímero (se borra cada redeploy)
- Para archivos permanentes: agregar Render Disk ($7/mes) o usar AWS S3

**Monitoreo:**
- Render te notifica de errores por email
- Ver logs en dashboard para debugging

---

## ✨ RESULTADO FINAL

**Plataforma en línea 100% gratuita:**
- ✅ App ASP.NET Core corriendo
- ✅ Base de datos PostgreSQL real
- ✅ Dominio .onrender.com gratuito
- ✅ HTTPS automático
- ✅ Deploy automático desde GitHub
- ✅ 24/7 online

**Costo mensual:** $0
**Limitación:** Sleep después de 15 min inactividad (plan Free)

---

¿Listo para empezar? Comienza por GitHub! 🚀

