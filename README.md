# Plataforma de Formaciones - Aplicación Web

Sistema completo de gestión de formaciones con videos y archivos adjuntos para clientes de Netoffice.

## 🎯 Características

- **Panel de Administración**: Gestión completa de formaciones, videos y archivos
- **Portal de Usuarios**: Acceso a formaciones y reproducción de videos
- **Autenticación**: Sistema de login con roles (Admin/Usuario)
- **Base de datos**: SQLite para fácil implementación
- **Registro de accesos**: Logs de todos los inicios de sesión
- **Almacenamiento local**: Videos y archivos en el servidor

## 📋 Requisitos Previos

1. **Windows Server** con IIS instalado
2. **.NET 8.0 Runtime** (ASP.NET Core Runtime)
   - Descargar desde: https://dotnet.microsoft.com/download/dotnet/8.0
   - Instalar: "ASP.NET Core Runtime 8.0.x - Windows Hosting Bundle"

## 🚀 Instalación en IIS

### Paso 1: Publicar la aplicación

1. Abrir una terminal en la carpeta del proyecto
2. Ejecutar:
```bash
dotnet restore
dotnet publish -c Release -o ./publish
```

### Paso 2: Configurar IIS

1. Abrir **Administrador de IIS**
2. En el servidor, hacer clic derecho en **Sitios** → **Agregar sitio web**
3. Configurar:
   - **Nombre del sitio**: FormacionesApp
   - **Ruta física**: Ruta a la carpeta `publish`
   - **Puerto**: 80 (o el que prefieras)
   - **Nombre de host** (opcional): formaciones.tudominio.com

4. Configurar **Grupo de aplicaciones**:
   - Hacer clic en **Grupos de aplicaciones**
   - Seleccionar el grupo creado para tu sitio
   - Clic derecho → **Configuración avanzada**
   - Cambiar **Versión de .NET CLR** a: **Sin código administrado**

### Paso 3: Permisos

1. En la carpeta `publish`, hacer clic derecho → **Propiedades**
2. Ir a **Seguridad** → **Editar**
3. Agregar permisos de **Modificar** para:
   - `IIS_IUSRS`
   - `IUSR`

### Paso 4: Verificar instalación

1. Navegar a: `http://localhost` (o la URL configurada)
2. Deberías ver la página de login

## 👤 Acceso Inicial

**Usuario Administrador predeterminado:**
- Email: `admin@formaciones.com`
- Contraseña: `Admin123!`

⚠️ **IMPORTANTE**: Cambia esta contraseña después del primer acceso.

## 📁 Estructura del Proyecto

```
FormacionesApp/
├── Controllers/          # Controladores MVC
│   ├── AccountController.cs    # Autenticación
│   ├── AdminController.cs      # Panel admin
│   ├── FormacionesController.cs # Vista usuarios
│   └── HomeController.cs       # Página principal
├── Models/              # Modelos de datos
├── Views/               # Vistas Razor
├── Data/                # Contexto de base de datos
├── wwwroot/             # Archivos estáticos
│   ├── css/            # Estilos
│   ├── js/             # JavaScript
│   └── uploads/        # Videos y archivos subidos
│       ├── videos/     # Videos de formaciones
│       └── archivos/   # Documentos adjuntos
├── Program.cs           # Configuración de la app
├── web.config          # Configuración IIS
└── appsettings.json    # Configuración general
```

## 💾 Base de Datos

La aplicación usa **SQLite** (archivo `formaciones.db`) que se crea automáticamente.

**Ubicación**: En la carpeta raíz de la aplicación publicada.

### Respaldo de la base de datos

Para hacer backup, simplemente copia el archivo `formaciones.db`

## 📤 Gestión de Archivos

### Videos
- **Formatos permitidos**: MP4, AVI, MOV, WMV, MKV
- **Tamaño máximo**: 500 MB
- **Ubicación**: `wwwroot/uploads/videos/`

### Archivos adjuntos
- **Formatos permitidos**: PDF, DOC, DOCX, XLS, XLSX, PPT, PPTX, TXT, ZIP
- **Tamaño máximo**: 500 MB
- **Ubicación**: `wwwroot/uploads/archivos/`

### Cambiar el tamaño máximo de archivo

Editar en `appsettings.json`:
```json
"MaxFileSize": 524288000  // 500MB en bytes
```

Y en `web.config`:
```xml
<requestLimits maxAllowedContentLength="524288000" />
```

## 🔒 Seguridad

### Crear nuevos usuarios

1. Login como administrador
2. Ir a **Usuarios** → **Nuevo Usuario**
3. Completar el formulario
4. Asignar rol (Usuario o Admin)

### Logs de acceso

Los inicios de sesión se registran en la tabla `AccesosLog`:
- Email del usuario
- Fecha y hora
- IP de origen
- Navegador
- Estado (exitoso/fallido)

Ver logs: Panel Admin → **Logs**

## 🎥 Uso del Sistema

### Como Administrador:

1. **Crear formación**: Panel Admin → Nueva Formación
2. **Subir videos**: Editar formación → Subir Video
3. **Subir archivos**: Editar formación → Subir Archivo
4. **Gestionar usuarios**: Panel Admin → Usuarios

### Como Usuario:

1. Login con credenciales
2. Ver formaciones disponibles
3. Reproducir videos online
4. Descargar material complementario

## 🔧 Solución de Problemas

### Error 500.19
- Verificar que el Hosting Bundle de .NET 8.0 esté instalado
- Reiniciar IIS: `iisreset`

### Error de permisos
- Verificar permisos de la carpeta para IIS_IUSRS y IUSR
- Verificar permisos de escritura en `wwwroot/uploads`

### Videos no se reproducen
- Verificar que el archivo esté en `wwwroot/uploads/videos/`
- Verificar que el formato sea compatible con HTML5 (MP4 recomendado)
- Algunos navegadores requieren HTTPS para reproducción

### Base de datos no se crea
- Verificar permisos de escritura en la carpeta
- Revisar logs en: `logs/stdout`

## 📞 Soporte

Para problemas o preguntas, revisar:
- Logs de IIS en Event Viewer
- Logs de la aplicación en carpeta `logs/`
- Verificar configuración en `web.config` y `appsettings.json`

## 📝 Notas Adicionales

- La aplicación está optimizada para redes locales
- Para producción en internet, se recomienda configurar HTTPS
- Realizar backups regulares de la base de datos y carpeta uploads
- Monitorear el espacio en disco si se suben muchos videos

## 🎨 Personalización

### Cambiar colores y estilos
Editar: `wwwroot/css/site.css`

### Cambiar logo y nombre
Editar: `Views/Shared/_Layout.cshtml`

### Modificar límites de archivos
Editar: `appsettings.json` y `web.config`

---

**Versión**: 1.0  
**Framework**: ASP.NET Core 8.0  
**Base de datos**: SQLite  
**Licencia**: Uso libre
