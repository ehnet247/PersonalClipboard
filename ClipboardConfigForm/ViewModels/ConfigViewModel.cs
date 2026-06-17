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
        private string editingPhraseName = string.Empty;
        partial void OnEditingPhraseNameChanged(string value)
        {
            Debug.WriteLine($"Phrase name changed to: {value}");
        }
        [ObservableProperty]
        private string editingPhraseContent = string.Empty;
        partial void OnEditingPhraseContentChanged(string value)
        {
            Debug.WriteLine($"Phrase content changed to: {value}");
        }
        [ObservableProperty]
        private bool editingPhraseIsSecret = false;
        partial void OnEditingPhraseIsSecretChanged(bool value)
        {
            Debug.WriteLine($"Phrase IsSecret changed to: {value}");
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
                EditingPhraseName = selectedPhrase.Value.Name;
                EditingPhraseContent = selectedPhrase.Value.Content;
                EditingPhraseIsSecret = selectedPhrase.Value.IsSecret;
            }
        }
        private bool _needToSave = false;
        private bool _newPhrase = false;
        [RelayCommand]
        private void Add()
        {
            _newPhrase = true;
            EditingPhraseName = "New Phrase";
            EditingPhraseContent = string.Empty;
            EditingPhraseIsSecret = false;
        }

        private void Rem()
        {if (!string.IsNullOrEmpty(EditingPhraseName))
            {
                var existingPhrase = _dataManager!.ClipboardData!.Phrases.SingleOrDefault(p => p.Name == EditingPhraseName);
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
            var existingPhrase = _dataManager!.ClipboardData!.Phrases.SingleOrDefault(p => p.Name == EditingPhraseName);
            // If the phrase already exists, update it
            if (!string.IsNullOrEmpty(existingPhrase.Name))
            {
                if (!_newPhrase)
                {
                    if (existingPhrase.Name != EditingPhraseName ||
                        existingPhrase.Content != EditingPhraseContent ||
                        existingPhrase.IsSecret != EditingPhraseIsSecret)
                    {
                        for (int i = 0; i < _dataManager.ClipboardData!.Phrases.Count; i++)
                        {
                            if (_dataManager.ClipboardData.Phrases[i].Name == existingPhrase.Name)
                            {
                                _dataManager.ClipboardData.Phrases[i] = new PhraseStruct
                                {
                                    Name = EditingPhraseName,
                                    Content = EditingPhraseContent,
                                    IsSecret = EditingPhraseIsSecret
                                };
                                break;
                            }
                        }
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
                    Name = EditingPhraseName,
                    Content = EditingPhraseContent,
                    IsSecret = EditingPhraseIsSecret
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
