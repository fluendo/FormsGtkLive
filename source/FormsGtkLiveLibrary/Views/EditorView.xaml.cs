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
            vm.SelectedXaml = (string)listView.SelectedItem;
            XAMLEditor.Text = vm.LiveXaml;
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