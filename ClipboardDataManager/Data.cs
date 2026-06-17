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

        public Data()
        {
            PhraseList = new List<PhraseStruct>();
            Phrases = new ObservableCollection<PhraseStruct>();
        }
    }

    public struct PhraseStruct
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public bool IsSecret { get; set; }
    }
}
