# 📚 Sistema de Gestión de Biblioteca

Sistema de escritorio desarrollado en **C# con Windows Forms**, utilizando **.NET Framework 4.7.2** y **Microsoft SQL Server** como gestor de base de datos.

El proyecto tiene como objetivo facilitar la administración de una biblioteca mediante módulos para la gestión de libros, usuarios y préstamos, incorporando autenticación y diferentes niveles de acceso según el rol del usuario.

## 🛠️ Tecnologías utilizadas

| Tecnología           | Descripción                           |
| -------------------- | ------------------------------------- |
| C#                   | Lenguaje de programación principal    |
| Windows Forms        | Desarrollo de interfaces gráficas     |
| .NET Framework 4.7.2 | Plataforma de ejecución               |
| SQL Server           | Sistema gestor de base de datos       |
| ADO.NET              | Acceso y consultas a la base de datos |
| Visual Studio        | Entorno de desarrollo                 |
| Git y GitHub         | Control de versiones                  |

## 🚀 Instalación y configuración

### 1. Descargar el proyecto

Clonar el repositorio desde GitHub:

```bash
git clone https://github.com/Star-Lord7/SistemaBiblioteca-USAM.git
```

Una vez descargado, abrir la solución `SistemaBiblioteca.sln` desde Visual Studio.

### 2. Restaurar la base de datos

1. Hacer clic derecho en **Databases**.
2. Seleccionar **Restore Database**.
3. Elegir **Device** y seleccionar el archivo `.bak`.
4. Restaurar la base de datos con el nombre `Biblioteca`.
5. Verificar que la restauración haya finalizado correctamente.

Entre las principales tablas utilizadas por el sistema se encuentran:

| Tabla                 | Función                                    |
| --------------------- | ------------------------------------------ |
| `Libro`               | Almacena los libros registrados            |
| `Autor`               | Almacena los autores                       |
| `Autores_Libros`      | Relaciona libros con autores               |
| `Editorial`           | Almacena las editoriales                   |
| `Categoria`           | Clasifica los libros                       |
| `Usuarios`            | Almacena las cuentas del sistema           |
| `Roles`               | Define los permisos de acceso              |
| `Lector`              | Almacena los lectores asociados a usuarios |
| `Prestamos`           | Registra los préstamos                     |
| `Copias_Libro`        | Relaciona libros, sucursales y ejemplares  |
| `Sucursal Biblioteca` | Almacena las sucursales                    |

### 3. Configurar la conexión a SQL Server

Dentro de Visual Studio, abrir el archivo:

`App.config`

Localizar la cadena de conexión:

```xml
<connectionStrings>
  <add
    name="BibliotecaBD"
    connectionString="Data Source=localhost;Initial Catalog=Biblioteca;Integrated Security=True;TrustServerCertificate=True"
    providerName="System.Data.SqlClient" />
</connectionStrings>
```

Modificar `Data Source` si la instancia de SQL Server tiene un nombre diferente.

Ejemplos:

**Instancia local predeterminada:**

```text
Data Source=localhost
```

**SQL Server Express:**

```text
Data Source=.\SQLEXPRESS
```

**Instancia con nombre personalizado:**

```text
Data Source=NOMBRE_EQUIPO\INSTANCIA
```

### 4. Ejecutar el proyecto

1. Abrir `SistemaBiblioteca.sln`.
2. Esperar a que Visual Studio cargue el proyecto.
3. Verificar que las referencias estén disponibles.
4. Compilar la solución mediante **Build → Build Solution**.
5. Ejecutar utilizando **F5** o el botón **Iniciar**.
6. Esperar a que aparezca la pantalla de inicio de sesión.

## 🏗️ Arquitectura del proyecto

El proyecto está organizado en capas para separar las responsabilidades de la aplicación.

```text
SistemaBiblioteca/
│
├── Models/
│   ├── Libro.cs
│   ├── Autor.cs
│   ├── Editorial.cs
│   ├── Categoria.cs
│   ├── Usuario.cs
│   └── Rol.cs
│
├── Data/
│   ├── ConexionBD.cs
│   ├── LibroRepository.cs
│   ├── UsuarioRepository.cs
│   └── PrestamoRepository.cs
│
├── Services/
│   ├── LibroService.cs
│   ├── PasswordService.cs
│   ├── AuthService.cs
│   ├── UsuarioService.cs
│   └── PrestamoService.cs
│
├── Forms/
│   ├── LibrosForm.cs
│   ├── UsuariosForm.cs
│   ├── PrestamosForm.cs
│   └── InicioForm.cs
│
├── Login.cs
├── MenuPrincipalForm.cs
├── MenuBibliotecarioForm.cs
├── MenuUsuarioForm.cs
├── Program.cs
└── App.config
```

**Models:** Representa las entidades del sistema.

**Data:** Contiene las consultas SQL y las operaciones de acceso a datos.

**Services:** Implementa la lógica de negocio y las validaciones.

**Forms:** Contiene las interfaces gráficas desarrolladas con Windows Forms.

## 🔐 Inicio de sesión

El sistema cuenta con autenticación mediante correo electrónico y contraseña.

Las contraseñas se almacenan utilizando un mecanismo de hash basado en **PBKDF2 con SHA-256**, evitando guardar contraseñas en texto plano dentro de la base de datos.

### Roles disponibles

| Rol           | Descripción                                              |
| ------------- | -------------------------------------------------------- |
| Administrador | Acceso al menú administrativo y gestión de usuarios      |
| Bibliotecario | Acceso al menú correspondiente al personal de biblioteca |
| Usuario       | Acceso al menú destinado a usuarios generales            |

El menú mostrado después del inicio de sesión depende del rol asociado a la cuenta.

### Credenciales de prueba

Para ingresar al sistema, utilizar las credenciales de prueba proporcionadas:

| Campo              | Valor                  |
| ------------------ | ---------------------- |
| Correo electrónico | `admin@biblioteca.com` |
| Contraseña         | `12345`                |
| Rol                | `Administrador`        |

## 🧩 Módulos del sistema

### 🔑 Autenticación y control de acceso

El sistema dispone de una pantalla de inicio de sesión que permite:

- Validar el correo electrónico y la contraseña.
- Verificar que la cuenta esté activa.
- Identificar el rol del usuario.
- Abrir el menú correspondiente.
- Mostrar u ocultar la contraseña.
- Regresar a la pantalla de inicio de sesión al cerrar el menú principal.

### 📖 Gestión de libros

El sistema incorpora un módulo para administrar la información de los libros de la biblioteca.

Entre las funcionalidades desarrolladas se encuentra la integración con la base de datos para consultar la información de los libros y utilizar sus identificadores en las operaciones del sistema.

El módulo se relaciona con los registros de autores, editoriales y categorías.

### 👥 Gestión de usuarios

El módulo permite administrar las cuentas registradas en el sistema.

**Funcionalidades implementadas:**

- Registrar nuevos usuarios.
- Consultar los usuarios existentes.
- Actualizar la información de los usuarios.
- Asignar roles.
- Generar hashes seguros para las contraseñas.
- Mostrar u ocultar la contraseña durante el registro.

**Asociación automática con lectores:**

Cuando se registra una cuenta con el rol `Usuario`, el sistema genera automáticamente un registro asociado en la tabla `Lector`.

El número de tarjeta se genera con cinco dígitos, tomando como referencia el identificador del usuario.

Ejemplo:

| IdUsuario | Num_Tarjeta |
| --------- | ----------- |
| 1         | 00001       |
| 15        | 00015       |
| 125       | 00125       |

Esta operación se realiza mediante una transacción para mantener la consistencia entre ambas tablas.

### 📚 Gestión de préstamos

El sistema incorpora un módulo para registrar y consultar préstamos de libros.

**Funcionalidades implementadas:**

- Cargar lectores registrados desde la base de datos.
- Mostrar los títulos de los libros disponibles en el catálogo.
- Cargar las sucursales registradas.
- Seleccionar el lector que solicita el préstamo.
- Seleccionar el libro y la sucursal.
- Establecer la fecha del préstamo.
- Establecer la fecha prevista de devolución.
- Registrar préstamos en SQL Server.
- Consultar los préstamos registrados.
- Buscar préstamos por usuario o libro.
- Limpiar los campos del formulario.
- Validar que la fecha prevista de devolución sea posterior a la fecha del préstamo.

**Consideración:** El registro de préstamos todavía no implementa un control completo de disponibilidad de ejemplares por sucursal.

### 🏢 Sucursales

El sistema utiliza sucursales registradas previamente en SQL Server.

Estas sucursales aparecen en el formulario de préstamos mediante un ComboBox.

La información se obtiene directamente desde la tabla correspondiente de la base de datos.

## ⚠️ Funcionalidades pendientes y limitaciones

El proyecto se encuentra en una etapa de desarrollo funcional, por lo que existen características que todavía requieren implementación.

Entre ellas:

- Validación de disponibilidad real de ejemplares por sucursal.
- Gestión completa de devolución de libros.
- Identificación automática de préstamos vencidos.
- Búsqueda de préstamos según su estado.
- Mejoras de adaptación visual de los formularios.
- Ampliación de validaciones y manejo de errores.

Estas funcionalidades no deben considerarse completamente implementadas en la versión actual.

## 🧪 Pruebas recomendadas

Después de instalar el sistema, se recomienda realizar las siguientes comprobaciones:

1. Iniciar sesión utilizando una cuenta válida.
2. Verificar que se abra el menú correspondiente al rol.
3. Consultar los libros registrados.
4. Registrar un usuario con rol `Usuario`.
5. Comprobar que se genere su registro de lector y número de tarjeta.
6. Abrir el módulo de préstamos.
7. Verificar que aparezcan los lectores, títulos de libros y sucursales.
8. Registrar un préstamo con fechas válidas.

## 👨‍💻 Integrantes del proyecto

- Stephany Lourdes Alvarado Flores - 250736
- Samuel Isaac Azucena Gómez - 250639

## 📌 Estado del proyecto

**Versión:** 1.0 — USAM Programación 1

**Estado:** Funcional con características pendientes de implementación.

**Año:** 2026

El proyecto fue desarrollado con fines académicos para aplicar conocimientos de programación orientada a objetos, desarrollo de aplicaciones de escritorio, bases de datos relacionales, autenticación de usuarios y organización del código mediante capas.
