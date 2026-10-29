namespace Encadenar;

/// <summary>
/// Map y Bind arman la cadena y devuelven otro Result.
/// Match la cierra. Ninguna atrapa excepciones: si el lambda lanza, el throw sale de la cadena.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Transforma el valor de un éxito. La función NO puede fallar: TIn → TOut, no TIn → Result.
    /// Análogo de Select. Si se usa para un paso que devuelve Result, queda Result&lt;Result&lt;T&gt;&gt;.
    /// </summary>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> resultado, Func<TIn, TOut> transformar) =>
        resultado.EsExito
            ? Result.Exito(transformar(resultado.Valor))   // corre la función y vuelve a envolver
            : Result.Fallo<TOut>(resultado.Error);         // mismo Error, otro TOut; la función no corre

    /// <summary>
    /// Encadena un paso que TAMBIÉN puede fallar y aplana el Result, para no anidar.
    /// Análogo de SelectMany. Si ya hay fallo, no llama a siguiente: cortocircuito.
    /// </summary>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> resultado, Func<TIn, Result<TOut>> siguiente) =>
        resultado.EsExito
            ? siguiente(resultado.Valor)                   // devuelve el Result de siguiente tal cual
            : Result.Fallo<TOut>(resultado.Error);         // propaga el primer error y corta la cadena

    /// <summary>
    /// Consume el Result. No devuelve otro Result: las dos ramas son obligatorias
    /// y tienen que devolver el mismo TOut. Ignorar el fallo no compila.
    /// Va en el borde (la consola), no en el dominio.
    /// </summary>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> resultado, Func<TIn, TOut> siExito, Func<Error, TOut> siFallo) =>
        resultado.EsExito ? siExito(resultado.Valor) : siFallo(resultado.Error);
}
