using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FormsGtkLive.ViewModels
{
    public class AssemblyViewModel : BindableObject
    {
        Assembly assembly;

        public AssemblyViewModel()
        {
            XMLFiles = new Dictionary<string, string>();
        }

        public Assembly Assembly
        {
            get
            {
                return assembly;
            }
            set
            {
                assembly = value;
                GetXMLFiles();
            }
        }

        public string AssemblyName { get { return Assembly.FullName; } }

        public Dictionary<string, string> XMLFiles { get; set; }

        public void GetXMLFiles()
        {
            var types = Assembly.DefinedTypes;
            foreach (var item in types)
            {
                var attrData = item.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(Xamarin.Forms.Xaml.XamlFilePathAttribute));

                if (attrData != null)
                {
                    // the type has a XAML linked
                    var name = item.FullName;
                    var path = attrData.ConstructorArguments.First().Value;
                    XMLFiles.Add(name, path.ToString());
                }
            }
        }
    }
}
