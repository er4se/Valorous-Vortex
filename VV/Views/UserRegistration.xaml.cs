﻿using System;
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
using VV.ViewModels;

namespace VV.Views
{
    /// <summary>
    /// Логика взаимодействия для UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Window
    {
        public UserRegistration()
        {
            InitializeComponent();
            RegistrationViewModel registrationViewModel = new RegistrationViewModel();
            this.DataContext = registrationViewModel;
            if(registrationViewModel.CloseAction == null )
            {
                registrationViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
