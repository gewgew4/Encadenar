using Encadenar;

// Sin esto, la consola de Windows no muestra las tildes.
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Tres casos del ejercicio guiado. El segundo corta en el nombre; el tercero llega al email.
(string Nombre, string Email)[] pedidos =
[
    ("Ana Gómez", "ana@galeria.com"),       // éxito: corren nombre, email y el Map
    ("", "ana@galeria.com"),                // fallo en el nombre: el email ni se mira
    ("Ana Gómez", "esto-no-es-un-mail"),    // nombre ok, email inválido
];

foreach (var (nombre, email) in pedidos)
{
    // Match cierra la cadena en el borde. Las dos ramas devuelven string;
    // el dominio sigue devolviendo Result y no decide el texto de la consola.
    Console.WriteLine(ClienteGaleria.Crear(nombre, email).Match(
        siExito: c => $"Cliente {c.Nombre} <{c.Email}>",
        siFallo: e => e.Mensaje));
}
