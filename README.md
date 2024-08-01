# Cliente 1 proveedor de identidades C#

Este proyecto está realizado en .NET 8.0 C#.

Es una aplicación web MVC que trabaja como un cliente del proveedor de identidades C#.

Este proyecto incluye un proceso de autenticación mediante login de credenciales básicas (usuario y contraseña) y diversas vistas protegidas mediante autenticación y autorización por RBA (acceso por roles).

## Program

En el archivo program se configura el servicio de autenticación, la cookie de enrutamiento de vistas y la inyección singleton del módulo de autenticación como dependencia usando inversión de control.

## ModuloAutenticacionProveedorCSharp

Este módulo es inyectado como dependencia a través de inversión de control, y permite la autenticación del usuario mediante el consumo HTTP del proveedor de identidades c# mediante los siguientes métodos: 

- ConsumirProveedorIdentidades(): Realiza el consumo rest del servicio de autenticación del proveedor de identidades c# (externo), y según la respuesta del servicio, se genera la de serialización de la respuesta en el objeto "TokenResponse".

- AutenticarUsuario(): Con base en lo obtenido mediante el método ConsumirProveedorIdentidades(), se crea un controlador de JWT que permite extraer los claims del usuario y con estos crear el "ClaimsPrincipal" de la sesión del usuario para generar la autenticación de este en el contexto HTTP del programa, abriendo una sesión para el usuario a través del token de acceso dado por el proveedor; que al ser centralizado puede funcionar como un SSO para diferentes clientes. 

## AccountController 

Este controlador se usa por defecto en el programa para habilitar la interfaz de login, y a través del módulo de autenticación inyectado como dependencia se realiza la autenticación del usuario. También contiene servicios para la generación de vista de acceso denegado para usuarios sin autorización y el servicio de cierre de sesión que finaliza la sesión del usuario en el contexto HTTP del programa. 

## HomeController

Este controlador es la capa funcional que requiere de una sesión de usuario abierta, para esto se usa el decorador [Authorize] que indica que el usuario debe estar autenticado en el sistema para poder acceder a los siguientes servicios expuestos: 

- Index(): Genera la vista "Home" de la aplicación, en ella se expone una breve descripción del funcionamiento y se visualizan algunos claims no sensibles del usuario obtenidos a través de la identidad de su sesión. 

- Privacy(): Este servicio tiene una capa de protección a través de RBA por medio del decorador [Authorize(Roles = "administrador")], el cual indica que el servicio solo estará disponible para usuarios cuyo claim de rol sea "administrador", en caso contrario el sistema dirigirá al usuario a la vista definida por "AccessDeniedPath" en el program, que presenta una interfaz indicando al usuario que no tiene acceso permitido para visualizar la información del servicio solicitado. 
En caso de que el usuario sí tenga el rol solicitado, se presenta una vista donde se expone una breve descripción del funcionamiento y se visualizan claims no sensibles del usuario y el claim del JWT entregado por el proveedor de identidades, el cual se entiende como dato sensible. 

- Error(): Como parte del control de la aplicación, este servicio expone una vista genérica en caso de que se presente un error en el sistema. 
