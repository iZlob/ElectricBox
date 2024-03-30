using System.Data.SQLite;

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


                using (_connection = new SQLiteConnection($"Data Source={nameDB};"))
                {
                    _connection.Open();

                    using (_command = new SQLiteCommand())
                    {
                        _command.Connection = _connection;

                        CreateGroundResistanceDbo("GroundResistance");
                        CreateClimaticZoneFactorsDbo("ClimaticZoneFactors");
                        CreateFactorsUseVerticalElectrodesDbo("FactorsUseVerticalElectrodes");
                        CreateFactorsUseHorizontalElectrodesDbo("FactorsUseHorizontalElectodes");
                        CreateUnitCableResistanceDbo("UnitCableResistance");
                        CreateCutsDbo("Cuts");
                        CreateTypeMethodsDbo("Methods");
                        CreateCuprumCurrentsDbo("CuprumCurrents", "Cuts", "Methods");
                        CreateAluminiumCurrentsDbo("AluminiumCurrents", "Cuts", "Methods");
                    }
                }
            }
        }

        private void CreateGroundResistanceDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, GroundName TEXT, Resistance INTEGER)";
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
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Zone TEXT, VerticalElectrod FLOAT, HorizontalElectrod FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Zone, VerticalElectrod, HorizontalElectrod) VALUES " +
                                                          "('Зона I', 1.9, 5.75), " +
                                                          "('Зона II', 1.75, 4.0), " +
                                                          "('Зона III', 1.5, 2.25), " +
                                                          "('Зона IV', 1.3, 1.75);";
            _command.ExecuteNonQuery();
        }

        private void CreateFactorsUseVerticalElectrodesDbo(string nameDbo)
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

        private void CreateFactorsUseHorizontalElectrodesDbo(string nameDbo)
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

        private void CreateUnitCableResistanceDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Cable TEXT, UnitResistance FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Cable, UnitResistance) VALUES " +
                                                                  "('Медь', 0.0175), " +
                                                                  "('Алюминий', 0.028), " +
                                                                  "('Серебро', 0.016), " +
                                                                  "('Золото', 0.024), " +
                                                                  "('Вольфрам', 0.055), " +
                                                                  "('Железо', 0.1), " +
                                                                  "('Нихром', 1.1), " +
                                                                  "('Никелин', 0.4);";
            _command.ExecuteNonQuery();
        }

        private void CreateCutsDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Cut FLOAT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Cut) VALUES " +
                 "(0.5), (0.75), (1), (1.2), (1.5), (2), (2.5), (3), (4), (5), (6), (8), (10), (16), (25), (35), (50), (70), (95), (120), (150), (185), (240), (300), (400);";
            _command.ExecuteNonQuery();
        }

        private void CreateTypeMethodsDbo(string nameDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Method TEXT)";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Method) VALUES " +
                 "('Открыто'), ('2-х 1 жильных'), ('3-х 1 жильных'), ('4-х 1 жильных'), ('1-го двухжильного'), ('1-го трехжильного');";
            _command.ExecuteNonQuery();
        }

        private void CreateCuprumCurrentsDbo(string nameDbo, string cutDbo, string methodDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} " +
                $"(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"Current INTEGER, " +
                $"cut_id INTEGER REFERENCES {cutDbo} (Id), " +
                $"method_id INTEGER REFERENCES {methodDbo} (Id));";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Current, cut_id, method_id) VALUES " +
                 "(11, 1, 1), (15, 2, 1), (17, 3, 1), (20, 4, 1), (23, 5, 1), (26, 6, 1), (30, 7, 1), (34, 8, 1), (41, 9, 1), (46, 10, 1), (50, 11, 1), " +
                 "(62, 12, 1), (80, 13, 1), (100, 14, 1), (140, 15, 1), (170, 16, 1), (215, 17, 1), (270, 18, 1), (330, 19, 1), (385, 20, 1), (440, 21, 1), " +
                 "(510, 22, 1), (605, 23, 1), (695, 24, 1), (830, 25, 1), (16, 3, 2), (18, 4, 2), (19, 5, 2), (24, 6, 2), (27, 7, 2), (32, 8, 2), (38, 9, 2), " +
                 "(42, 10, 2), (46, 11, 2), (54, 12, 2), (70, 13, 2), (85, 14, 2), (115, 15, 2), (135, 16, 2), (185, 17, 2), (225, 18, 2), (275, 19, 2), " +
                 "(315, 20, 2), (360, 21, 2), (15, 3, 3), (16, 4, 3), (17, 5, 3), (22, 6, 3), (25, 7, 3), (28, 8, 3), (35, 9, 3), (39, 10, 3), (42, 11, 3), " +
                 "(51, 12, 3), (60, 13, 3), (80, 14, 3), (100, 15, 3), (125, 16, 3), (170, 17, 3), (210, 18, 3), (255, 19, 3), (290, 20, 3), (330, 21, 3), " +
                 "(14, 3, 4), (15, 4, 4), (16, 5, 4), (20, 6, 4), (25, 7, 4), (26, 8, 4), (30, 9, 4), (34, 10, 4), (40, 11, 4), (46, 12, 4), (50, 13, 4), " +
                 "(75, 14, 4), (90, 15, 4), (115, 16, 4), (150, 17, 4), (185, 18, 4), (225, 19, 4), (260, 20, 4), (15, 3, 5), (16, 4, 5), (18, 5, 5), (23, 6, 5), " +
                 "(25, 7, 5), (28, 8, 5), (32, 9, 5), (37, 10, 5), (40, 11, 5), (48, 12, 5), (55, 13, 5), (80, 14, 5), (100, 15, 5), (125, 16, 5), (160, 17, 5), " +
                 "(195, 18, 5), (245, 19, 5), (295, 20, 5), (14, 4, 6), (15, 5, 6), (19, 6, 6), (21, 7, 6), (24, 8, 6), (27, 9, 6), (31, 10, 6), (34, 11, 6), " +
                 "(43, 12, 6), (50, 13, 6), (70, 14, 6), (85, 15, 6), (100, 16, 6), (135, 17, 6), (175, 18, 6), (215, 19, 6), (250, 20, 6);";
            _command.ExecuteNonQuery();
        }

        private void CreateAluminiumCurrentsDbo(string nameDbo, string cutDbo, string methodDbo)
        {
            _command.CommandText = $"DROP TABLE IF EXISTS {nameDbo};";
            _command.ExecuteNonQuery();
            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {nameDbo} " +
                $"(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"Current INTEGER, " +
                $"cut_id INTEGER REFERENCES {cutDbo} (Id), " +
                $"method_id INTEGER REFERENCES {methodDbo} (Id));";
            _command.ExecuteNonQuery();
            _command.CommandText = $"INSERT INTO {nameDbo} (Current, cut_id, method_id) VALUES " +
                 "(21, 6, 1), (24, 7, 1), (27, 8, 1), (32, 9, 1), (36, 10, 1), (39, 11, 1), (46, 12, 1), (60, 13, 1), (75, 14, 1), (105, 15, 1), (130, 16, 1), " +
                 "(165, 17, 1), (210, 18, 1), (255, 19, 1), (295, 20, 1), (340, 21, 1), (390, 22, 1), (465, 23, 1), (535, 24, 1), (645, 25, 1), " +
                 "(19, 6, 2), (20, 7, 2), (24, 8, 2), (28, 9, 2), (32, 10, 2), (36, 11, 2), (43, 12, 2), (50, 13, 2), (60, 14, 2), (85, 15, 2), (100, 16, 2), " +
                 "(140, 17, 2), (175, 18, 2), (215, 19, 2), (245, 20, 2), (275, 21, 2), (18, 6, 3), (19, 7, 3), (22, 8, 3), (28, 9, 3), (30, 10, 3), (32, 11, 3), " +
                 "(40, 12, 3), (47, 13, 3), (60, 14, 3), (80, 15, 3), (95, 16, 3), (130, 17, 3), (165, 18, 3), (200, 19, 3), (220, 20, 3), (255, 21, 3), " +
                 "(15, 6, 4), (19, 7, 4), (21, 8, 4), (23, 9, 4), (27, 10, 4), (30, 11, 4), (37, 12, 4), (39, 13, 4), (55, 14, 4), (70, 15, 4), (85, 16, 4), " +
                 "(120, 17, 4), (140, 18, 4), (175, 19, 4), (200, 20, 4), (17, 6, 5), (19, 7, 5), (22, 8, 5), (25, 9, 5), (28, 10, 5), (31, 11, 5), (38, 12, 5), " +
                 "(42, 13, 5), (60, 14, 5), (75, 15, 5), (95, 16, 5), (125, 17, 5), (150, 18, 5), (190, 19, 5), (230, 20, 5), (14, 6, 6), (16, 7, 6), (18, 8, 6), " +
                 "(21, 9, 6), (24, 10, 6), (26, 11, 6), (32, 12, 6), (38, 13, 6), (55, 14, 6), (65, 15, 6), (75, 16, 6), (105, 17, 6), (135, 18, 6), (165, 19, 6), " +
                 "(190, 20, 6);";
            _command.ExecuteNonQuery();
        }
    }
}