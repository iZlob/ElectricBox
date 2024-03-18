namespace ElectricBox.Models
{
    public class Storage
    {
        public string ClimaticZone { get; set; }
        public float ClimaticFactorVert { get; set; }
        public float ClimaticFactorHor { get; set; }
        public int Ratio { get; set; }
        public int CountEl { get; set; }
        public float FactorUse { get; set; }

        public Storage()
        {
            ClimaticZone = "";
            ClimaticFactorVert = 0;
            ClimaticFactorHor = 0;
            Ratio = 0;
            CountEl = 0;
            FactorUse = 0;
        }
    }
}
