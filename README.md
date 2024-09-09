# Descripción del proyecto

API realizada con C# .NET 8 como minimal API
Paquetes:
-  Newtonsoft.Json
-  Microsoft.AspNet.WebApi.Cors
-  Microsoft.AspNetCore.Mvc.Testing
  
Front realizado en Angular 17
Paquetes:
-  Material UI
-  SweetAlert2

 <hr>

# Notas

Para ejecutar el front se debe correr en la consola "npm i" y luego "ng serve" o "npm run ng serve"
Si se inicia en un puero diferente a localhost:4200 se deberá incluir la url en "path del repo"\API\Suriscode API\appsettings.json en la sección "AllowedHosts", para incluirla en el Cors Policy.
El back utiliza unos packetes nuGets, por lo que se deben restaurar los mismos antes de correr la API que se puede iniciar por IIS Express y abre directamente la documentación por Swagger.

# Aclaraciones

El front valida, por renderizado condicional del botón del "Realizar pedido", que el pedido sea válido para poder llamar al método que utiliza el servicio que genera la acción al backend, para poder probar las respuestas incorrectas se puede eliminar el *ngIf de la línea 32 de home.component.html

Usé material, por lo que los componentes de utils quedaron sin usar, la idea era armar componentes custom, pero no me enfoqué en el diseño.

En el controlador están los comentarios, pero el link al get de vendedores de mocky estaba roto, así que armé uno en https://mocki.io/v1/5d1bc82b-21dd-4ba5-9870-4063d45e8ab0 y por las dudas hice el controlador en el back, el servicio del front tiene ambos métodos y en el llamado verifica que vuelva algo de la URL externa, si no llama al endpoint creado.

Faltaron test, porque no había trabajado nunca con minimal API, al no tener controllers y sus métodos, se debe testear por pruebas de integración, a las que no estoy acostumbrada con el httpClient y sus Assertions específicos.

El primer commit salió a nombre de Alejandro, porque estuve usando su pc, luego de notarlo configuré localmente mi usuario de git.
