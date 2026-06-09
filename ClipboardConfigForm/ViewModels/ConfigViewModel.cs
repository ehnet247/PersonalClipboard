using ClipboardData;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace ClipboardConfigForm.ViewModels
{
    public partial class ConfigViewModel : ObservableRecipient, IConfigViewModel
    {
        private readonly IClipboardDataManager? _dataManager;
        [ObservableProperty]
        private string phraseNameEditing = string.Empty;
        partial void OnPhraseNameEditingChanged(string value)
        {
            Debug.WriteLine($"Phrase name changed to: {value}");
        }
        [ObservableProperty]
        private string phraseContentEditing = string.Empty;
        partial void OnPhraseContentEditingChanged(string value)
        {
            Debug.WriteLine($"Phrase content changed to: {value}");
        }
        [ObservableProperty]
        private bool busy = false;
        [ObservableProperty]
        private object? selectedPhrase;
        partial void OnSelectedPhraseChanged(object? value)
        {
            var selectedPhrase = value as PhraseStruct?;
            if (selectedPhrase != null)
            {
                PhraseNameEditing = selectedPhrase.Value.Name;
                PhraseContentEditing = selectedPhrase.Value.Content;
            }
        }
        private bool _needToSave = false;
        private bool _newPhrase = false;
        [RelayCommand]
        private void Add()
        {
            _newPhrase = true;
            PhraseNameEditing = "New Phrase";
            PhraseContentEditing = string.Empty;
        }

        private void Rem()
        {if (!string.IsNullOrEmpty(PhraseNameEditing))
            {
                var existingPhrase = _dataManager!.ClipboardData!.Phrases.SingleOrDefault(p => p.Name == PhraseNameEditing);
                if (!string.IsNullOrEmpty(existingPhrase.Name))
                {
                    _dataManager.ClipboardData!.Phrases.Remove(existingPhrase);
                    _needToSave = true;
                }
            }
        }

        [RelayCommand]
        private void Validate()
        {
            var existingPhrase = _dataManager!.ClipboardData!.Phrases.SingleOrDefault(p => p.Name == PhraseNameEditing);
            // If the phrase already exists, update it
            if (!string.IsNullOrEmpty(existingPhrase.Name))
            {
                if (!_newPhrase)
                {
                    if (existingPhrase.Name != PhraseNameEditing &&
                        existingPhrase.Content != PhraseContentEditing)
                    {
                        existingPhrase.Name = PhraseNameEditing;
                        existingPhrase.Content = PhraseContentEditing;
                        _needToSave = true;
                        NotifyPhraseListChanged();
                    }
                }
                else
                {
                    // Error: A phrase with the same name already exists
                    _newPhrase = false;
                }
            }
            // If the phrase doesn't exist, add it to the list
            else if(_newPhrase)
            {
                _newPhrase = false;
                PhraseStruct newPhrase = new PhraseStruct
                {
                    Name = PhraseNameEditing,
                    Content = PhraseContentEditing
                };
                _dataManager.ClipboardData!.Phrases.Add(newPhrase);
                _needToSave = true;
                NotifyPhraseListChanged();
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

        public void NotifyPhraseListChanged()
        {
            OnPropertyChanged(nameof(Phrases));
        }

        public ObservableCollection<PhraseStruct>? Phrases => _dataManager?.ClipboardData?.Phrases;
        public List<PhraseStruct>? PhraseList => _dataManager?.ClipboardData?.PhraseList;
        public ConfigViewModel(IClipboardDataManager dataManager)
        {
            _dataManager = dataManager;
        }
    }
}
