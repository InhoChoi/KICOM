using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace kicom {
    public partial class MainWindow {

        private void button_Click(object sender, RoutedEventArgs e) {
            //MyView.Source = ImageLoad("pack://application:,,,/Resource/Settings-64.png");
        }

        private void CloseKicom(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        
        private void minimaizeKicoom(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void MaximaizeKicom(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Maximized;
            this.Maximaize.Visibility = Visibility.Hidden;
            this.unMaximaize.Visibility = Visibility.Visible;
        }
        
        private void UnMaximaizeKicom(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Normal;
            this.Maximaize.Visibility = Visibility.Visible;
            this.unMaximaize.Visibility = Visibility.Hidden;
        }

        private void MoveHome(object sender, RoutedEventArgs e) {
            this.HomeButton.Background = Brushes.DodgerBlue;
            this.Home_icon_white.Visibility = Visibility.Visible;
            this.Home_icon_blue.Visibility = Visibility.Hidden;
            this.Home_text.Foreground = Brushes.White;

            
            this.HistoryButton.Background = Brushes.Transparent;
            this.History_icon_white.Visibility = Visibility.Hidden;
            this.History_icon_blue.Visibility = Visibility.Visible;
            this.History_text.Foreground = Brushes.DodgerBlue;
            
            this.RegisterButton.Background = Brushes.Transparent;
            this.Register_icon_white.Visibility = Visibility.Hidden;
            this.Register_icon_blue.Visibility = Visibility.Visible;
            this.Register_text.Foreground = Brushes.DodgerBlue;

            this.HomePage.Visibility = Visibility.Visible;
            this.HistoryPage.Visibility = Visibility.Hidden;
            this.RegisterPage.Visibility = Visibility.Hidden;
        }

        private void MoveHistory(object sender, RoutedEventArgs e) {
            this.HistoryButton.Background = Brushes.DodgerBlue;
            this.History_icon_white.Visibility = Visibility.Visible;
            this.History_icon_blue.Visibility = Visibility.Hidden;
            this.History_text.Foreground = Brushes.White;

            this.HomeButton.Background = Brushes.Transparent;
            this.Home_icon_white.Visibility = Visibility.Hidden;
            this.Home_icon_blue.Visibility = Visibility.Visible;
            this.Home_text.Foreground = Brushes.DodgerBlue;
            
            this.RegisterButton.Background = Brushes.Transparent;
            this.Register_icon_white.Visibility = Visibility.Hidden;
            this.Register_icon_blue.Visibility = Visibility.Visible;
            this.Register_text.Foreground = Brushes.DodgerBlue;

            this.HomePage.Visibility = Visibility.Hidden;
            this.HistoryPage.Visibility = Visibility.Visible;
            this.RegisterPage.Visibility = Visibility.Hidden;

            this.HistoryPage.refreshImageList();
        }

        private void MoveRegister(object sender, RoutedEventArgs e) {
            this.RegisterButton.Background = Brushes.DodgerBlue;
            this.Register_icon_white.Visibility = Visibility.Visible;
            this.Register_icon_blue.Visibility = Visibility.Hidden;
            this.Register_text.Foreground = Brushes.White;
            
            this.HistoryButton.Background = Brushes.Transparent;
            this.History_icon_white.Visibility = Visibility.Hidden;
            this.History_icon_blue.Visibility = Visibility.Visible;
            this.History_text.Foreground = Brushes.DodgerBlue;
            
            this.HomeButton.Background = Brushes.Transparent;
            this.Home_icon_white.Visibility = Visibility.Hidden;
            this.Home_icon_blue.Visibility = Visibility.Visible;
            this.Home_text.Foreground = Brushes.DodgerBlue;

            this.HomePage.Visibility = Visibility.Hidden;
            this.HistoryPage.Visibility = Visibility.Hidden;
            this.RegisterPage.Visibility = Visibility.Visible;
        }
    }
}
