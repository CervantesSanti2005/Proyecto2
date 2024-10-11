using System;
using System.IO;
using System.Data.SQLite;
using TagLib;
using System.Collections.Generic;
namespace BDMusica{
    public class Minero{
        private BaseDeDatos db;

        public Minero(BaseDeDatos database){
            db = database;
        }

        public List<string> minarDirectorio(string directoryPath){

            List<string> cancionesMinadas = new List<string>();

            if (!Directory.Exists(directoryPath)){
                Console.WriteLine($"El directorio {directoryPath} no existe.");
                return cancionesMinadas;
            }

            var archivoMP3 = Directory.GetFiles(directoryPath, "*.mp3", SearchOption.AllDirectories);


            foreach(var archivo in archivoMP3){
                string cancion = minarArchivo(archivo);
                if (!string.IsNullOrEmpty(cancion)){
                    cancionesMinadas.Add(cancion);
                }
            }
            return cancionesMinadas;
        }

        private string minarArchivo(string filePath){
            try{
                    if (!System.IO.File.Exists(filePath)){
                        Console.WriteLine($"El archivo {filePath} no existe.");
                        return null;
                    }
                    var archivo = TagLib.File.Create(filePath);

                    string title = archivo.Tag.Title ?? "Unkown";
                    string performer = archivo.Tag.FirstPerformer ?? "Unkown";
                    string album = archivo.Tag.Album ?? Path.GetDirectoryName(filePath);
                    int track = archivo.Tag.Track > 0 ? (int)archivo.Tag.Track : 0;
                    int year = archivo.Tag.Year > 0 ? (int)archivo.Tag.Year : DateTime.Now.Year;
                    string genre = archivo.Tag.FirstGenre ?? "Unknown";

                    int albumId = db.obtenerInsertarAlbum(album, Path.GetDirectoryName(filePath), year);
                    int performerId = db.obtenerInsertarArtista(performer);

                    db.insertarCancion(performerId, albumId, filePath, title, track, year, genre);

                     return $"{title} - {performer} ({album}, {year})";
            }catch (Exception ex){
                Console.WriteLine($"Error procesando el archivo {filePath}: {ex.Message}");
                return null;
            }
        }


    }
}
