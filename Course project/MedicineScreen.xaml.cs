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
    /// Логика взаимодействия для MedicineScreen.xaml
    /// </summary>
    public partial class MedicineScreen : Window
    {
        public MedicineScreen()
        {
            InitializeComponent();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Здесь тоже нужно будет сохранить или получить текущего пользователя, если нужно
            this.Close();
        }
    }
}
