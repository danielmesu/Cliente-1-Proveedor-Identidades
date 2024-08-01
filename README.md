# Proveedor de identidades C#

Este proyecto está realizado en .NET 8.0 C#.

Es una aplicación web de única página que trabaja como un proveedor de identidades y tokens de acceso por medio del nuget IdentityServer4.

## Program

En el archivo program se configura el servicio de IdentityServer4 con los siguientes métodos: 

- AddIdentityServer(): Agrega el servidor de identidades al programa. 

- AddDeveloperSigningCredential(): Realiza la firma de un certificado de desarrollo para poder generar tokens firmados.

- AddInMemoryClients(): Agrega una lista de clientes del servidor, cada cliente se comportará como una aplicacón registrada a la cual los usuarios podrán solicitar autenticación. 

- AddInMemoryApiResources(): Agrega los recursos de la api de autenticación indicando los scopes que tendrá la autenticación y los claims que serán incluidos en los JWT.

- AddInMemoryApiScopes(): Agrega las instancias simuladas de los scopes de la aplicación.

- AddTestUsers(): Agrega una lista de usuarios simulados, esta información corresponde a la base de datos de credenciales para cada usuario registrado, incluye los datos de autenticación y los claims que tendrá cada usuario.

- UseIdentityServer(): Agrega el servidor de autenticación ya configurado a la aplicación.
