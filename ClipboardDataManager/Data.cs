using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ClipboardData
{
    public class Data
    {
        public List<PhraseStruct> PhraseList { get; set; }
        public ObservableCollection<PhraseStruct> Phrases { get; set; }
    }

    public struct PhraseStruct
    {
        public string Phrase { get; set; }
        public string Name { get; set; }
    }
}
