using System.IO;
using FormsGtkLive.ViewModels;
using Xamarin.Forms;
using Plugin.FilePicker;
using Gtk;
using System.Collections.Generic;
using System.Reflection;

namespace FormsGtkLive.Views
{
    public partial class EditorView : ContentPage
    {
        public EditorView()
        {
            InitializeComponent();

            BindingContext = new EditorViewModel();
            XMLList.ItemSelected += XMLList_ItemSelected;
            LoadButton.Clicked += LoadButton_Clicked;
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

        void LoadButton_Clicked(object sender, System.EventArgs e)
        {
            FileChooserDialog fChooser;
            Window toplevel = null;
            List<string> path;
            EditorViewModel vm = (EditorViewModel)BindingContext;

            fChooser = new FileChooserDialog("Assembly chooser", toplevel, FileChooserAction.Open,
                                             "gtk-cancel", ResponseType.Cancel,
                                             "gtk-open", ResponseType.Accept);

            fChooser.SelectMultiple = false;

            if (fChooser.Run() != (int)ResponseType.Accept)
            {
                path = new List<string>();
            }
            else
            {
                path = new List<string>(fChooser.Filenames);
                Assembly ass = Assembly.LoadFrom(path[0]);
                vm.AssembliesList.Add(new AssemblyViewModel() { Assembly = ass });
            }

            fChooser.Destroy();
        }
    }
}