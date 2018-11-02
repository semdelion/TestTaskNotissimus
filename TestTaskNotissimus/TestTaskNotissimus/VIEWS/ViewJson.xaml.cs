using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestTaskNotissimus.VIEWS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewJson : ContentPage
	{
		public ViewJson (XmlNode offer)
		{
			InitializeComponent ();
            labelJson.Text = JsonConvert.SerializeXmlNode(offer, Newtonsoft.Json.Formatting.Indented);
        }
	}
}