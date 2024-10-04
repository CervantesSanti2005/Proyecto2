using System;
using System.IO;
using System.Data.SQLite;
using TagLib;
using System.Collections.Generic;

class Minero{
    private string directorio;
    private SQLiteConnection conexionBD;
    private List<string> ArchivoMp3;

    public Minero(string directorio){
        this.directorio = directorio;
        ArchivoMp3 = new List<string>();
    }

}
