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

        public void minarDirectorio(string directoryPath){
            var archivoMP3 = Directory.GetFiles(directoryPath, "*.mp3", SearchOption.AllDirectories);

            foreach(var archivo in archivoMP3){
                minarArchivo(archivo);
            }
        }

        private void minarArchivo(string filePath){
            try{
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
            }catch (Exception ex){
                Console.WriteLine($"Error procesando el archivo {filePath}: {ex.Message}");
            }
        }


    }
}
