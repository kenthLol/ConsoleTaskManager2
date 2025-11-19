# ConsoleTaskManager2

Aplicación de consola en .NET que permite gestionar un listado de tareas pendientes de forma interactiva. El programa expone un menú textual 
para crear, listar, buscar, filtrar, completar y eliminar tareas, persistiendo la información en un archivo JSON local.

  ## Características principales
  - ✅ Alta de tareas con título, descripción y prioridad (baja, media, alta).
  - 📋 Listado completo mostrando el estado (pendiente/completada).
  - 🔍 Búsqueda por coincidencias en título o descripción.
  - 🎯 Filtro por prioridad para enfocarse en tareas críticas.
  - ✅ Marcado de tareas como completadas y eliminación permanente.
  - 🧼 Limpieza de pantalla tras cada operación para mantener la lectura ordenada.

  ## Requisitos previos
  - [SDK de .NET](https://dotnet.microsoft.com/en-us/download) 8.0 o superior. El proyecto está configurado para `net10.0`
  , por lo que necesitarás una versión reciente del SDK.

  ## Cómo ejecutar el proyecto
  1. Clona este repositorio y abre una terminal en la raíz.
  2. Entra al proyecto de consola:
     ```bash
     cd GestorTareas2
     ```
  3. Restaura dependencias (opcional, `dotnet run` lo hará automáticamente) y ejecuta:
     ```bash
     dotnet run
     ```
  4. Sigue las instrucciones del menú para interactuar con el gestor.

  > **Nota:** Los datos se guardan en un archivo `tasks.json` ubicado en la misma carpeta donde se genere el ejecutable (`
bin/Debug/net10.0/tasks.json` por defecto). Puedes respaldarlo o editarlo manualmente si necesitas migrar información.

  ## Flujo de uso
  1. Elige la opción deseada del menú (1-7).
  2. Ingresa los datos solicitados (títulos, descripciones o IDs numéricos).
  3. Observa el resultado en pantalla y continúa operando o selecciona `7` para salir.

  ## Diagrama de funcionamiento
  ```mermaid
  flowchart LR
      Program["Program.cs: Interfaz de consola"] --> TaskManager["TaskManager (Lógica de negocio)"]
      TaskManager --> Models["Modelos: WorkTask / Priority"]
      TaskManager --> RepoInterface{"IWorkTaskRepository"}
      RepoInterface --> JsonRepo["JsonTaskRepository (Implementación de persistencia)"]
      JsonRepo --> JsonHelper["JsonHelper (Serializa/Deserializa)"]
      JsonHelper --> Storage[("tasks.json: Almacenamiento local")]
  ```

  El diagrama muestra cómo la aplicación de consola orquesta las operaciones: la interfaz de usuario (`Program.cs`) delega
 la lógica al servicio `TaskManager`, el cual utiliza los modelos para manipular datos y se comunica exclusivamente mediante la
interfaz `IWorkTaskRepository`. La implementación `JsonTaskRepository` delega la persistencia en `JsonHelper`, que lee y escribe
 el archivo `tasks.json` donde se guardan las tareas.


