namespace ElectricBox.Models.Kontur
{
    public class Storage
    {
        public string climaticZone { get; set; }
        public float climaticFactorVert { get; set; }
        public float climaticFactorHor { get; set; }
        public int ratio { get; set; }
        public int countEl { get; set; }
        public float factorUse { get; set; }

        public Storage()
        {
            climaticZone = "";
            climaticFactorVert = 0;
            climaticFactorHor = 0;
            ratio = 0;
            countEl = 0;
            factorUse = 0;
        }
    }
}
