# 🏢 GESTIÓN DE USUARIOS POR EMPRESA

## Funcionalidad Implementada

El sistema ahora permite **agrupar y gestionar usuarios por empresa**, facilitando el control y seguimiento de usuarios de diferentes organizaciones.

---

## 📋 **Características Principales**

### 1. **Campo Empresa Obligatorio**
- Cada usuario debe tener asignada una empresa
- Campo con autocompletado de empresas existentes
- Permite crear nuevas empresas libremente

### 2. **Agrupación Visual**
- Los usuarios se muestran agrupados por empresa
- Cada grupo muestra el número de usuarios
- Diseño tipo acordeón para fácil navegación

### 3. **Filtrado Dinámico**
- Filtro desplegable por empresa
- Actualización instantánea sin recargar la página
- Muestra el número de usuarios por empresa

### 4. **Estadísticas Detalladas**
- Panel completo de estadísticas por empresa
- Gráficos de distribución
- Información de accesos y actividad

---

## 🎯 **Cómo Usar el Sistema**

### **Crear Usuario con Empresa**

1. **Login como administrador**
2. Ir a: **Usuarios** → **Nuevo Usuario**
3. **Completar el formulario:**
   - **Empresa**: Escribir o seleccionar del desplegable
   - Email, Nombre, Contraseña
   - Rol (Usuario o Admin)
4. Click en **Crear Usuario**

**Ejemplo:**
```
Empresa: Transportes ABC
Email: juan@transportesabc.com
Nombre: Juan Pérez
Rol: Usuario
```

---

### **Ver Usuarios Agrupados por Empresa**

1. **Login como administrador**
2. Ir a: **Usuarios**
3. Verás usuarios agrupados por empresa:

```
┌─────────────────────────────────────┐
│ 🏢 Transportes ABC (3 usuarios)    │
├─────────────────────────────────────┤
│ - juan@transportesabc.com           │
│ - maria@transportesabc.com          │
│ - carlos@transportesabc.com         │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ 🏢 Logística XYZ (2 usuarios)      │
├─────────────────────────────────────┤
│ - ana@logisticaxyz.com              │
│ - pedro@logisticaxyz.com            │
└─────────────────────────────────────┘
```

---

### **Filtrar por Empresa**

1. En la página de **Usuarios**
2. Usar el **selector desplegable** en la parte superior
3. Seleccionar la empresa que quieres ver
4. Solo se mostrarán usuarios de esa empresa
5. Click en **Limpiar** para ver todas

---

### **Ver Estadísticas por Empresa**

1. **Login como administrador**
2. Ir a: **Estadísticas** (menú superior)
3. Verás un dashboard completo con:

#### **Resumen General:**
- Total de empresas registradas
- Total de usuarios
- Usuarios activos
- Administradores

#### **Tabla Detallada:**
- Usuarios por empresa
- Activos vs Inactivos
- Número de administradores
- Último acceso
- Porcentaje del total

#### **Gráfico de Distribución:**
- Barras visuales de distribución
- Porcentajes por empresa
- Top 10 empresas

---

## 📊 **Casos de Uso**

### **Caso 1: Empresa de Transporte con Múltiples Sucursales**

```
Empresa: Transportes Norte
├── admin@transportesnorte.com (Admin)
├── conductor1@transportesnorte.com (Usuario)
├── conductor2@transportesnorte.com (Usuario)
└── gestor@transportesnorte.com (Usuario)

Empresa: Transportes Sur
├── admin@transportessur.com (Admin)
└── conductor1@transportessur.com (Usuario)
```

**Ventaja**: Puedes ver rápidamente cuántos usuarios tiene cada sucursal.

---

### **Caso 2: Capacitación para Clientes Externos**

```
Empresa: Cliente A - Construcciones
├── operador1@construcciones.com
└── operador2@construcciones.com

Empresa: Cliente B - Minería
├── conductor1@mineria.com
└── conductor2@mineria.com
```

**Ventaja**: Control separado de usuarios por cliente.

---

### **Caso 3: Control de Accesos**

**Ver quién accedió recientemente:**
1. Ir a **Estadísticas**
2. Ver columna "Último Acceso" por empresa
3. Identificar empresas sin actividad reciente

---

## 🔍 **Reportes y Análisis**

### **Preguntas que puedes responder:**

✅ **¿Cuántas empresas tengo registradas?**
- Panel Admin → Tarjeta de "Empresas"

✅ **¿Qué empresa tiene más usuarios?**
- Estadísticas → Ver tabla ordenada

✅ **¿Cuántos usuarios activos tiene cada empresa?**
- Estadísticas → Columna "Activos"

✅ **¿Cuándo fue el último acceso por empresa?**
- Estadísticas → Columna "Último Acceso"

✅ **¿Qué empresas no han accedido recientemente?**
- Estadísticas → Buscar fechas antiguas o "Sin accesos"

✅ **¿Cuántos administradores tiene cada empresa?**
- Estadísticas → Columna "Admins"

---

## 💡 **Mejores Prácticas**

### **1. Nomenclatura de Empresas**
- Usar nombres claros y consistentes
- Evitar abreviaciones confusas
- Mantener formato uniforme

**Bien:**
```
Transportes ABC S.A.
Logística Global
Construcciones del Norte
```

**Evitar:**
```
Trans ABC
Log Glob
Const N
```

### **2. Asignación de Roles**
- Cada empresa debería tener al menos 1 Admin
- Usuarios normales para el personal operativo
- Revisar periódicamente los accesos

### **3. Mantenimiento**
- Revisar usuarios inactivos mensualmente
- Desactivar cuentas de personal que ya no trabaja
- Actualizar datos de empresa si hay cambios

### **4. Seguridad**
- No compartir contraseñas entre usuarios
- Cada persona debe tener su propio usuario
- Usar la empresa real, no genéricos

---

## 🚀 **Flujo de Trabajo Recomendado**

### **Para Administradores del Sistema:**

1. **Inicio del día:**
   - Revisar **Estadísticas** para ver actividad
   - Verificar accesos en **Logs**

2. **Cuando llega una nueva empresa:**
   - Crear usuario administrador de la empresa
   - El admin de la empresa puede crear sus propios usuarios

3. **Mensualmente:**
   - Revisar estadísticas por empresa
   - Identificar empresas inactivas
   - Limpiar usuarios no utilizados

4. **Cuando hay cambios:**
   - Actualizar datos de usuarios
   - Reagrupar si cambia de empresa

---

## ⚙️ **Configuración Avanzada**

### **Migración de Base de Datos Existente**

Si ya tienes usuarios sin empresa:

1. **Detener la aplicación**
2. **Eliminar formaciones.db**
3. **Ejecutar `dotnet run`**
4. Se creará nueva BD con campo empresa

**O manualmente:**
- Abrir formaciones.db con DB Browser
- Editar tabla Usuarios
- Asignar empresa a cada usuario

---

## 📞 **Preguntas Frecuentes**

**P: ¿Puedo cambiar la empresa de un usuario?**
R: Actualmente no hay vista de edición, pero puedes eliminar y recrear el usuario.

**P: ¿Puedo tener usuarios sin empresa?**
R: No, el campo es obligatorio para mantener el orden.

**P: ¿Cuántas empresas puedo tener?**
R: Ilimitadas.

**P: ¿Los usuarios ven solo su empresa?**
R: Los usuarios normales ven todas las formaciones. Solo los admins ven la gestión de empresas.

**P: ¿Puedo exportar las estadísticas?**
R: No implementado aún, pero puedes hacer captura de pantalla del dashboard.

---

## 🎯 **Próximas Mejoras Sugeridas**

- [ ] Edición de usuarios (cambiar empresa)
- [ ] Exportar estadísticas a Excel
- [ ] Restringir formaciones por empresa
- [ ] Notificaciones por empresa
- [ ] Dashboard personalizado por empresa

---

**Versión:** 2.0  
**Última actualización:** 2024  
**Funcionalidad:** Gestión de Usuarios por Empresa
