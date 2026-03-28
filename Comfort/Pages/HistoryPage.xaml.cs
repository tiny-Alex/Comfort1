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
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        private Partners _partner;

        public HistoryPage(Partners partner)
        {
            InitializeComponent();
            _partner = partner;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                PartnerNameTb.Text = _partner.NamePartner;
                PartnerInfoTb.Text = $"Рейтинг: {_partner.Raiting} | ИНН: {_partner.INN} | Телефон: {_partner.Phone}";

                var sales = (from sh in Connection.connect.SaleHistory
                             join sp in Connection.connect.SalePoint on sh.Id_point equals sp.Id_point
                             join p in Connection.connect.Products on sh.Id_product equals p.Id_product
                             where sp.Id_partner == _partner.Id_partner
                             select new
                             {
                                 p.NameProduct,
                                 sh.Quantity,
                                 sh.Amount,
                                 sp.NamePoint,
                             }).ToList();

                if (sales.Count > 0)
                {
                    SalesListView.ItemsSource = sales;
                    EmptyMessage.Visibility = Visibility.Collapsed;
                    SalesListView.Visibility = Visibility.Visible;

                    TotalSalesCount.Text = sales.Count.ToString();
                    TotalProductsCount.Text = sales.Sum(x => x.Quantity ?? 0).ToString();
                    TotalAmount.Text = sales.Sum(x => x.Amount ?? 0).ToString("N2");
                }
                else
                {
                    SalesListView.Visibility = Visibility.Collapsed;
                    EmptyMessage.Visibility = Visibility.Visible;

                    TotalSalesCount.Text = "0";
                    TotalProductsCount.Text = "0";
                    TotalAmount.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
