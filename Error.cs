namespace Encadenar;

/// <summary>
/// Error de negocio: código estable para tests y lógica, mensaje para el usuario.
/// Record: inmutable e igualdad por valor, así Assert.Equal compara el código y el texto.
/// </summary>
public sealed record Error(string Codigo, string Mensaje)
{
    // Centinela de "no hay error". No es un error de negocio: éxito lo lleva y un fallo no puede usarlo.
    public static readonly Error Ninguno = new(string.Empty, string.Empty);
}
