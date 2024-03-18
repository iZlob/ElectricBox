using Microsoft.AspNetCore.Connections;
using System.Text;
using System.Data.SQLite;
using System.Security.Cryptography;
//using Microsoft.Data.SqlClient;


namespace ElectricBox.Models
{
    public class GroundResistance
    {
        public Dictionary<string,int> groundResistance { get; }
        public List<Storage> climaticZoneFactors { get; }
        public List<Storage> factorsUseVerticalElectrodesLine { get; }
        public List<Storage> factorsUseVerticalElectrodesCircle { get; }
        public List<Storage> factorsUseHorizontalElectrodesLine { get; }
        public List<Storage> factorsUseHorizontalElectrodesCircle { get; }


        public GroundResistance()
        {
            groundResistance = new Dictionary<string,int>();
            climaticZoneFactors = new List<Storage>();
            factorsUseVerticalElectrodesLine = new List<Storage>();
            factorsUseVerticalElectrodesCircle = new List<Storage>();
            factorsUseHorizontalElectrodesLine = new List<Storage>();
            factorsUseHorizontalElectrodesCircle = new List<Storage>();

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
                                storage.ClimaticZone = reader.GetString(0);
                                storage.ClimaticFactorVert = reader.GetFloat(1);
                                storage.ClimaticFactorHor = reader.GetFloat(2);
                                climaticZoneFactors.Add(storage);
                            }
                        }
                    }

                    //достаем данные из таблицы коэффициентов использования вертикальных электродов в разомкнутом контуре
                    command.CommandText = "SELECT Ratio, ElectrodesCount, FactorUse FROM FactorsUseVerticalElectrodesLine;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.Ratio = reader.GetInt32(0);
                                storage.CountEl = reader.GetInt32(1);
                                storage.FactorUse = reader.GetFloat(2);
                                factorsUseVerticalElectrodesLine.Add(storage);
                            }
                        }
                    }

                    //достаем данные из таблицы коэффициентов использования вертикальных электродов в замкнутом контуре
                    command.CommandText = "SELECT Ratio, ElectrodesCount, FactorUse FROM FactorsUseVerticalElectrodesCircle;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.Ratio = reader.GetInt32(0);
                                storage.CountEl = reader.GetInt32(1);
                                storage.FactorUse = reader.GetFloat(2);
                                factorsUseVerticalElectrodesCircle.Add(storage);
                            }
                        }
                    }

                    //достаем данные из таблицы коэффициентов использования горизонтальных электродов в разомкнутом контуре
                    command.CommandText = "SELECT Ratio, ElectrodesCount, FactorUse FROM FactorsUseHorizontalElectodesLine;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.Ratio = reader.GetInt32(0);
                                storage.CountEl = reader.GetInt32(1);
                                storage.FactorUse = reader.GetFloat(2);
                                factorsUseHorizontalElectrodesLine.Add(storage);
                            }
                        }
                    }

                    //достаем данные из таблицы коэффициентов использования горизонтальных электродов в замкнутом контуре
                    command.CommandText = "SELECT Ratio, ElectrodesCount, FactorUse FROM FactorsUseHorizontalElectrodesCircle;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.Ratio = reader.GetInt32(0);
                                storage.CountEl = reader.GetInt32(1);
                                storage.FactorUse = reader.GetFloat(2);
                                factorsUseHorizontalElectrodesCircle.Add(storage);
                            }
                        }
                    }
                }
            }
        }
    }
}

