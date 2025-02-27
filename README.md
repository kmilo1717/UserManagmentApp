# .NET API - User Managment APP

Esta simple app fue desarrollada con .NET sin ORM usando Ada


Requirements:
- .NET 9
- SQL Server
- SSMS
## Instalation and configurarion

Clone this repository

```bash
git clone https://github.com/kmilo1717/UserManagmentApp.git
cd UserManagmentApp/
```

En tu SSMS, conectate y ejecuta el script que se encuentra en el proyecto como: user_managment_db.sql

### Instalacion de dependencias

```bash
dotnet restore
dotnet build
```

## Development server

Para ejecutar el servidor ejecuta:

```bash
dotnet run
```
Una vez que ya lo ejecutes abre el navegador en  `http://localhost:5198`. 

## Endpoints

- GET http://localhost:5198/api/personas retornara los usuarios con sus telefonos, correos y direcciones.
- POST http://localhost:5198/api/personas:
  Ejemplo de body:
  ```bash
    {
      "documento": "5544322D",
      "nombres": "Ana",
      "apellidos": "Martínez",
      "fechaNacimiento": "1995-07-10T00:00:00",
        "telefonos": [
            { "numero": "3222426272" },
            { "numero": "3237776666" }
        ],
        "correos": [
            { "email": "ana.martinez@example.com" }
        ],
        "direcciones": [
            { "direccionFisica": "Carrera 45 #10-20, Bogota" }
        ]
    }
  ```
