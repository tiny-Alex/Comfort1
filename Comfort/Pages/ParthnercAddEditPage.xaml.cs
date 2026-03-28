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
using Comfort.Pages;

namespace Comfort.Pages
{
    /// <summary>
    /// Логика взаимодействия для ParthnercAddEditPage.xaml
    /// </summary>
    public partial class ParthnercAddEditPage : Page
    {
        Partners partner;
        public ParthnercAddEditPage(Partners _partner)
        {
            InitializeComponent();
            partner = _partner;
            this.DataContext = partner;
            TypeCB.ItemsSource =
                Connection.connect.TypeOfBusiness.ToList();
            TypeCB.DisplayMemberPath = "NameBusiness";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                partner.Id_type = (TypeCB.SelectedItem as TypeOfBusiness).Id_type;
                if (partner.Id_partner == 0)
                {
                    Connection.connect.Partners.Add(partner);
                }
                Connection.connect.SaveChanges();
                MessageBox.Show("Операция прошла успешно");
                NavigationService.Navigate(new ParthnerListPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
