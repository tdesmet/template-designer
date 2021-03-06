﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using DotLiquid;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;

namespace template_designer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _currentTemplateFileName;
        private string _currentDataFileName;
        private readonly SynchronizationContext _syncContext;
        Timer _timer ; 
        public MainWindow()
        {
            _syncContext = SynchronizationContext.Current;
            InitializeComponent();
            TemplateTextEditor.TextChanged += (sender, args) => RenderCallBack();
            DataTextEditor.TextChanged += (sender, args) => RenderCallBack();
            TemplateTextEditor.WordWrap = true;
            TemplateTextEditor.Options.HighlightCurrentLine = true;
            TemplateTextEditor.Options.IndentationSize = 2;

            TemplateTextEditor.Encoding = Encoding.UTF8;
            DataTextEditor.Encoding = Encoding.UTF8;
        }

        private void RenderCallBack()
        {
            _timer = null;
            _syncContext.Send(_ => RenderTemplate(TemplateTextEditor.Text, DataTextEditor.Text), null);
        }

        private void RenderTemplate(string text, string data)
        {
            try
            {
                Output.Text = "";
                if (text == null)
                {
                    text = " ";
                }

                var template = DotLiquid.Template.Parse(text);
                if (string.IsNullOrWhiteSpace(data))
                {
                    data = "{}";
                }
                Dictionary<string, object> obj = JsonToDictionaryConverter.DeserializeJsonToDictionary(data);
                dynamic parameters = Hash.FromDictionary(obj);
                var rendered = template.Render(parameters);
                PreviewWindow.NavigateToString(rendered);
            }
            catch (Exception exception)
            {
                Output.Text = exception.Message;
            }
        }

        private void PreviewWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri != null) e.Cancel = true;
        }

        private void OpenTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            AskToSaveTemplate();
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Html files (*.htm, *.html)|*.htm;*.html|All files (*.*)|*.*"
            };
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value) return;
            _currentTemplateFileName = dialog.FileName;
            TemplateTextEditor.Load(dialog.FileName);
            TemplateTextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(_currentTemplateFileName));
        }

        private void NewTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            AskToSaveTemplate();

            _currentTemplateFileName = null;
            TemplateTextEditor.Clear();
        }

        private void AskToSaveTemplate()
        {
            if (!TemplateTextEditor.IsModified) return;
            var result = MessageBox.Show(
                "Do you want to save your changes?",
                "Template modified",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveTemplate();
            }
        }

        private void SaveTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            SaveTemplate();
        }

        private void SaveTemplate()
        {
            if (_currentTemplateFileName == null)
            {
                var dialog = new SaveFileDialog
                {
                    DefaultExt = "html",
                    Filter = "Html files (*.htm, *.html)|*.htm;*.html|All files (*.*)|*.*"
                };
                if (dialog.ShowDialog() ?? false)
                {
                    _currentTemplateFileName = dialog.FileName;
                }
                else
                {
                    return;
                }
            }
            TemplateTextEditor.Save(_currentTemplateFileName);
        }



        private void NewDataButton_Click(object sender, RoutedEventArgs e)
        {
            AskToSaveData();

            _currentDataFileName = null;
            DataTextEditor.Clear();
        }

        private void AskToSaveData()
        {
            if (!DataTextEditor.IsModified) return;
            var result = MessageBox.Show(
                "Do you want to save your changes?",
                "Data modified",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveData();
            }
        }

        private void OpenDataButton_Click(object sender, RoutedEventArgs e)
        {
            AskToSaveData();
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value) return;
            _currentDataFileName = dialog.FileName;
            DataTextEditor.Load(dialog.FileName);
            DataTextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(_currentDataFileName));
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            if (_currentDataFileName == null)
            {
                var dialog = new SaveFileDialog
                {
                    DefaultExt = "json",
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
                };
                if (dialog.ShowDialog() ?? false)
                {
                    _currentDataFileName = dialog.FileName;
                }
                else
                {
                    return;
                }
            }
            DataTextEditor.Save(_currentDataFileName);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AskToSaveTemplate();
            AskToSaveData();
        }
    }
}
