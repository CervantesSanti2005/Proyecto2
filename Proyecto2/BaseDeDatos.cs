using System;
using System.Data.SQLite;

public class BaseDeDatos{
    public SQLiteConnection connection { get; private set; }

    public void Database(string dbPath){
        connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
        connection.Open();
    }

    public void CreateTables(){
        string sql = @"
            CREATE TABLE IF NOT EXISTS types (
                id_type INTEGER PRIMARY KEY,
                description TEXT
            );

            CREATE TABLE IF NOT EXISTS performers (
                id_performer INTEGER PRIMARY KEY,
                id_type INTEGER,
                name TEXT,
                FOREIGN KEY (id_type) REFERENCES types(id_type)
            );

            CREATE TABLE IF NOT EXISTS persons (
                id_person INTEGER PRIMARY KEY,
                stage_name TEXT,
                real_name TEXT,
                birth_date TEXT,
                death_date TEXT
            );

            CREATE TABLE IF NOT EXISTS groups (
                id_group INTEGER PRIMARY KEY,
                name TEXT,
                start_date TEXT,
                end_date TEXT
            );

            CREATE TABLE IF NOT EXISTS in_group (
                id_person INTEGER,
                id_group INTEGER,
                PRIMARY KEY (id_person, id_group),
                FOREIGN KEY (id_person) REFERENCES persons(id_person),
                FOREIGN KEY (id_group) REFERENCES groups(id_group)
            );

            CREATE TABLE IF NOT EXISTS albums (
                id_album INTEGER PRIMARY KEY,
                path TEXT,
                name TEXT,
                year INTEGER
            );

            CREATE TABLE IF NOT EXISTS rolas (
                id_rola INTEGER PRIMARY KEY,
                id_performer INTEGER,
                id_album INTEGER,
                path TEXT,
                title TEXT,
                track INTEGER,
                year INTEGER,
                genre TEXT,
                FOREIGN KEY (id_performer) REFERENCES performers(id_performer),
                FOREIGN KEY (id_album) REFERENCES albums(id_album)
            );
        ";

        using (var command = new SQLiteCommand(sql, connection)){
            command.ExecuteNonQuery();
        }
    }

    public void InsertDefaultData(){
        string sql = @"
            INSERT OR IGNORE INTO types (id_type, description) VALUES (0, 'Person');
            INSERT OR IGNORE INTO types (id_type, description) VALUES (1, 'Group');
            INSERT OR IGNORE INTO types (id_type, description) VALUES (2, 'Unknown');
        ";

        using (var command = new SQLiteCommand(sql, connection)){
            command.ExecuteNonQuery();
        }
    }
    public int GetOrInsertAlbum(string albumName, string path, int year){
        // Verificar si el álbum ya existe
        string selectAlbumSql = "SELECT id_album FROM albums WHERE name = @name";
        using (var command = new SQLiteCommand(selectAlbumSql, connection)){
            command.Parameters.AddWithValue("@name", albumName);
            var result = command.ExecuteScalar();

            if (result != null){
                // Retornar el ID si el álbum ya existe
                return Convert.ToInt32(result);
            }else{
                // Insertar el álbum si no existe
                string insertAlbumSql = @"
                    INSERT INTO albums (name, path, year)
                    VALUES (@name, @path, @year);
                    SELECT last_insert_rowid();
                ";

                using (var insertCommand = new SQLiteCommand(insertAlbumSql, connection)){
                    insertCommand.Parameters.AddWithValue("@name", albumName);
                    insertCommand.Parameters.AddWithValue("@path", path);
                    insertCommand.Parameters.AddWithValue("@year", year);

                    // Retornar el ID del nuevo álbum insertado
                    return Convert.ToInt32(insertCommand.ExecuteScalar());
                }
            }
        }
    }

    public int GetOrInsertPerformer(string performerName){

        string selectPerformerSql = "SELECT id_performer FROM performers WHERE name = @name";
        using (var command = new SQLiteCommand(selectPerformerSql, connection))
        {
            command.Parameters.AddWithValue("@name", performerName);
            var result = command.ExecuteScalar();

            if (result != null)
            {
                // Retornar el ID si el intérprete ya existe
                return Convert.ToInt32(result);
            }
            else
            {
                // Insertar el intérprete si no existe
                string insertPerformerSql = @"
                    INSERT INTO performers (name, id_type)
                    VALUES (@name, 2);  -- Tipo 'Unknown' por defecto
                    SELECT last_insert_rowid();
                ";

                using (var insertCommand = new SQLiteCommand(insertPerformerSql, connection))
                {
                    insertCommand.Parameters.AddWithValue("@name", performerName);
                    // Retornar el ID del nuevo intérprete insertado
                    return Convert.ToInt32(insertCommand.ExecuteScalar());
                }
            }
        }
    }

}
