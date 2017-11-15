using System.Collections.Generic;
using System.IO;
using FormsGtkLive.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FormsGtkLive.Views
{
    public partial class EditorView : ContentPage
    {
        public EditorView()
        {
            InitializeComponent();

            BindingContext = new EditorViewModel();
            XMLList.ItemTapped += XMLList_ItemTapped;
        }

        void XMLList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView listView = (ListView)sender;

            if (listView == null)
            {
                return;
            }

            EditorViewModel vm = (EditorViewModel)BindingContext;
            string selectedXAML = listView.SelectedItem.ToString();

            // Read XAML file content
            var xaml = string.Empty;
            using (var fileStream = new FileStream(selectedXAML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                xaml = textReader.ReadToEnd();
            }
        }
    }
}