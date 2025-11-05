namespace NicheVinDecoder.Legacy.Models
{
    public class LegacyModelEntry
    {
        public string make { get; set; } = "";
        public int[] model_position_range { get; set; } = new int[2];
        public Dictionary<string, string> model_map { get; set; } = new();
        public string notes { get; set; } = "";
    }
}
