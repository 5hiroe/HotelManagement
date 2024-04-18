#### CARUELLE Nathan, MALON Killian, I1 C2
# Hotel Management API Documentation

## Introduction

This API is designed for managing a hotel's operations including room bookings, customer management, and housekeeping services. It facilitates interaction between different hotel services such as reservations, customer management, and room maintenance.

## Configuration Initial

### Requirements

- .NET 8.0 SDK
- Any compatible SQL database (this setup uses SQLite)
- Properly configured `appsettings.json`

### Setup

1. Clone the repository to your local machine.
2. Ensure the connection string in `appsettings.json` is correct:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=hotelmanagement.db"
    }
    ```

3. Restore dependencies and update the database:

    ```bash
    dotnet restore
    dotnet ef database update
    ```

4. Start the application:

    ```bash
    dotnet run
    ```

## Authentication

JWT is used for securing the API. Ensure the JWT settings in `appsettings.json` are correct:

```json
"Jwt": {
    "Key": "V3ryC0mpl3xS3cr3tK3yTh4tY0uSh0uldN0tShare",
    "Issuer": "MyHotelManagementApp",
    "Audience": "HotelManagementUsers"
}
```

## Authentication

Users are authenticated using the `/api/auth/login` endpoint which issues a token on valid credentials.

## API Endpoints

### Room Operations

- **GET /Room/available**: Returns a list of available rooms for given dates.
- **POST /Room**: Adds a new room to the database.
- **PUT /Room/{id}**: Updates details of a specific room.
- **DELETE /Room/{id}**: Removes a room from the database.

### Reservation Operations

- **GET /Reservation**: Retrieves all reservations.
- **POST /Reservation**: Creates a new reservation with credit card validation.
- **PUT /Reservation/{id}**: Updates a reservation.
- **DELETE /Reservation/{id}**: Deletes a reservation.

### Customer Operations

- **GET /Customer**: Lists all customers.
- **POST /Customer**: Registers a new customer.
- **PUT /Customer/{id}**: Updates customer information.
- **DELETE /Customer/{id}**: Deletes a customer record.

### Housekeeping Operations

- **GET /Housekeeping/rooms-to-clean**: Lists all rooms that need cleaning.
- **POST /Housekeeping/mark-room-cleaned/{roomId}**: Marks a room as cleaned.

### Receptionist Operations

- **GET /Receptionist/available-rooms**: Fetches all available rooms for a specific period.
- **POST /Receptionist/check-in**: Checks in a customer.
- **POST /Receptionist/check-out**: Checks out a customer and marks the room for cleaning.

## Models

Detailed descriptions of the data models used (e.g., Room, Reservation, Customer).

## Services

Overview of services used in the application such as RoomService, ReservationService, and CustomerService.

## Project Configuration

Details about the project's configuration in `HotelManagment.csproj` and its dependencies.

## Swagger API Documentation

Access the Swagger UI for a visual representation of the API and to interact with its endpoints directly at:

```
http://localhost:<port>
```

