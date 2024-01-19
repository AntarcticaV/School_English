
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using School_English.Class;
using School_English.Interface;
using School_English.Windows;

namespace School_English.Page
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : System.Windows.Controls.Page
    {
        private IGetEntity _context;
        private int outElement = 10;
        private int countSendElement = 0;
        private int pageNumber = 0;
        private int allClient = 0;

        public AdminPanel(Frame mainFrame)
        {
            InitializeComponent();
            _context = GetEntity.GetInstance();
            Load();
        }

        public void Load()
        {
            try
            {
                IEnumerable<ExtendedClient> res = _context.GetDB().Client.Select(c => new ExtendedClient
                {
                    ID = c.ID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Patronymic = c.Patronymic,
                    Birthday = c.Birthday,
                    RegistrationDate = c.RegistrationDate,
                    Email = c.Email,
                    Phone = c.Phone,
                    GenderCode = c.GenderCode,
                    PhotoPath = c.PhotoPath,
                    Gender = c.Gender,
                    ClientService = c.ClientService,
                    Tag = c.Tag,
                }).AsEnumerable();
                allClient = res.Count();
                IFilter fr = new Filter(res, textBoxFilter.Text, comboBoxGender.SelectedIndex, comboBoxSort.SelectedIndex);

                TurnButton();

                TextBoxNumberPage.Text = (pageNumber + 1).ToString();

                var send = fr.GetFilterAndSort().ToList();

                countSendElement = fr.Count();

                TextBlockCount.Text = $"Вывелось записей {countSendElement} из {allClient}";


                DataGridClient.ItemsSource = send.Skip(pageNumber * outElement).Take(outElement);
            }
            catch (Exception ex)
            {
                var info = new Information(ex.Message);
                info.ShowDialog();
            }
        }


        private void TurnButton()
        {
            if (pageNumber == 0) { ButtonBackPage.IsEnabled = false; }
            else { ButtonBackPage.IsEnabled = true; };
            if ((pageNumber + 1) * outElement >= countSendElement) { ButtonNextPage.IsEnabled = false; }
            else { ButtonNextPage.IsEnabled = true; };
        }


        private void ComboBoxGender_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load();
        }

        private void ComboBoxSort_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load();
        }

        private void TextBoxFilter_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Load();
        }

        private void ButtonFirstPage_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber = 0;
            Load();
        }


        private void ButtonBackPage_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber--;
            Load();
        }

        private void ButtonNextPage_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber++;
            Load();
        }

        private void ButtonLastPage_OnClick(object sender, RoutedEventArgs e)
        {

            pageNumber = (int)Math.Ceiling((double)countSendElement / (double)outElement) - 1;
            Load();
        }

        private void ButtonOutTen_OnClick(object sender, RoutedEventArgs e)
        {
            outElement = 10;
            Load();
        }

        private void ButtonOutFifty_OnClick(object sender, RoutedEventArgs e)
        {
            outElement = 50;
            Load();
        }

        private void ButtonOutTwoHundred_OnClick(object sender, RoutedEventArgs e)
        {
            outElement = 200;
            Load();
        }

        private void ButtonOutAll_OnClick(object sender, RoutedEventArgs e)
        {
            outElement = allClient;
            Load();
        }

        private void ButtonDeleteClient_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridClient.SelectedItem != null)
            {
                var item = (Client)DataGridClient.SelectedItem;
                try
                {
                    if (item.ClientService.Count() != 0)
                    {
                        throw new Exception();
                    }
                    var tempDB = _context.GetDB();

                    item = tempDB.Set<Client>().Find(item.ID);
                    if (item != null)
                    {
                        tempDB.Set<Client>().Remove(item);
                        tempDB.SaveChanges();
                    }
                }
                catch
                {
                    var info = new Information("Удаление невозможно поскольку клиент посещал сервис");
                    info.ShowDialog();
                }
                Load();
            }
        }

        private void ButtonEditClient_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridClient.SelectedItem != null)
            {
                var windowCreateClient = new ClientEditAndCreate((Client)DataGridClient.SelectedItem);
                windowCreateClient.ShowDialog();
                Load();
            }
        }

        private void ButtonAddClient_OnClick(object sender, RoutedEventArgs e)
        {
            var windowCreateClient = new ClientEditAndCreate(null);
            windowCreateClient.ShowDialog();
            Load();
        }

        private void DataGridClient_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridClient.SelectedItem != null)
            {
                var clientService = new ClientOfService((Client)DataGridClient.SelectedItem);
                clientService.ShowDialog();

            }
        }
    }
}

