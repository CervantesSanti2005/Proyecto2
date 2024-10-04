using System;

namespace BDMusica{
    static class Program{
        static void Main(string[] args){
                // Inicializar la base de datos (se creará en el archivo 'music.db')
            string dbPath = "music.db";

            var bd = new BaseDeDatos();
            bd.Database(dbPath);

            // Crear las tablas y asegurarse de que están listas
            bd.crearTabla();
            bd.insertarDefault();

            // Inicializar el minero de MP3 y comenzar a minar el directorio
            var minero = new Minero(bd);


            string directoryPath = @"/home/santiago/Downloads/";  // Cambia esto a la ruta de los archivos MP3
            Console.WriteLine("Minando archivos en: " + directoryPath);

            minero.minarDirectorio(directoryPath);

            Console.WriteLine("Listo");
        }
    }
}
