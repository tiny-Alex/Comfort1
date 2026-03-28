using Comfort.DBModel;
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

namespace Comfort.Pages
{
    /// <summary>
    /// Логика взаимодействия для ParthnerListPage.xaml
    /// </summary>
    public partial class ParthnerListPage : Page
    {
        public ParthnerListPage()
        {
            InitializeComponent();

            FilterCombo.ItemsSource = Connection.connect.TypeOfBusiness.ToList();
            FilterCombo.DisplayMemberPath = "NameBusiness";
            FilterCombo.SelectedValuePath = "Id_type";
            PartnersLW.ItemsSource = Connection.connect.Partners.ToList();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var partners = Connection.connect.Partners.ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                partners = partners.Where(p => p.NamePartner.Contains(SearchBox.Text)).ToList();
            }

            if (FilterCombo.SelectedItem is TypeOfBusiness type)
            {
                partners = partners.Where(p => p.Id_type == type.Id_type).ToList();
            }

            PartnersLW.ItemsSource = partners;
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var partners = Connection.connect.Partners.ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                partners = partners.Where(p => p.NamePartner.Contains(SearchBox.Text) ||
                                               p.SurnameDirector.Contains(SearchBox.Text) ||
                                               p.Phone.Contains(SearchBox.Text)).ToList();
            }

            if (FilterCombo.SelectedItem is TypeOfBusiness type)
            {
                partners = partners.Where(p => p.Id_type == type.Id_type).ToList();
            }

            PartnersLW.ItemsSource = partners;
        }

        private void ClearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            FilterCombo.SelectedIndex = -1;
            PartnersLW.ItemsSource = Connection.connect.Partners.ToList();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ParthnercAddEditPage(new Partners()));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selPartner = PartnersLW.SelectedItem as Partners;
            if (selPartner != null)
            {
                NavigationService.Navigate(new ParthnercAddEditPage(selPartner));
            }
            else
            {
                MessageBox.Show("Не выбран партнер для редактирования");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selPartner = PartnersLW.SelectedItem as Partners;
            if (selPartner != null)
            {
                Connection.connect.Partners.Remove(selPartner);
                Connection.connect.SaveChanges();
                PartnersLW.ItemsSource = Connection.connect.Partners.ToList();
                MessageBox.Show("Партнер удален");
            }
            else
            {
                MessageBox.Show("Выберите партнера для удаления");
            }
        }

        private void historyBtn_Click(object sender, RoutedEventArgs e)
        {
            var selPartner = PartnersLW.SelectedItem as Partners;
            if (selPartner != null)
            {
                NavigationService.Navigate(new HistoryPage(selPartner));
            }
            else
            {
                MessageBox.Show("Выберите партнера для просмотра истории");
            }
        }
    }
}
