using System.Data.SQLite;


namespace ElectricBox.Models.Kontur
{
    public class GroundResistance
    {
        public Dictionary<string, int> groundResistance { get; }
        public List<Storage> climaticZoneFactors { get; }
        public List<Storage> factorsUseVerticalElectrodes { get; }
        public List<Storage> factorsUseHorizontalElectrodes { get; }


        public GroundResistance()
        {
            groundResistance = new Dictionary<string, int>();
            climaticZoneFactors = new List<Storage>();
            factorsUseVerticalElectrodes = new List<Storage>();
            factorsUseHorizontalElectrodes = new List<Storage>();

            extractData();//извлекаем данные из БД
        }

        private void extractData()
        {
            using (var connect = new SQLiteConnection($"Data Source={CreateAppDB.nameDB};"))//подключаемся к БД
            {
                connect.Open();

                using (var command = connect.CreateCommand())//создаем класс команды
                {
                    //достаем данные из таблицы удельных сопротивлений грунтов
                    command.CommandText = "SELECT GroundName, Resistance FROM GroundResistance;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                groundResistance.Add(reader.GetString(0), reader.GetInt32(1));
                            }
                        }
                    }

                    //достаем данные из таблицы климатических зон
                    command.CommandText = "SELECT Zone, VerticalElectrod, HorizontalElectrod FROM ClimaticZoneFactors;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.climaticZone = reader.GetString(0);
                                storage.climaticFactorVert = reader.GetFloat(1);
                                storage.climaticFactorHor = reader.GetFloat(2);
                                climaticZoneFactors.Add(storage);
                            }
                        }
                    }

                    //достаем данные из таблицы коэффициентов использования вертикальных электродов
                    command.CommandText = "SELECT Ratio, ElectrodesCount, FactorUse FROM FactorsUseVerticalElectrodes;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.ratio = reader.GetInt32(0);
                                storage.countEl = reader.GetInt32(1);
                                storage.factorUse = reader.GetFloat(2);
                                factorsUseVerticalElectrodes.Add(storage);
                            }
                        }
                    }

                    //достаем данные из таблицы коэффициентов использования горизонтальных электродов
                    command.CommandText = "SELECT Ratio, ElectrodesCount, FactorUse FROM FactorsUseHorizontalElectodes;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.ratio = reader.GetInt32(0);
                                storage.countEl = reader.GetInt32(1);
                                storage.factorUse = reader.GetFloat(2);
                                factorsUseHorizontalElectrodes.Add(storage);
                            }
                        }
                    }
                }
            }
        }
    }
}

