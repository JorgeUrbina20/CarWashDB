# 🚘 CarWashDB

> Sistema de gestión para negocios de autolavado desarrollado en **SQL Server 2022**.

CarWashDB es una base de datos relacional diseñada para administrar clientes, vehículos, órdenes de servicio, empleados, pagos, inventario y auditoría dentro de un carwash, siguiendo estándares profesionales de normalización y seguridad.

---

# ✨ Características

* Gestión de clientes y vehículos
* Control de órdenes de lavado
* Administración de empleados y usuarios
* Facturación y pagos
* Gestión de inventario y proveedores
* Auditoría mediante triggers
* Procedimientos almacenados
* Diseño normalizado en **3FN**
* Sistema de roles y permisos

---

# 🛠 Tecnologías

| Tecnología | Versión |
| ---------- | ------- |
| SQL Server | 2022+   |
| T-SQL      | Native  |
| SSMS       | 19+     |

---

# 📂 Estructura del Proyecto

```bash id="e7ws9t"
Proyecto_Carwash/
│
├── database/
│   ├── Backup/Backups.sql
│   ├── Consultas-CarWash.sql
│   ├── Procedimiento-Master.sql
│   ├── Trigger-Auditoria.sql
│   ├── Users.sql
│   └── Vistas-Servicios-Realizados.sql
│   ├── Tablas-Carwash.sql
│   └── Insercion-Datos.sql
│
├── imagesSchema/
│   └── ERD.png
│
└── README.md
```

---

# 📊 Flujo del Sistema

```text id="d9cq0n"
Cliente → Vehículo → Orden → Servicio → Pago
```

Estados de órdenes:

* Pendiente
* Lavando
* Terminado
* Entregado

---

# 🔐 Seguridad

* Roles personalizados
* Permisos granulares
* Contraseñas hasheadas
* Procedimientos almacenados
* Auditoría automática

---

# 🧾 Procedimientos Incluidos

| Procedimiento                | Función            |
| ---------------------------- | ------------------ |
| `sp_InsertarEmpleado`        | Registrar empleado |
| `sp_InsertarCliente`         | Registrar cliente  |
| `sp_CrearOrden`              | Crear orden        |
| `sp_CambiarEstadoOrden`      | Cambiar estado     |
| `sp_CrearServicioDesdeOrden` | Facturar servicio  |
| `sp_ReporteCajaDiario`       | Reporte diario     |

---

# 🚀 Instalación

## Clonar repositorio

```bash id="9a4kkv"
git clone https://github.com/JorgeUrbina20/CarWashDB.git
```

## Ejecutar scripts

```sql id="7q6ok1"
schema.sql
seed.sql
procedures.sql
triggers.sql
views.sql
```

---

# 📐 Diseño de Base de Datos

* Modelo relacional
* 19 tablas
* Integridad referencial
* Normalización en **3FN**
* Arquitectura escalable

---

# 👨‍💻 Autor

**Jorge Luis Nuñez Urbina**

Proyecto desarrollado con enfoque profesional utilizando buenas prácticas de modelado relacional y administración en SQL Server.

---

# 📄 Licencia

Proyecto de uso educativo y académico.

