# Encadenar

Solución mínima de la diapositiva 12, "Encadenar con Bind, Map y Match", de la clase "¿Excepción o Result?" (T123-22, UAI). Un solo proyecto de consola en .NET 9. No hay capas, CSV, repositorio ni tests: solo el cliente y la cadena.

## Abrir

1. Visual Studio 2022 17.12 o superior, con el SDK de .NET 9.
2. Abrir `Encadenar.slnx`.
3. F5. La salida tiene que ser exactamente:

```
Cliente Ana Gómez <ana@galeria.com>
El nombre del cliente es obligatorio.
El email 'esto-no-es-un-mail' no es válido.
```

Por consola: `dotnet run` desde esta carpeta.

## Qué hay en cada archivo

| Archivo | Rol |
|---|---|
| `Error.cs` | Código estable y mensaje. `Ninguno` es el centinela de éxito. |
| `Result.cs` | `Result<T>`, invariantes y conversiones implícitas. |
| `ResultExtensions.cs` | `Map`, `Bind` y `Match`. |
| `ClienteGaleria.cs` | `Crear` encadena las dos validaciones. |
| `Program.cs` | Los tres casos. `Match` cierra la cadena y arma el texto. |

Los comentarios del código marcan las líneas que deciden el encadenado.

## La cadena

`ClienteGaleria.Crear` valida el nombre y, solo si hubo éxito, valida el email y construye:

```csharp
ValidarNombre(nombre)
    .Bind(nombreValido => ValidarEmail(email)
        .Map(emailValido => new ClienteGaleria(nombreValido, emailValido)));
```

`Bind` se usa porque `ValidarEmail` puede fallar. `Map` se usa porque el `new` no valida. Con el nombre vacío, el email no se evalúa: queda un solo error. `Match` está en `Program.cs`, no en el dominio.
