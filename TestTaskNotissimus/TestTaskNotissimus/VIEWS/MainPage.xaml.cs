using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;
using System.Net.Http;
using System.Xml;

namespace TestTaskNotissimus.VIEWS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        ObservableCollection<string> Items { get; set; }
        XmlNode Offers { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Connecting()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(Encoding.UTF8.GetString(              Encoding.Convert(
                                       Encoding.GetEncoding("Windows-1251"), Encoding.GetEncoding("UTF-8"),
                                       await client.GetByteArrayAsync("http://partner.market.yandex.ru/pages/help/YML.xml"))));
                        Offers = xmldoc.DocumentElement.SelectSingleNode("//yml_catalog/shop/offers");
                        var offers = Offers.SelectNodes("//offer");
                        Items = new ObservableCollection<string>();
                        foreach (XmlNode n in offers)
                            Items.Add(n.SelectSingleNode("@id").Value);
                        MyListView.ItemsSource = Items;
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message.ToString()}", "OK");
            }
            finally
            {
                DependencyService.Get<IMessage>().ShortAlert($"Connection type : {CrossConnectivity.Current.ConnectionTypes.FirstOrDefault().ToString()}");
            }
        }

        private void Button_Click(object sender, EventArgs e) => Connecting();
        
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            XmlNode offer = Offers.SelectSingleNode($"offer[@id='{e.Item.ToString()}']");

            await Navigation.PushAsync(new ViewJson(offer));
            ((ListView)sender).SelectedItem = null;
        }
    }
}
