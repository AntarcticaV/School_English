using School_English.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace School_English.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClientOfService.xaml
    /// </summary>
    public partial class ClientOfService : Window
    {
       
        public ClientOfService(Client client)
        {
            InitializeComponent();
            Load(client);
        }

        public void Load(Client client)
        {
            var listToDataGrid = new List<DataGridService>();
            foreach (var item in client.ClientService)
            {
                var temp = new DataGridService();
                
                temp.Name = item.Service.Title;

                temp.Time = item.StartTime;

                listToDataGrid.Add(temp);
            }
            DataGridClient.ItemsSource = listToDataGrid;
        }
    }

    public class DataGridService
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }

    }
}

