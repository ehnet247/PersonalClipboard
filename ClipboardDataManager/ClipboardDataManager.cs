using System.Collections.ObjectModel;
using System.Text.Json;

namespace ClipboardData
{
    public class ClipboardDataManager : IClipboardDataManager
    {
        public Data? ClipboardData { get; set; }
        private readonly string _filename;
        private readonly string _dataFileName = "Clipboard.data";
        public ClipboardDataManager()
        {
            // Attempt to read the data file
            TryReadData();
        }

        public int TryReadData()
        {
            // Read the data file
            if (File.Exists(_filename))
            {
                string fileContent = string.Empty;
                try
                {
                    fileContent = File.ReadAllText(_filename);
                }
                catch (IOException ex)
                {
                    // Create an empty data object if the file cannot be read
                    ClipboardData = new Data();
                    // Indicate failure
                    return -1;
                }
                if (!string.IsNullOrEmpty(fileContent))
                {
                    ClipboardData = JsonSerializer.Deserialize<Data>(fileContent);
                    // Create an ObservableCollection from the deserialized PhraseList
                    if (ClipboardData != null)
                    {
                        ClipboardData.Phrases = new ObservableCollection<PhraseStruct>(ClipboardData.PhraseList);
                        // Indicate success
                        return 0;
                    }
                    else
                    {
                        // Indicate failure
                        return -1;
                    }
                }
                else
                    // Indicate failure
                    return -1;
            }
            else
            {
                // Create an empty data object if the file does not exist
                ClipboardData = new Data();
                // Indicate failure
                return -1;
            }
        }

        public int WriteData()
        {
            if (ClipboardData != null)
            {
                string jsonData = JsonSerializer.Serialize(ClipboardData);
                try
                {
                    File.WriteAllText(_filename, jsonData);
                }
                catch (IOException ex)
                {
                    // Indicate failure
                    return -1; 
                }
                // Indicate success
                return 0;
            }
            // Indicate failure
            return -1;
        }
    }
}
