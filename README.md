## Installation

To run the project, you must have Docker and docker-compose.

We run the project with the command:

```
docker-compose up --build
```

API is running on PORT 8080. For your convenience, a swagger client is provided
http://localhost:8080/swagger/index.html

## Tests

Before start testing our application we need to provide PostgreSql instance:
```
docker-compose up postgres
```