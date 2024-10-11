using System;
using System.Data.SQLite;

namespace BDMusica{
    public class Busqueda{

        private BaseDeDatos db;

        public Busqueda(BaseDeDatos baseDeDatos){
            db = baseDeDatos;
        }

        public List<string> Buscar(string queryUsuario){

            List<string> listaResultados = new List<string>();


            try{
                var filtros = queryUsuario.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                // seprar los filtros para recibir el formato
                string sql = @"
                        SELECT r.title, r.year, r.track, r.genre, p.name AS performer, a.name AS album
                        FROM rolas r
                        JOIN performers p ON r.id_performer = p.id_performer
                        JOIN albums a ON r.id_album = a.id_album
                        WHERE 1 = 1";


                var command = new SQLiteCommand();
                foreach (var filtro in filtros){
                    var partes = filtro.Split(':');
                    if(partes.Length == 2){
                        var clave = partes[0].Trim().ToLower();
                        var valor = partes[1].Trim().ToLower();

                        switch(clave){

                            case "t" : //titulo
                                sql += " AND LOWER(r.title) LIKE @titulo";
                                command.Parameters.AddWithValue("@titulo", "%" + valor + "%");
                                break;
                            case "a" : //artista
                                sql += "AND LOWER(p.name) LIKE @artista";
                                command.Parameters.AddWithValue("@artista", "%" + valor + "%");
                                break;

                            case "g" : //genero
                                sql += " AND LOWER(r.genre) LIKE @genero";
                                command.Parameters.AddWithValue("@genero", "%" + valor + "%");
                                break;
                            case "y" ://año
                                if(int.TryParse(valor, out int year)){
                                    sql += " AND r.year = @year";
                                    command.Parameters.AddWithValue("@year", year);
                                }
                                break;
                        }
                    }
                }
                command.CommandText = sql;
                command.Connection = db.connection;




                using (var reader = command.ExecuteReader()){
                    while(reader.Read()){

                    string resultado = $"{reader["title"]} - {reader["performer"]} ({reader["album"]}, {reader["year"]})";
                    listaResultados.Add(resultado);
                    }
                }
            }catch(Exception ex){
                Console.WriteLine($"Error durante la búsqueda: {ex.Message}");
                listaResultados.Add($"Error: {ex.Message}");
            }
            return listaResultados;
        }
    }
}
