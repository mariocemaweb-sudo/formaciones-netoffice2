# Despliegue en Railway - Guía Rápida

## 1. Requisitos Previos
- Cuenta en [railway.app](https://railway.app)
- Git instalado
- Proyecto .NET 8.0 con `web.config` configurado

## 2. Preparar el Proyecto

### Crear archivo `Procfile` en la carpeta raíz:
```
web: dotnet FormacionesApp.dll
```

### Crear archivo `.gitignore` (si no existe):
```
bin/
obj/
publish/
*.db
*.db-shm
*.db-wal
.vs/
.vscode/
appsettings.Production.json
```

## 3. Subir a GitHub

1. Initializar git en la carpeta del proyecto:
```bash
git init
git add .
git commit -m "Initial commit"
git branch -M main
```

2. Crear un repositorio en GitHub y subir:
```bash
git remote add origin https://github.com/tu-usuario/tu-repo.git
git push -u origin main
```

## 4. Conectar Railway

1. Ir a [railway.app](https://railway.app)
2. Hacer login o crear cuenta
3. Click en **New Project** → **Deploy from GitHub**
4. Conectar tu cuenta de GitHub
5. Seleccionar el repositorio `FormacionesApp`
6. Railway detectará automáticamente que es un proyecto .NET

## 5. Configurar Variables de Entorno

En la consola de Railway:

1. Ir a **Variables**
2. Agregar estas variables:

```
ASPNETCORE_ENVIRONMENT=Production
DATABASE_URL=formaciones.db
ASPNETCORE_URLS=http://+:3000
```

## 6. Configurar Base de Datos

Railway ofrece PostgreSQL incluido, pero para SQLite:

1. El archivo `formaciones.db` se creará automáticamente
2. Asegurarse que la carpeta tenga permisos de escritura

## 7. Desplegar

1. Railway desplegará automáticamente al hacer push a GitHub
2. O hacer click en **Deploy** manualmente
3. Esperar a que compile e inicie

## 8. Ver Logs y Errores

- Click en el proyecto
- Pestaña **Deployments**
- Click en el deployment más reciente
- Ver **Logs** para diagnosticar errores

## 9. Acceder a la Aplicación

1. Ir a **Settings** del proyecto
2. Copiar el dominio auto-generado
3. Acceder a: `https://tu-dominio-railway.up.railway.app`

## 10. Solución de Problemas

### Error: "Build failed"
- Verificar que `Procfile` esté en la carpeta raíz
- Verificar que el proyecto compile localmente: `dotnet build`

### Error: "502 Bad Gateway"
- Ver logs en Railway
- Verificar que `ASPNETCORE_ENVIRONMENT=Production`

### Base de datos no se crea
- Verificar permisos de la carpeta
- Ver logs del error específico

## 11. Dominio Personalizado (Opcional)

1. En **Settings** → **Domains**
2. Agregar dominio personalizado
3. Cambiar registros DNS según las instrucciones de Railway

## 12. Backup de Base de Datos

Descargar regularmente:
1. Conectarse por SSH a Railway
2. Descargar `formaciones.db`
3. Guardar en lugar seguro

---

**¡Listo!** Tu aplicación estará desplegada en Railway en unos minutos.
