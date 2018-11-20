using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Schedule_WPF.ModelViews;
using Schedule_WPF.Views;

namespace Schedule_WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.MainWindow = new MainContainer();
            this.MainWindow.DataContext = new MenuViewModel();
            this.MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
