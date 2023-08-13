Front:  
Tecnologia principal:  
React con Vite  
Frameworks o bibliotecas:  
Google Oauth2  
JWT JsonWebToken  
Tailwind CSS  
Axios  

Back:  
Tecnologia principal:  
.NET 6  
Frameworks o bibliotecas:  
Entity Framework  
Google Oauth2  
JWT JsonWebToken  
Puppeteer  
Quartz Scheduler (Tareas CRON)  
Google API (Integración con Google Drive y Google Calendar)  
MSTests  

Datos:  
Tecnologia principal:  
Microsoft SQL  
(Es completamente administrado por el ORM (Entity Framework) no se hacen cambios sobre la base de datos)  

Entornos: 
Dev (Front: localhost + Back: localhost + Datos: Docker)  
Test (Front: Netlify + Back: Render.com + Datos: AWS)  
Prod (Front: Netlify + Back: Render.com + Datos: AWS)  
Cada entorno y cada proveedor (Render, Netlify, Github, etc) tiene configuradas las variables de entorno necesarias para conectarse con la capa de software que sea necesarias. NO se escriben datos de API keys o ConnectionStrings de bases de datos en el código.  

DevOps: 
Cada desarrollador crea un nuevo branch basado en el Github Issue que toma del pool de tareas. Cuando termina la tarea hace un pull request que luego se hace un Merge al branch de desarrollo, si todo funciona correctamente se hace el merge con branch de Test y se prueba en la web (no localhost).  
Eventualmente cuando se haga el release de la aplicación se hara el mismo procedimiento para pasar de Test a Prod.  
  
DevOps (Backend):  
Cada PR (Pull Request) activa un Pipeline de CI/CD (Continuous Integration/Continuous Delivery) que hace un build del proyecto de .NET, si el build se realiza sin fallas, corre los tests automaticos del proyecto y si pasa los tests hace un deploy automatico en Render.com  
Render usa el archivo Dockerfile para generar una imagen de docker y monta el contenedor con la aplicación.  