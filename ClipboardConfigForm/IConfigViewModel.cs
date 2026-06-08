using ClipboardData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ClipboardConfigForm
{
    public interface IConfigViewModel
    {
        PhraseStruct SelectedPhrase { get; set; }
        List<PhraseStruct>? PhraseList { get; }
    }
}
