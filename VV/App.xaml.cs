using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VV.Models;
using System.Data.Entity;
using VV.ViewModels;
using VV.Views;

namespace VV
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ValidationViewModel validationViewModel = new ValidationViewModel();
            SignIn sign = new SignIn();
            sign.DataContext = validationViewModel;

            Current.MainWindow = sign;
            sign.Show();
        }
    }
}
