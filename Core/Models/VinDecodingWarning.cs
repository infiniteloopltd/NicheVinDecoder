namespace NicheVinDecoder.Core.Models
{
    public class VinDecodingWarning
    {
        public string Message { get; set; }
        public string? Code { get; set; }
        public DateTime Timestamp { get; set; }

        public VinDecodingWarning(string message, string? code = null)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Code = code;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return Code != null ? $"[{Code}] {Message}" : Message;
        }
    }
}
