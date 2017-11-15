using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsGtkLive.ViewModels
{
    public class EditorViewModel : BindableObject
    {
        private string _liveXaml;
        private View _preview;
        private string xmlSelected;

        public EditorViewModel()
        {
            Assembly ass = typeof(EditorViewModel).GetTypeInfo().Assembly;
            AssembliesList = new ObservableCollection<AssemblyViewModel>();
            AssembliesList.Add(new AssemblyViewModel() { Assembly = ass });
        }

        public string LiveXaml
        {
            get { return _liveXaml; }
            set
            {
                _liveXaml = value;
                PreviewXaml(_liveXaml);
            }
        }

        public ObservableCollection<AssemblyViewModel> AssembliesList { get; set; }
        string selectedXaml;

        public string SelectedXaml
        {
            get
            {
                return selectedXaml;
            }
            set
            {
                selectedXaml = value;
                Reload();
            }
        }

        public Dictionary<string, string> XAMLFiles
        {
            get
            {
                return AssembliesList[0].XMLFiles;
            }
        }

        public View Preview
        {
            get { return _preview; }
            set
            {
                _preview = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReloadCommand => new Command(Reload);

        private async void PreviewXaml (string xaml)
        {
            var contentPage = new ContentPage();

            try
            {
                if (string.IsNullOrEmpty(xaml))
                    return;

                string contentPageXaml = $"<?xml version='1.0' encoding='utf-8' ?><ContentPage xmlns='http://xamarin.com/schemas/2014/forms' xmlns:x='http://schemas.microsoft.com/winfx/2009/xaml' x:Class ='FormsGtkLive.XamlPage'>{xaml}</ContentPage>";

                await Live.UpdatePageFromXamlAsync(contentPage, contentPageXaml);
            }
            catch (Exception exception)
            {
                // Error 
                Debug.WriteLine(exception.Message);
                var xamlException = Live.GetXamlException(exception);
                await Live.UpdatePageFromXamlAsync(contentPage, xamlException);
            }

            Preview = contentPage.Content;
        }

        private void Reload ()
        {
            string selectedXAML = SelectedXaml;

            // Read XAML file content
            var xaml = string.Empty;
            using (var fileStream = new FileStream(XAMLFiles[selectedXAML], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                xaml = textReader.ReadToEnd();
            }
            LiveXaml = xaml;
            //XAMLEditor.Text = xaml;
            PreviewXaml(LiveXaml);
        }
    }
}