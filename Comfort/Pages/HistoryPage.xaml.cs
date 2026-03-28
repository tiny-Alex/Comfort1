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

                var sales = Connection.connect.SaleHistory
                    .Where(sh => sh.SalePoint.Id_partner == _partner.Id_partner)
                    .Select(sh => new
                    {
                        ProductName = sh.Products.NameProduct,
                        Quantity = sh.Quantity,
                        Amount = sh.Amount,
                        PointName = sh.SalePoint.NamePoint,
                        ProductId = sh.Id_product,
                        ProductTypeId = sh.Products.Id_type
                    }).ToList();

                SalesListView.ItemsSource = sales;

                TotalSalesCount.Text = sales.Count.ToString();
                TotalProductsCount.Text = sales.Sum(x => x.Quantity ?? 0).ToString();
                TotalAmount.Text = sales.Sum(x => x.Amount ?? 0).ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int CalculateMaterial(int productTypeId, int materialTypeId, int quantity, decimal param1, decimal param2)
        {
            try
            {
                var prodType = Connection.connect.ProductType.FirstOrDefault(pt => pt.Id_prodtype == productTypeId);
                var material = Connection.connect.TypeMaterial.FirstOrDefault(tm => tm.Id_type_material == materialTypeId);

                if (prodType == null || material == null) return -1;

                decimal koeff;
                if (!decimal.TryParse(prodType.Coefficient, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out koeff))
                    return -1;

                decimal brak = (decimal)material.LostProcent;

                decimal naEdinicu = param1 * param2 * koeff;
                decimal vsego = naEdinicu * quantity;
                decimal sBrakom = vsego * (1 + brak);

                return (int)Math.Ceiling(sBrakom);
            }
            catch
            {
                return -1;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
