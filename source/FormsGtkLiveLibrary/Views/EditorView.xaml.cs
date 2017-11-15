using System.IO;
using FormsGtkLive.ViewModels;
using Xamarin.Forms;

namespace FormsGtkLive.Views
{
    public partial class EditorView : ContentPage
    {
        public EditorView()
        {
            InitializeComponent();

            BindingContext = new EditorViewModel();
            XMLList.ItemSelected += XMLList_ItemSelected;
        }

        void XMLList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
            using (var fileStream = new FileStream(vm.XAMLFiles[selectedXAML], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                xaml = textReader.ReadToEnd();
            }
            vm.LiveXaml = xaml;
            XAMLEditor.Text = xaml;
        }
    }
}