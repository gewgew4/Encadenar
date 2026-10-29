using System.Net.Mail;

namespace Encadenar;

public static class ClienteErrores
{
    // Sin parámetros: una sola instancia para todos los nombres vacíos. Los tests comparan esta referencia.
    public static readonly Error NombreVacio =
        new("Cliente.NombreVacio", "El nombre del cliente es obligatorio.");

    // Lleva el texto recibido: cada email inválido es un Error distinto, con el mismo código.
    public static Error EmailInvalido(string email) =>
        new("Cliente.EmailInvalido", $"El email '{email}' no es válido.");
}

public sealed class ClienteGaleria
{
    // Privado: la única forma de obtener un cliente es Crear, que ya validó.
    private ClienteGaleria(string nombre, string email)
    {
        Nombre = nombre;
        Email = email;
    }

    public string Nombre { get; }
    public string Email { get; }

    // Firma honesta: Result avisa que crear puede fallar. El constructor no lo diría.
    public static Result<ClienteGaleria> Crear(string nombre, string email) =>
        ValidarNombre(nombre)
            // Bind: ValidarEmail puede fallar. Si el nombre ya falló, este lambda no corre.
            .Bind(nombreValido => ValidarEmail(email)
                // Map: el new no valida. Transforma dos strings ya válidos en la entidad.
                .Map(emailValido => new ClienteGaleria(nombreValido, emailValido)));

    private static Result<string> ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return ClienteErrores.NombreVacio;   // conversión implícita Error → Result<string>

        return nombre.Trim();                    // conversión implícita string → Result<string>
    }

    private static Result<string> ValidarEmail(string email)
    {
        // Patrón Try: "no es un email" es un fallo esperado, así que no se usa el Parse que lanza.
        if (!MailAddress.TryCreate(email, out var direccion))
            return ClienteErrores.EmailInvalido(email);

        return direccion.Address;
    }
}
