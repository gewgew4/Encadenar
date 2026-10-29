namespace Encadenar;

/// <summary>
/// Éxito o fallo como valor. Esta demo solo usa Result&lt;T&gt;;
/// la clase base guarda el estado común y las invariantes.
/// </summary>
public class Result
{
    protected Result(bool esExito, Error error)
    {
        // Armar un Result inválido es un bug de quien implementa el tipo, no un error de negocio: por eso throw.
        if (esExito && error != Error.Ninguno)
            throw new InvalidOperationException("Un resultado exitoso no puede contener un error.");
        if (!esExito && error == Error.Ninguno)
            throw new InvalidOperationException("Un resultado fallido debe contener un error.");

        EsExito = esExito;
        Error = error;
    }

    public bool EsExito { get; }
    public bool EsFallo => !EsExito;
    public Error Error { get; }

    // Éxito: hay valor y el error es el centinela, nunca null.
    public static Result<T> Exito<T>(T valor) => new(valor, true, Error.Ninguno);

    // Fallo: el valor queda en default y no se puede leer. El dato útil es Error.
    public static Result<T> Fallo<T>(Error error) => new(default, false, error);
}

/// <summary>Si salió bien, transporta T. Si salió mal, transporta un Error y T no existe.</summary>
public sealed class Result<T> : Result
{
    private readonly T? _valor;

    internal Result(T? valor, bool esExito, Error error) : base(esExito, error)
        => _valor = valor;

    // Leer el valor sin haber discriminado el resultado es un bug del llamador.
    // Los bugs se modelan con excepción: esto no contradice el Result, lo aplica.
    public T Valor => EsExito
        ? _valor!
        : throw new InvalidOperationException("No se puede acceder al valor de un resultado fallido.");

    // Permiten `return nombre.Trim();` y `return ClienteErrores.NombreVacio;`
    // sin escribir Exito(...) ni Fallo(...). El compilador elige según el tipo del operando.
    public static implicit operator Result<T>(T valor) => Exito(valor);
    public static implicit operator Result<T>(Error error) => Fallo<T>(error);
}
