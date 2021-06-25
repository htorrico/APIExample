using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace APIExample
{
    public partial class MainPage : ContentPage
    {
        HttpClient client;
        public MainPage()
        {
            InitializeComponent();
            client = new HttpClient();

        }
        public async Task<List<TodoItem>> CargarDatosAsync()
        {


        
           var Items = new List<TodoItem>();

            Uri uri = new Uri(string.Format(Constants.RestUrl, "Get"));

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Items = JsonConvert.DeserializeObject<List<TodoItem>>(content);
            }
            return Items;
        }

        public async Task Save(TodoItem item)
        {

            HttpResponseMessage response = null;
            //1. Convertir objeto a JSON            
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //2. Armar la URI
            var uri = new Uri(string.Format(Constants.RestUrl, "Create"));
            
            //3. LLamar al servicio 
            response = await client.PostAsync(uri, content);
          
            //4. Evaluo si es correcto o no
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Gracias", "gracias", "ok");
            }
            else
            {
                await DisplayAlert("Error", "Error", "Error");
            }
        }



        private async void Button_Clicked(object sender, EventArgs e)
        {

            ltvItems.ItemsSource = await CargarDatosAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Save(
                new TodoItem { ID = "3333333", Name = "Tecsup 2022", Notes = "Notes", Done = false }
                );

        }
    }


}
