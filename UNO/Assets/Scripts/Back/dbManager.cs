using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityNpgsql;

//Clase que se encarga de realizar la conexión con la base de datos
public class dbManager : MonoBehaviour
{

    static private string postgresConfig = "Database=rjwphrqg; " +
            "Username=rjwphrqg; " +
            "Host=balarama.db.elephantsql.com; " +
            "Port=5432; " +
            "Password=VO8gx2CjTr4hhoCG4gPZIqA0ARi1Tzjn; ";


    public static NpgsqlConnection getDBConnection()
    {
        NpgsqlConnection conn = new NpgsqlConnection(postgresConfig);
        //PARA PRUEBAS LOCALES: "Server=127.0.0.1;User Id=postgres;Password=postgres;Database=postgres;"
        conn.Open();
        return conn;
    }
}
