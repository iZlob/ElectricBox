namespace ElectricBox.Models.Cable
{
    public class Storage
    {
        public string method { get; set; }
        public float cut { get; set; }
        public int current { get; set; }

        public Storage()
        {
            method = "";
            cut = 0;
            current = 0;
        }
    }
}
