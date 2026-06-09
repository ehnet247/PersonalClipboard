using ClipboardData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ClipboardConfigForm
{
    public interface IConfigViewModel
    {
        List<PhraseStruct>? PhraseList { get; }
    }
}
