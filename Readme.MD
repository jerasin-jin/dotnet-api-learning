## Application Overall

### Technology To Use

    * ASP .NET API 7.0
    * MySql
    * Dockerfile
    * Docker-compose
    * Makefile

### Command To Create New or Edit DataBase

#### Create Migration File

```console
$ dotnet ef migrations add ${comment}
```

#### Update DataBase From Migration File

```console
$ dotnet ef database update
```

### How To Run

```console
$ make dev
```
