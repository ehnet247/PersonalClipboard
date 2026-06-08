using ClipboardData;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace ClipboardConfigForm.ViewModels
{
    public partial class ConfigViewModel : ObservableRecipient, IConfigViewModel
    {
        private readonly IClipboardDataManager? _dataManager;
        [ObservableProperty]
        private PhraseStruct selectedPhrase;
        [ObservableProperty]
        private bool busy = false;
        private bool _needToSave = false;
        [RelayCommand]
        private void New()
        {
            SelectedPhrase = new PhraseStruct { Name = "New Phrase", Phrase = string.Empty };
        }

        [RelayCommand]
        private void Validate()
        {
            var existingPhrase = _dataManager!.ClipboardData!.Phrases.SingleOrDefault(p => p.Name == SelectedPhrase.Name);
            if (!string.IsNullOrEmpty(existingPhrase.Name))
            {
                if (existingPhrase.Name != SelectedPhrase.Name &&
                    existingPhrase.Phrase != SelectedPhrase.Phrase)
                {
                    existingPhrase.Name = SelectedPhrase.Name;
                    existingPhrase.Phrase = SelectedPhrase.Phrase;
                    _needToSave = true;
                }
            }
            else
            {
                _dataManager.ClipboardData!.Phrases.Add(SelectedPhrase);
                _needToSave = true;
            }
        }

        [RelayCommand]
        private void Close()
        {
            Busy = true;
            //Store the phrases in file
            if (_needToSave)
            {
                _dataManager!.WriteData();
                _needToSave = false;
            }
            Busy = false;
        }

        public ObservableCollection<PhraseStruct>? Phrases => _dataManager?.ClipboardData?.Phrases;
        public List<PhraseStruct>? PhraseList => _dataManager?.ClipboardData?.PhraseList;
        public ConfigViewModel(IClipboardDataManager dataManager)
        {
            _dataManager = dataManager;
        }
    }
}
