namespace ClipboardData
{
    public interface IClipboardDataManager
    {
        Data? ClipboardData { get; set; }
        int TryReadData();
        int WriteData();
    }
}