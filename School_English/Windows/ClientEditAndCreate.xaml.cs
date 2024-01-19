using Microsoft.Win32;
using School_English.Class;
using School_English.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ClientEditAndCreate.xaml
    /// </summary>
    public partial class ClientEditAndCreate : Window
    {
        private string pathStoregeImage =
            $"C:\\Users\\{Environment.UserName}\\OneDrive\\Документы\\English_school\\";

        private Client _client;
        private IGetEntity _context;
        private string fullPathImage = "";

        public ClientEditAndCreate(Client client)
        {
            InitializeComponent();
            _client = new Client();
            _context = GetEntity.GetInstance();
            if (client != null)
            {
                _client = _context.GetDB().Set<Client>().Find(client.ID);
                Load();
            }
            else
            {
                textBoxID.Visibility = Visibility.Hidden;
            }
        }

        public void Load()
        {
            textBoxID.Text = _client.ID.ToString();
            textBoxEmail.Text = _client.Email;
            textBoxFirstName.Text = _client.FirstName;
            textBoxPatronymic.Text = _client.Patronymic;
            textBoxPhone.Text = _client.Phone;
            textBoxSecondName.Text = _client.LastName;
            datePickerBirthday.DisplayDate = _client.Birthday.Value;
            datePickerBirthday.SelectedDate = _client.Birthday.Value;
            CheckGender();
            if (_client.PhotoPath != null)
            {
                ImageClient.Source = new BitmapImage(new Uri(pathStoregeImage + "\\" + _client.PhotoPath,
                    UriKind.RelativeOrAbsolute));
            }

            dataGridTag.ItemsSource = _client.Tag.ToList();
        }

        public void CheckGender()
        {
            if (_client.GenderCode == "2")
            {
                radioButtonFmale.IsChecked = true;
            }
            else if (_client.GenderCode == "1")
            {
                radioButtonMale.IsChecked = true;
            }
        }

        private void ButtonImageChange_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();
                file.InitialDirectory = "c:\\";
                if (file.ShowDialog() == true)
                {
                    fullPathImage = file.FileName;
                    FileInfo size = new FileInfo(file.FileName);
                    if ((double)2 < (double)size.Length / (1024 * 1024))
                    {
                        throw new Exception(message: "Слишком большой размер фотографии");
                    }

                    ImageClient.Source = new BitmapImage(new Uri(file.FileName, UriKind.RelativeOrAbsolute));
                }

                _client.PhotoPath = "Клиенты\\" + NameImage(file.FileName);
            }
            catch (Exception ex)
            {
                var inf = new Information(ex.Message);
                inf.ShowDialog();
            }
        }

        public string NameImage(string path)
        {
            return path.Split('\\')[path.Split('\\').Length - 1];
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    if ((textBoxPhone.Text == "") ||
                        (!(new Regex(@"\d{1}\(\d{0,4}\)\d{3}\-\d{2}\-\d{2}$")).IsMatch(textBoxPhone.Text)))
                        throw new Exception("Ошибка в поле телефона");
                    if ((textBoxFirstName.Text == "") || 
                        (!(new Regex(@"[A-Za-zА-Яа-яЁё \-]")).IsMatch(textBoxFirstName.Text)))
                        throw new Exception("Ошибка в поле имени");
                    if (!(new Regex(@"[A-Za-zА-Яа-яЁё \-]")).IsMatch(textBoxPatronymic.Text))
                        throw new Exception("Ошибка в поле отчестве");
                    if ((textBoxSecondName.Text == "") ||
                        (!(new Regex(@"[A-Za-zА-Яа-яЁё \-]{0,50}")).IsMatch(textBoxSecondName.Text)))
                        throw new Exception("Ошибка в поле фамилия");
                    if ((textBoxEmail.Text == "") ||
                        (!(new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)")).IsMatch(textBoxEmail.Text)))
                        throw new Exception("Ошибка в поле Email");
                    if (CheckRadioButton() == 0) throw new Exception("Ошибка в поле пол");

                    var db = _context.GetDB();

                    if (_client.PhotoPath != null && fullPathImage != "")
                    {
                        SaveImage();
                    }

                    if (_client.RegistrationDate == new DateTime())
                    {
                        _client.RegistrationDate = DateTime.Now;
                    }

                    _client.Birthday = datePickerBirthday.SelectedDate;
                    _client.Email = textBoxEmail.Text;
                    _client.FirstName = textBoxFirstName.Text;
                    _client.LastName = textBoxSecondName.Text;
                    _client.Patronymic = textBoxPatronymic.Text;
                    _client.Phone = textBoxPhone.Text;
                    _client.GenderCode = CheckRadioButton().ToString();
                    var tempList = dataGridTag.Items.Cast<Tag>().ToList();
                    var tags = _client.Tag.ToList();
                    foreach (var entity in tempList)
                    {

                        if (tags.FirstOrDefault(i => i.Title == entity.Title) == null)
                        {
                            _client.Tag.Add(entity);
                        }


                    }
                    if (textBoxID.Visibility == Visibility.Hidden)
                    {
                        db.Client.Add(_client);
                    }

                    db.SaveChanges();
                    this.Close();
                }
                catch (Exception ex)
                {
                    var inf = new Information(ex.Message);
                    inf.ShowDialog();
                }
            }
        }
        public int CheckRadioButton()
        {
            if (radioButtonFmale.IsChecked == true)
            {
                return 1;
            }

            if (radioButtonMale.IsChecked == true)
            {
                return 2;
            }

            return 0;
        }
        public void SaveImage()
        {

            BitmapImage image = (BitmapImage)ImageClient.Source;
            if (image != null)
            {

                string nameImage = NameImage(fullPathImage);
                using (FileStream fl = new FileStream(pathStoregeImage + "Клиенты\\" + nameImage, FileMode.Create))
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(fl);
                }
            }

        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAddTag_OnClick(object sender, RoutedEventArgs e)
        {
            var tagAdd = new AddTag();
            tagAdd.ShowDialog();
            if (tagAdd.outTags != null)
            {
                var tempList = dataGridTag.Items.Cast<Tag>().ToList();
                tempList.Add(tagAdd.outTags);
                dataGridTag.ItemsSource = tempList;
            }
        }

        private void ButtomDeleteTag_OnClick(object sender, RoutedEventArgs e)
        {
            if (dataGridTag.SelectedItem != null)
            {
                var tempList = dataGridTag.Items.Cast<Tag>().ToList();
                tempList.Remove((Tag)dataGridTag.SelectedItem);
                dataGridTag.ItemsSource = tempList;
            }
        }
    }
}
