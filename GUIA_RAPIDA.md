# GUÍA RÁPIDA DE USO

## 🚀 Instalación Rápida en IIS

### 1. Requisitos
- Windows Server con IIS
- .NET 8.0 Runtime - ASP.NET Core Hosting Bundle
  Descargar: https://dotnet.microsoft.com/download/dotnet/8.0
  Instalar: "ASP.NET Core Runtime 8.0.x - Windows Hosting Bundle"

### 2. Publicar la aplicación
Ejecuta el archivo `publicar.bat` o manualmente:
```bash
dotnet restore
dotnet publish -c Release -o ./publish
```

### 3. Configurar en IIS
1. Administrador de IIS → Sitios → Agregar sitio web
2. Nombre: FormacionesApp
3. Ruta física: [ruta a carpeta publish]
4. Puerto: 80
5. En Grupos de aplicaciones → Tu sitio → Configuración avanzada
   → Versión .NET CLR: "Sin código administrado"

### 4. Permisos
- Carpeta publish → Propiedades → Seguridad
- Agregar permisos de "Modificar" para: IIS_IUSRS y IUSR

### 5. Primer acceso
URL: http://localhost (o tu dominio)
Usuario: admin@formaciones.com
Contraseña: Admin123!

---

## 📚 USO DEL SISTEMA

### COMO ADMINISTRADOR

#### Crear una formación:
1. Login → Panel Admin → Nueva Formación
2. Completa: Título, Descripción, Categoría
3. Click en "Crear Formación"

#### Subir videos:
1. Panel Admin → Editar formación (botón lápiz)
2. En sección "Videos" → Completar formulario
3. Seleccionar archivo de video (MP4 recomendado)
4. Click "Subir Video"
5. El video aparecerá en la lista

#### Subir archivos complementarios:
1. En la misma pantalla de edición
2. Sección "Archivos Adjuntos" → Completar formulario
3. Seleccionar archivo (PDF, Word, etc.)
4. Click "Subir Archivo"

#### Crear usuarios:
1. Panel Admin → Usuarios → Nuevo Usuario
2. Completar: Email, Nombre, Contraseña
3. Seleccionar rol: Usuario o Admin
4. Click "Crear Usuario"

#### Ver logs de acceso:
1. Panel Admin → Logs
2. Ver últimos 100 intentos de login

### COMO USUARIO

#### Ver formaciones:
1. Login con tus credenciales
2. Verás tarjetas con todas las formaciones disponibles

#### Ver una formación:
1. Click en "Ver Formación"
2. Los videos se reproducen directamente en el navegador
3. Los archivos se pueden descargar desde el panel derecho

---

## 🎥 GESTIÓN DE VIDEOS

### Formatos recomendados:
- **MP4 (H.264)**: Mejor compatibilidad con navegadores
- **Resolución**: 1280x720 (HD) o 1920x1080 (Full HD)
- **Tamaño**: Hasta 500 MB por video

### Convertir videos a MP4 (si tienes otro formato):
Usa herramientas gratuitas como:
- HandBrake
- VLC Media Player (Medio → Convertir/Guardar)
- ffmpeg

---

## 📁 ESTRUCTURA DE ARCHIVOS

```
publish/                          # Carpeta publicada
├── formaciones.db               # Base de datos SQLite
├── wwwroot/
│   └── uploads/
│       ├── videos/              # Videos subidos aquí
│       └── archivos/            # Documentos aquí
├── web.config                   # Configuración IIS
└── [demás archivos de la app]
```

---

## 💾 BACKUP

### Hacer respaldo:
1. Copia el archivo `formaciones.db`
2. Copia la carpeta `wwwroot/uploads`

### Restaurar backup:
1. Reemplaza `formaciones.db` con tu backup
2. Reemplaza la carpeta `wwwroot/uploads`

---

## ⚙️ CONFIGURACIONES AVANZADAS

### Cambiar tamaño máximo de archivos:

**appsettings.json:**
```json
"MaxFileSize": 1048576000  // 1GB en bytes
```

**web.config:**
```xml
<requestLimits maxAllowedContentLength="1048576000" />
```

### Agregar más formatos de video permitidos:

**appsettings.json:**
```json
"AllowedVideoExtensions": [".mp4", ".avi", ".mov", ".wmv", ".mkv", ".webm"]
```

### Agregar más formatos de archivos:

**appsettings.json:**
```json
"AllowedFileExtensions": [".pdf", ".doc", ".docx", ".xls", ".xlsx", 
                          ".ppt", ".pptx", ".txt", ".zip", ".rar"]
```

---

## 🔧 SOLUCIÓN DE PROBLEMAS COMUNES

### ❌ Error 500.19
**Problema**: IIS no encuentra el módulo ASP.NET Core
**Solución**: Instalar "ASP.NET Core Hosting Bundle" y reiniciar IIS
```
iisreset
```

### ❌ Videos no se ven
**Problema**: Formato no compatible
**Solución**: Convertir a MP4 (H.264)

### ❌ Error al subir archivos grandes
**Problema**: Límite excedido
**Solución**: 
1. Aumentar límite en appsettings.json
2. Aumentar límite en web.config
3. Reiniciar el sitio en IIS

### ❌ No puedo crear usuarios
**Problema**: Permisos de base de datos
**Solución**: Verificar permisos de escritura en la carpeta para IIS_IUSRS

---

## 📊 MONITOREO

### Ver logs de errores:
1. Windows Event Viewer → Registros de aplicaciones y servicios → Microsoft → IIS
2. Carpeta `logs/` en la aplicación

### Ver logs de acceso:
Panel Admin → Logs (muestra intentos de login)

---

## 🔐 SEGURIDAD

### Cambiar contraseña del admin:
1. Conectarse a la base de datos `formaciones.db`
2. Usar una herramienta como DB Browser for SQLite
3. O crear un nuevo usuario admin y eliminar el predeterminado

### Crear usuario admin adicional:
Panel Admin → Usuarios → Nuevo Usuario → Rol: Admin

---

## 📞 CONTACTO Y SOPORTE

Para problemas técnicos:
1. Revisar README.md completo
2. Verificar logs en Event Viewer
3. Verificar permisos de carpetas
4. Verificar que el Hosting Bundle esté instalado

---

**Última actualización**: 2024  
**Versión**: 1.0
