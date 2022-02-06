
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CountriesLinQ
{
    public class DataBase
    {
        
            private MySqlConnection db;
            private MySqlCommand command;
            public static class ConnectionString
            {
                private static string _server;
                private static string _db;
                private static string _user;
                private static string _password;

                public static string Init(string path)
                {
                    using var file = new StreamReader(path);
                    var temp = file.ReadLine();

                    var config = temp.Split('|');
                    _server = config[0];
                    _db = config[1];
                    _user = config[2];
                    _password = config[3];

                    return $"Server={_server};Database={_db};Uid={_user};Pwd={_password}";
                }
            }

            public DataBase()
            {
                var connectionString = ConnectionString.Init(@"db_connect.ini");
                db = new MySqlConnection(connectionString);
                command = new MySqlCommand {Connection = db};
            }

            public void Open() => db.Open();

            public void Close() => db.Close();
            
            public List<WorldParts> WorldPartsList()
            {
                Open();
                var list = new List<WorldParts>();
                var sql = @"SELECT id_country, country_name,capital_name, world_parts_name, country_area, country_population
                        FROM tab_country
                        JOIN tab_capital 
                            ON tab_country.id_country = tab_capital.id_country
                        JOIN tab_world_parts 
                            ON tab_world_parts.world_parts_id = tab_country.world_parts_id ;";
                command.CommandText = sql;
                var res = command.ExecuteReader();
                if (!res.HasRows) return null;

                while (res.Read())
                {
                    var id_country = res.GetUInt32("id_country");
                    var country_name = res.GetString("country_name");
                    var capital_name = res.GetString("capital_name");
                    var world_parts_name = res.GetString("world_parts_name");
                    var country_area = res.GetUInt32("country_area");
                    var country_population = res.GetUInt32("country_population");
                    
                    list.Add(new WorldParts
                    {
                        id_country = id_country, country_name = country_name,
                        capital_name = capital_name, world_parts_name = world_parts_name,
                        country_area = country_area, country_population = country_population
                    });
                }

                Close();

                return list;
            }

    }
}