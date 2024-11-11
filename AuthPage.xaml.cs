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
using System.Windows.Threading;

namespace Khabirova41Size
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    
    public partial class AuthPage : Page
    {
        DispatcherTimer _timer;
        TimeSpan _time;
        public AuthPage()
        {
            InitializeComponent();

        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PassTB.Text;

            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            User user = Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                LoginTB.Text = "";
                PassTB.Text = "";
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                LoginBtn.IsEnabled = false;

                int timerSec = 10;
                _time = TimeSpan.FromSeconds(10);

                _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    TimeTB.Text = "повторите вход через " + timerSec.ToString() + " сек.";
                    if (_time == TimeSpan.Zero)
                    {
                        _timer.Stop();
                        LoginBtn.IsEnabled = true;
                        TimeTB.Text = "";
                    }
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                    timerSec--;
                }, Application.Current.Dispatcher);

                _timer.Start();
            }
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage(null));
            LoginTB.Text = "";
            PassTB.Text = "";
        }
    }
}
