using School_English.Class;
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
    /// Логика взаимодействия для AddTag.xaml
    /// </summary>
    public partial class AddTag : Window
    {
        private List<Tag> _tags;
        private IGetEntity _context;
        public Tag outTags = new Tag();
        public AddTag()
        {
            InitializeComponent();
            _context = GetEntity.GetInstance();
            sliderRed.ValueChanged += UpdateColorPreview;
            sliderGreen.ValueChanged += UpdateColorPreview;
            sliderBlue.ValueChanged += UpdateColorPreview;
            var tempList = _context.GetDB().Tag.ToList();
            List<Tag> outList = new List<Tag>();
            foreach (var tag in tempList)
            {
                if (outList.FirstOrDefault(i => i.Title == tag.Title) == null)
                {
                    outList.Add(tag);
                }

            }
            DataGridTag.ItemsSource = outList;
        }
        private void UpdateColorPreview(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Создаем цвет на основе текущих значений слайдеров
            Color selectedColor = Color.FromRgb(
                (byte)sliderRed.Value,
                (byte)sliderGreen.Value,
                (byte)sliderBlue.Value);

            // Создаем кисть с выбранным цветом и устанавливаем ее для прямоугольника предварительного просмотра
            SolidColorBrush brush = new SolidColorBrush(selectedColor);
            colorPreview.Fill = brush;

            // Обновляем текстовое поле с кодом цвета
            colorCode.Text = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridTag.SelectedItem == null)
            {
                if (TextBoxNameTag.Text != "")
                {
                    try
                    {
                        outTags.Color = colorCode.Text;
                        outTags.Title = TextBoxNameTag.Text;
                        this.Close();
                    }
                    catch
                    {
                        var info = new Information("Такого цвета нет");
                        info.ShowDialog();
                    }
                }
            }
            else
            {
                var tag = (Tag)DataGridTag.SelectedItem;
                outTags.Color = tag.Color;
                outTags.Title = tag.Title;
                outTags.ID = tag.ID;
                this.Close();
            }
        }

    }
}
