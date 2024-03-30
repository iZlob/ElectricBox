using System.Data.SQLite;


namespace ElectricBox.Models.Cable
{
    public class Cable
    {

        public Dictionary<string, float> unitCableResistance { get; }
        public List<Storage> cuprum { get; }
        public List<Storage> aluminium { get; }
        public List<string> methods { get; }
        public List<float> cuts { get; }

        public Cable()
        {
            unitCableResistance = new Dictionary<string, float>();
            cuprum = new List<Storage>();
            aluminium = new List<Storage>();
            methods = new List<string>();
            cuts = new List<float>();

            extractData();//извлекаем данные из БД
        }

        private void extractData()
        {
            using (var connect = new SQLiteConnection($"Data Source={CreateAppDB.nameDB};"))//подключаемся к БД
            {
                connect.Open();

                using (var command = connect.CreateCommand())//создаем класс команды
                {
                    //достаем данные из таблицы удельных сопротивлений проводников
                    command.CommandText = "SELECT Cable, UnitResistance FROM UnitCableResistance;";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                unitCableResistance.Add(reader.GetString(0), reader.GetFloat(1));
                            }
                        }
                    }

                    //достаем данные для таблицы допустимых токов для медных проводников
                    command.CommandText = "SELECT Method, Cut, Current " +
                                          "FROM Cuts, Methods, CuprumCurrents " +
                                          "WHERE Cuts.Id=CuprumCurrents.cut_id AND Methods.Id=CuprumCurrents.method_id;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.method = reader.GetString(0);
                                storage.cut = reader.GetFloat(1);
                                storage.current = reader.GetInt32(2);
                                cuprum.Add(storage);
                            }
                        }
                    }

                    //достаем данные для таблицы допустимых токов для алюминиевых проводников
                    command.CommandText = "SELECT Method, Cut, Current " +
                                          "FROM Cuts, Methods, AluminiumCurrents " +
                                          "WHERE Cuts.Id=AluminiumCurrents.cut_id AND Methods.Id=AluminiumCurrents.method_id;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                Storage storage = new Storage();
                                storage.method = reader.GetString(0);
                                storage.cut = reader.GetFloat(1);
                                storage.current = reader.GetInt32(2);
                                aluminium.Add(storage);
                            }
                        }
                    }

                    //достаем данные для перечня способов прокладки кабеля
                    command.CommandText = "SELECT Method FROM Methods;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                methods.Add(reader.GetString(0));
                            }
                        }
                    }

                    //достаем данные для списка стандартных сечений
                    command.CommandText = "SELECT Cut FROM Cuts;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                cuts.Add(reader.GetFloat(0));
                            }
                        }
                    }
                }        
            }
        }
    }
}

