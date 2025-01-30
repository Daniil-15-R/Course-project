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

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для VacinationScreen.xaml
    /// </summary>
    public partial class VacinationScreen : Window
    {
        public VacinationScreen()
        {
            InitializeComponent();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, какую роль пользователя мы сохранили
            if (App.Current.Properties["User  Role"] != null)
            {
                string userRole = App.Current.Properties["User  Role"].ToString();
                if (userRole == "admin")
                {
                    HomeScreen homeScreen = new HomeScreen();
                    homeScreen.Show();
                }
                else if (userRole == "user")
                {
                    HomeScreen2 homeScreen2 = new HomeScreen2();
                    homeScreen2.Show();
                }
            }
            this.Close(); // Закрыть текущее окно
        }
    }
}
