using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;

namespace VV.ViewModels
{
    public partial class MainViewModel : Window
    {
        public MainViewModel()
        {
            FindProductCommand = new DelegateCommand(OnExecute, CanExecute);
        }
        public ICommand FindProductCommand { get; }

        private bool CanExecute()
        {
            return true;
        }

        public void OnExecute()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Исполняемые файлы (*.exe)|*.exe|Все файлы (*.*)|*.*";

            if(fileDialog.ShowDialog() == true)
            {
                string selectedFilePath = fileDialog.FileName;

                try
                {
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(selectedFilePath);
                    string programName = fileVersionInfo.ProductName;

                    MessageBox.Show(programName);
                }
                catch
                {
                    MessageBox.Show("Ошибка при получении информации");
                }
            }
        }
    }
}
