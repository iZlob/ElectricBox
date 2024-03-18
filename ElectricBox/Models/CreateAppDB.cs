using System.Data.SQLite;

using System;
using System.Data.SqlClient;

namespace ElectricBox.Models
{
    public class CreateAppDB
    {
        private SQLiteConnection _connection;
        private SQLiteCommand _command;
        public static string nameDB = "EBoxDB.sqlite";

        public CreateAppDB()
        {
            
        }

        public void CreateDB()
        {
            if (!File.Exists(nameDB))
            {
                SQLiteConnection.CreateFile(nameDB);
            }

            using (_connection = new SQLiteConnection($"Data Source={nameDB};"))
            {
                _connection.Open();

                using (_command = new SQLiteCommand())
                {
                    _command.Connection = _connection;

                    CreateGroundResistanceDbo("GroundResistance");
                    CreateClimaticZoneFactorsDbo("ClimaticZoneFactors");
                    CreateFactorsUseVerticalElectrodesLineDbo("FactorsUseVerticalElectrodesLine");
                    CreateFactorsUseVerticalElectrodesCircleDbo("FactorsUseVerticalElectrodesCircle");
                    CreateFactorsUseHorizontalElectrodesInLineDbo("FactorsUseHorizontalElectodesLine");
                    CreateFactorsUseHorizontalElectrodesInCircleDbo("FactorsUseHorizontalElectrodesCircle");
                }
            }

        }

        private void CreateGroundResistanceDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, GroundName nvarchar(15), Resistance INTEGER)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (GroundName, Resistance) VALUES " +
                                                                  "('Песок сухой', 1500), " +
                                                                  "('Песок влажный', 400), " +
                                                                  "('Супесок', 250), " +
                                                                  "('Суглинок', 110), " +
                                                                  "('Глина', 45), " +
                                                                  "('Чернозем', 30), " +
                                                                  "('Торф', 20), " +
                                                                  "('Каменистая глина', 100);";
            _command.ExecuteNonQuery();
        }

        private void CreateClimaticZoneFactorsDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Zone VARCHAR(10), VerticalElectrod FLOAT, HorizontalElectrod FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Zone, VerticalElectrod, HorizontalElectrod) VALUES " +
                                                          "('Зона I', 1.9, 5.75), " +
                                                          "('Зона II', 1.75, 4.0), " +
                                                          "('Зона III', 1.5, 2.25), " +
                                                          "('Зона IV', 1.3, 1.75);";
            _command.ExecuteNonQuery();
        }

        private void CreateFactorsUseVerticalElectrodesLineDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Ratio INTEGER, ElectrodesCount INTEGER, FactorUse FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Ratio, ElectrodesCount, FactorUse) VALUES " +
                                                          "(1, 2, 0.87), (1, 3, 0.80), (1, 5, 0.72), (1, 10, 0.62), (1, 15, 0.56), (1, 20, 0.50), " +
                                                          "(2, 2, 0.92), (2, 3, 0.88), (2, 5, 0.83), (2, 10, 0.77), (2, 15, 0.73), (2, 20, 0.70), " +
                                                          "(3, 2, 0.95), (3, 3, 0.92), (3, 5, 0.88), (3, 10, 0.83), (3, 15, 0.80), (3, 20, 0.77);";
            _command.ExecuteNonQuery();
        }

        private void CreateFactorsUseVerticalElectrodesCircleDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Ratio INTEGER, ElectrodesCount INTEGER, FactorUse FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Ratio, ElectrodesCount, FactorUse) VALUES " +
                                                          "(1, 4, 0.72), (1, 6, 0.65), (1, 10, 0.58), (1, 20, 0.50), (1, 50, 0.44), (1, 60, 0.42), (1, 100, 0.39), " +
                                                          "(2, 4, 0.80), (2, 6, 0.75), (2, 10, 0.71), (2, 20, 0.66), (2, 50, 0.61), (2, 60, 0.58), (2, 100, 0.55), " +
                                                          "(3, 4, 0.86), (3, 6, 0.82), (3, 10, 0.78), (3, 20, 0.73), (3, 50, 0.69), (3, 60, 0.67), (3, 100, 0.65);";
            _command.ExecuteNonQuery();
        }

        private void CreateFactorsUseHorizontalElectrodesInLineDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Ratio INTEGER, ElectrodesCount INTEGER, FactorUse FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Ratio, ElectrodesCount, FactorUse) VALUES " +
                                                          "(1, 4, 0.77), (1, 5, 0.74), (1, 8, 0.67), (1, 10, 0.62), (1, 20, 0.42), (1, 30, 0.31), (1, 50, 0.21), (1, 65, 0.20), " +
                                                          "(2, 4, 0.89), (2, 5, 0.86), (2, 8, 0.79), (2, 10, 0.75), (2, 20, 0.56), (2, 30, 0.46), (2, 50, 0.36), (2, 65, 0.34), " +
                                                          "(3, 4, 0.92), (3, 5, 0.90), (3, 8, 0.85), (3, 10, 0.82), (3, 20, 0.68), (3, 30, 0.58), (3, 50, 0.49), (3, 65, 0.47);";
            _command.ExecuteNonQuery();
        }

        private void CreateFactorsUseHorizontalElectrodesInCircleDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Ratio INTEGER, ElectrodesCount INTEGER, FactorUse FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Ratio, ElectrodesCount, FactorUse) VALUES " +
                                                          "(1, 4, 0.45), (1, 6, 0.40), (1, 8, 0.36), (1, 10, 0.34), (1, 20, 0.27), (1, 30, 0.24), (1, 50, 0.21), (1, 70, 0.20), (1, 100, 0.10), " +
                                                          "(2, 4, 0.55), (2, 6, 0.48), (2, 8, 0.48), (2, 10, 0.40), (2, 20, 0.32), (2, 30, 0.30), (2, 50, 0.28), (2, 70, 0.26), (2, 100, 0.24), " +
                                                          "(3, 4, 0.70), (3, 6, 0.64), (3, 8, 0.60), (3, 10, 0.56), (3, 20, 0.45), (3, 30, 0.41), (3, 50, 0.37), (3, 70, 0.35), (3, 100, 0.33);";
            _command.ExecuteNonQuery();
        }
    }
}