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

namespace Khabirova41Size
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        List<Product> selectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        //private OrderProduct currentOrderProduct = new OrderProduct();
        public int ProductCount = 0;
        User currentUser;
        private double Cost = 0;

        private int SetDeliveryDate(List<Product> products)
        {

            bool DeliveryStatus = false;
            foreach (var p in products)
            {
                if (p.inStock <= 3)
                {
                    DeliveryStatus = true;
                }
            }

            if (DeliveryStatus)
                return 6;
            else
                return 3;
        }

        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, User user)
        {
            InitializeComponent();
            currentUser = user;
            Cost = 0;
            var currentPickups = Entities.GetContext().PickUpPoint.ToList();
            /*PickupCombo.ItemsSource = currentPickups;

            ClientTB.Text = FIO;
            TBOrderID.Text = selectedOrderProducts.First().OrderID.ToString();

            ShoeListView.ItemsSource = selectedProducts;
            foreach(Product p in selectedProducts)
            {
                p.Quantity = 1;
                foreach(OrderProduct q in selectedOrderProducts)
                {
                    if(p.ProductArticleNumber == q.ProductArticleNumber)
                    {
                        p.Quantity = Convert.ToInt32(q.Amount);
                        ProductCount = p.Quantity;
                    }
                }
            }

            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            OrderDP.Text = DateTime.Now.ToString();
            DeliveryDP.Text = DateTime.Now.ToString();*/
            //SetDeliveryDate();
            PickupCombo.ItemsSource = currentPickups.Select(x => $"{x.PickUpPointID}, {x.PickUpCity}, {x.PickUpStreet}, {x.PickUpHouseNumber}"); //вывод информации о пункте выдачи
            PickupCombo.SelectedIndex = 0;
            int currentID = selectedOrderProducts.First().OrderID; //определение номера текущего заказа
            currentOrder.OrderID = currentID;

            //Рандомный код получения
            List<Order> allOrderCodes = Entities.GetContext().Order.ToList();
            List<int> OrderCodes = new List<int>();
            foreach (var p in allOrderCodes.Select(x => $"{x.OrderID}").ToList())
            {
                OrderCodes.Add(Convert.ToInt32(p));
            }
            Random random = new Random();

            while (true)
            {
                int num = random.Next(100, 1000);
                if (!OrderCodes.Contains(num))
                {
                    currentOrder.OrderID = num;
                    break;
                }
            }


            foreach (Product p in selectedProducts)
            {
                p.Quantity = 1;//Quantity - столбец таблицы product которой нет в бд
                foreach (OrderProduct q in selectedOrderProducts)
                {
                    if (p.ProductArticleNumber == q.OrderProductArticleNumber)
                    {
                        p.Quantity = (int)q.OrderProductAmount;
                    }
                }
            }


            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;


            //определение общей стоимости заказа
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
            }

            TBPrice.Text = Cost.ToString();

            //дата формирования заказа и дата доставки
            OrderDP.Text = DateTime.Now.ToString();
            DeliveryDP.Text = DateTime.Now.AddDays(SetDeliveryDate(selectedProducts)).ToString();

            TBOrderID.Text = currentID.ToString();
            ShoeListView.ItemsSource = selectedProducts;


            //вывод ФИО текущего польлзователя 
            if (user != null)
            {
                currentOrder.OrderUserID = user.UserID;
                ClientTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            }
            else
            {
                ClientTB.Text = "Гость";
                currentOrder.OrderUserID = null;
            }
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.OrderPickupPoint = PickupCombo.SelectedIndex + 1;
            currentOrder.OrderDate = DateTime.Now;
            currentOrder.OrderDeliveryDate = DateTime.Now.AddDays(SetDeliveryDate(selectedProducts));
            currentOrder.OrderStatus = "Новый";
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                if (selectedProducts[i].ProductQuantityInStock >= selectedOrderProducts[i].OrderProductAmount)
                    selectedProducts[i].ProductQuantityInStock -= (int)selectedOrderProducts[i].OrderProductAmount;
                else
                    selectedProducts[i].ProductQuantityInStock = 0;
            }
            foreach (var p in selectedOrderProducts)
            {
                Entities.GetContext().OrderProduct.Add(p);
            }

            Entities.GetContext().Order.Add(currentOrder);

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                selectedOrderProducts.Clear();
                selectedProducts.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            this.Close();
        }

        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            /*var prod = (sender as Button).DataContext as Product;
            prod.Quantity++;
            //TBProductCount.Text = Convert.ToString(prod.Quantity);
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedOrderProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].Amount++;
            ProductCount++;
            
            ShoeListView.Items.Refresh();*/

            Cost = 0;
            var prod = (sender as Button).DataContext as Product;
            prod.Quantity++;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.OrderProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedOrderProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].OrderProductAmount++;
            //if (prod.ProductQuantityInStock > 0)
            //{
            //    prod.ProductQuantityInStock--;
            //}
            ShoeListView.Items.Refresh();
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
            }

            TBPrice.Text = Cost.ToString();
            DeliveryDP.Text = DateTime.Now.AddDays(SetDeliveryDate(selectedProducts)).ToString();
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Product;
            if (prod.Quantity > 0)
            {
                prod.Quantity--;
                Cost = 0;
                var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.OrderProductArticleNumber == prod.ProductArticleNumber);
                int index = selectedOrderProducts.IndexOf(selectedOP);
                /*selectedOrderProducts[index].Amount--;
                ProductCount--;
                //SetDeliveryDate();
                ShoeListView.Items.Refresh();*/

                if (prod.Quantity == 0)
                {
                    selectedOrderProducts[index].OrderProductAmount = 0;
                    var pr = ShoeListView.SelectedItem as Product;
                    selectedOrderProducts.RemoveAt(index);
                    selectedProducts.RemoveAt(index);
                    if (ShoeListView.Items.Count == 0)
                    {
                        this.Close();
                    }
                }
                else
                {
                    selectedOrderProducts[index].OrderProductAmount--;
                    //if (this.selectedProducts[index].ProductQuantityInStock > prod.Quantity)
                    //    prod.ProductQuantityInStock++;
                }
                for (int i = 0; i < selectedProducts.Count; i++)
                {
                    Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
                }
                DeliveryDP.Text = DateTime.Now.AddDays(SetDeliveryDate(selectedProducts)).ToString();
                TBPrice.Text = Cost.ToString();
                ShoeListView.Items.Refresh();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Product;
            prod.Quantity = 0;
            var selectedOP = selectedProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].OrderProductAmount = 0;
            var pr = ShoeListView.SelectedItem as Product;
            selectedOrderProducts.RemoveAt(index);
            selectedProducts.RemoveAt(index);

            for (int i = 0; i < selectedProducts.Count; i++)
            {
                Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
            }
            TBPrice.Text = Cost.ToString();
            ShoeListView.Items.Refresh();
            DeliveryDP.Text = DateTime.Now.AddDays(SetDeliveryDate(selectedProducts)).ToString();
            if (ShoeListView.Items.Count == 0)
            {
                this.Close();
            }
        }
    }
}
