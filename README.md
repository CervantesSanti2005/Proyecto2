# Proyecto2
Proyecto 2 de la materia Modelado y Programación
El proyecto está hecho en el builder .NET orientado a C#
Las pricipales bibliotecas que usé fueron TagLib y SQLite, para la interfaz gráfica use Avalonia, que es una variación de Windows Presentation Foundation, la cosa es que WPF funciona solo para windows así que usé avalonia

El funcionamiento es simple, se ejecuta con "dotnet run"

La interfaz tiene dos querys, uno para indicar la ruta del directorio en donde quieres que se mine la música y otro para realizar las búsquedas, el query de las búsquedas recibe cuatro filtros importantes:
    t:titulo de la cancion
    a:performer de la cancion
    g:género de la cancion
    y:año de la cancion

Es importante recalcar que si se pone espacio despues de los dos puntos del filtro, es decir : a:_Gustavo Cerati, el filtro fallará, la búsqueda correcta es a:Gustavo Cerati
