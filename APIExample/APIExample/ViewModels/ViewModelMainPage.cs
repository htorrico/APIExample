using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace APIExample.ViewModels
{
    public class ViewModelMainPage : BaseViewModel
    {

        private string id;
        public string ID
        {
            get { return this.id; }
            set { SetValue(ref this.id, value); }
        }
        private string name;
        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }

        private string notes;
        public string Notes
        {
            get { return this.notes; }
            set { SetValue(ref this.notes, value); }
        }

        private bool done;
        public bool Done
        {
            get { return this.done; }
            set { SetValue(ref this.done, value); }
        }

        private ObservableCollection<TodoItem> items;
        public ObservableCollection<TodoItem> Items
        {
            get { return this.items; }
            set { SetValue(ref this.items, value); }
        }

        public IAsyncCommand CreateCommand {  get; set; }
        public IAsyncCommand GetCommand {  get; set; }

        HttpClient client;
        public ViewModelMainPage()
        {

            client = new HttpClient();

            CreateCommand = new AsyncCommand( () =>  Create());


            GetCommand = new AsyncCommand( () =>  Get());
            
        }

        async Task Create()
        {
            HttpResponseMessage response = null;

            var item = new TodoItem
            { ID = this.ID, Name = this.Name, Notes = this.Notes, Done = this.Done };

            //1. Convertir objeto a JSON
            //
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //2. Armar la URI
            var uri = new Uri(string.Format(Constants.RestUrl, "Create"));

            //3. LLamar al servicio 
            response = await client.PostAsync(uri, content);
        }

        async Task Get()
        {
            var ItemsList = new List<TodoItem>();

            Uri uri = new Uri(string.Format(Constants.RestUrl, "Get"));

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ItemsList = JsonConvert.DeserializeObject<List<TodoItem>>(content);

                //La lista la convierto a un colección 
                Items = new ObservableCollection<TodoItem>(ItemsList);
            }

        }

    }

    
}
