# 🔐 CREDENCIALES DE ACCESO - PLATAFORMA DE FORMACIONES

## 📱 Acceso a la Aplicación

**URL**: http://localhost:5000  
**Puerto**: 5000

---

## 👥 Usuarios Disponibles

### 👨‍💼 ADMINISTRADOR
- **Email**: `admin@formaciones.com`
- **Contraseña**: `Admin123!`
- **Empresa**: Administración
- **Rol**: Admin

**Permisos y Funciones:**
- ✅ Crear nuevas formaciones (módulos)
- ✅ Editar formaciones existentes
- ✅ Subir videos a formaciones
- ✅ Subir archivos adjuntos
- ✅ Gestionar usuarios (crear, editar, eliminar)
- ✅ Ver estadísticas por empresa
- ✅ Ver logs de acceso con IP y navegador
- ✅ Acceso total al sistema

---

### 👤 USUARIO DE PRUEBA
- **Email**: `usuario@formaciones.com`
- **Contraseña**: `Usuario123!`
- **Empresa**: Netoffice
- **Rol**: Usuario

**Permisos y Funciones:**
- ✅ Ver formaciones disponibles
- ✅ Reproducir videos online
- ✅ Descargar archivos adjuntos
- ❌ NO puede crear formaciones
- ❌ NO puede subir contenido
- ❌ NO puede acceder al panel de administración

---

## 📚 FORMACIONES DISPONIBLES

### 🎓 Introducción a NetOffice
**Descripción:** Módulo de formación que cubre los conceptos básicos de utilización del software NetOffice para la gestión de transporte.

**Categoría:** Conceptos Básicos

**Contenido:**

#### 📹 Videos:
1. **Video 1: Introducción al Sistema**
   - Duración: ~30 minutos (1 MB)
   - Descripción: Visión general de la plataforma NetOffice
   - Archivo: `sample-video-1.mp4`

2. **Video 2: Configuración Inicial**
   - Duración: ~45 minutos (1.5 MB)
   - Descripción: Pasos para configurar tu primera formación
   - Archivo: `sample-video-2.mp4`

#### 📄 Archivos Adjuntos:
1. **Guía Rápida de Inicio**
   - Tamaño: 3 KB
   - Descripción: Manual de inicio rápido para nuevos usuarios
   - Archivo: `guia-rapida.txt`

2. **Documentación Completa**
   - Tamaño: 5 KB
   - Descripción: Documentación técnica completa del sistema
   - Archivo: `documentacion.txt`

---

## 🔄 PERSISTENCIA DE DATOS

✅ **Los datos se guardan automáticamente en la base de datos SQLite**

**Características importantes:**
- ✅ Al cerrar sesión, los cambios se mantienen
- ✅ Al cerrar y reiniciar la aplicación, todos los datos persisten
- ✅ Los videos y archivos se almacenan en:
  - Videos: `wwwroot/uploads/videos/`
  - Archivos: `wwwroot/uploads/archivos/`
- ✅ Cada usuario tiene registro de acceso con:
  - Fecha y hora de acceso
  - IP de origen
  - Navegador utilizado
  - Estado (exitoso/fallido)

---

## 🚀 PASOS PARA PROBAR

### Como Usuario Administrador:

1. Navega a: **http://localhost:5000**
2. Inicia sesión:
   - Email: `admin@formaciones.com`
   - Contraseña: `Admin123!`
3. Verás el **Panel de Administración** con:
   - Total de formaciones
   - Total de videos
   - Total de archivos  
   - Total de usuarios
   - Total de empresas
4. Puedes:
   - Crear nuevas formaciones
   - Editar la formación existente
   - Subir más videos y archivos
   - Gestionar usuarios
   - Ver estadísticas

### Como Usuario Normal:

1. Navega a: **http://localhost:5000**
2. Inicia sesión:
   - Email: `usuario@formaciones.com`
   - Contraseña: `Usuario123!`
3. Verás **Mis Formaciones** con:
   - Formación "Introducción a NetOffice"
4. Haz clic en **"Ver Formación"**
5. Podrás:
   - Reproducir los 2 videos
   - Descargar los 2 archivos adjuntos
   - Ver descripciones de cada contenido

---

## 📊 PANEL DE ADMINISTRACIÓN

### Secciones disponibles:

**Dashboard Principal:**
- Estadísticas de formaciones, videos, archivos y usuarios
- Contador de empresas activas

**Gestión de Formaciones:**
- Crear nuevas formaciones
- Editar formaciones existentes
- Subir videos (hasta 500MB cada uno)
- Subir archivos (hasta 500MB cada uno)
- Organizar conten por orden
- Activar/desactivar formaciones

**Gestión de Usuarios:**
- Crear nuevos usuarios
- Asignar rol (Admin/Usuario)
- Filtrar por empresa
- Activar/desactivar usuarios
- Eliminar usuarios

**Estadísticas por Empresa:**
- Usuarios por empresa
- Usuarios activos/inactivos
- Administradores por empresa
- Gráficos de distribución
- Último acceso

**Logs de Acceso:**
- Historial completo de accesos
- Email de usuario
- Fecha y hora exacta
- IP de origen
- Navegador utilizado
- Estado (exitoso/fallido)

---

## 💾 COPIA DE SEGURIDAD

Para hacer backup de los datos:

**Base de datos:**
```
Ruta: formaciones.db
Ubicación: Carpeta raíz del proyecto
```

**Archivos (Videos y Documentos):**
```
Ruta: wwwroot/uploads/
  ├── videos/       (videos de formaciones)
  └── archivos/     (documentos adjuntos)
```

---

## 🏢 GESTIÓN DE EMPRESAS

**Características:**
- Agrupación automática de usuarios por empresa
- Filtrado por empresa en la gestión
- Estadísticas detalladas por empresa
- Control de accesos segmentado

**Empresas actuales:**
- Administración (usuario admin)
- Netoffice (usuario de prueba)
- Nuevas empresas se pueden agregar al crear usuarios

---

## ⚠️ NOTAS IMPORTANTES

1. **Base de datos SQLite:**
   - Se crea automáticamente en la primera ejecución
   - Ubicación: `formaciones.db` en la raíz del proyecto

2. **Primera ejecución:**
   - Si eliminas `formaciones.db`, se recreará con datos de inicialización
   - Se crearán automáticamente: admin, usuario de prueba y formación de ejemplo

3. **Videos y Archivos de ejemplo:**
   - Son placeholders que puedes reemplazar con contenido real
   - Ubicados en `wwwroot/uploads/`

4. **Seguridad:**
   - Las contraseñas se almacenan hasheadas con BCrypt
   - La autenticación usa cookies seguras
   - Logs de acceso para auditoría

---

## 🔐 CAMBIOS DE CONTRASEÑA

**Función próximamente:**
- Panel de cambio de contraseña en el perfil del usuario
- Validación de contraseña actual
- Requisitos de complejidad

---

## 📞 SOPORTE Y TROUBLESHOOTING

**Si tienes problemas:**

1. Verifica que la aplicación esté ejecutándose:
   - Abre: http://localhost:5000
   - Deberías ver la pantalla de login

2. Revisa los logs en la consola para errores

3. Comprueba permisos de escritura:
   - Carpeta `wwwroot/uploads/`
   - Archivo `formaciones.db`

4. Limpia la base de datos si es necesario:
   - Elimina `formaciones.db`
   - Reinicia la aplicación
   - Se recreará automáticamente

---

**Última actualización:** Febrero 2026  
**Versión:** 1.0  
**Estado:** ✅ Completamente funcional

---

## 📝 Cómo usar

### Para probar como ADMINISTRADOR:
1. Ir a: http://localhost:5000
2. Login: admin@formaciones.com / Admin123!
3. Crear formaciones y subir contenido
4. Gestionar usuarios por empresa

### Para probar como USUARIO:
1. Cerrar sesión (si estás como admin)
2. Login: usuario@formaciones.com / Usuario123!
3. Ver y consumir las formaciones

---

## ➕ Crear más usuarios

### Desde el panel de administración:
1. Login como admin
2. Ir a: **Usuarios** → **Nuevo Usuario**
3. Completar el formulario **incluyendo la empresa**
4. Puedes escribir una nueva empresa o seleccionar una existente del desplegable
5. Seleccionar rol: **Usuario** o **Admin**

### Características del campo empresa:
- **Autocompletado**: Sugiere empresas ya registradas
- **Creación libre**: Puedes escribir una empresa nueva
- **Agrupación automática**: Los usuarios se agrupan visualmente por empresa
- **Filtrado**: Puedes filtrar la lista de usuarios por empresa

---

## 🔒 Seguridad

⚠️ **IMPORTANTE:** Estas son credenciales de PRUEBA. 

En producción:
- Cambiar la contraseña del administrador
- Eliminar o cambiar la contraseña del usuario de prueba
- Crear usuarios reales con contraseñas seguras
- Asignar usuarios a sus empresas correspondientes

---

## 🗑️ Eliminar usuarios de prueba

Si quieres eliminar el usuario de prueba:

**Opción 1:** Desde el panel de administración
- Usuarios → Botón de eliminar (🗑️)

**Opción 2:** Eliminar la base de datos y reiniciar
```bash
# Detener la aplicación (Ctrl+C)
# Eliminar el archivo: formaciones.db
# Ejecutar de nuevo: dotnet run
```

---

## 📊 Control de empresas

### Ver qué empresas tienen más usuarios:
Panel Admin → **Estadísticas** → Ver distribución

### Filtrar usuarios por empresa:
Panel Admin → **Usuarios** → Usar el selector de empresa

### Crear usuarios de la misma empresa:
Al crear usuario, escribe o selecciona la empresa del desplegable

---

**Última actualización:** 2024  
**Versión:** 2.0 - Con gestión por empresas
