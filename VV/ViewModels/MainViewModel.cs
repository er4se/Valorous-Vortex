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
using Prism.Mvvm;
using VV.Models;

namespace VV.ViewModels
{
    public partial class MainViewModel : BindableBase
    {
        private GameProduct gameProduct = new GameProduct();

        private string gameName;

        public string GameName
        {
            get { return gameName; }
            set
            {
                if (SetProperty(ref gameName, value))
                {
                    RaisePropertyChanged(nameof(GameName));

                }
            }
        }

        public MainViewModel()
        {
            FindProductCommand = new DelegateCommand(FindProduct_OnExecute, FindProduct_CanExecute);
            StartProductCommand = new DelegateCommand(StartProduct_OnExecute, StartProduct_CanExecute);
        }
        public ICommand FindProductCommand { get; }
        public ICommand StartProductCommand { get; }

        private bool FindProduct_CanExecute()
        {
            return true;
        }

        public void FindProduct_OnExecute()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Исполняемые файлы (*.exe)|*.exe|Все файлы (*.*)|*.*";

            if(fileDialog.ShowDialog() == true)
            {
                string selectedFilePath = fileDialog.FileName;
                gameProduct.GameFileDialog = fileDialog;
                gameProduct.GameProductName = selectedFilePath;

                try
                {
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(selectedFilePath);
                    string programName = fileVersionInfo.ProductName;

                    GameName = programName;
                }
                catch
                {
                    MessageBox.Show("Ошибка при получении информации");
                }
            }
        }

        private bool StartProduct_CanExecute() => true;

        public void StartProduct_OnExecute()
        {
            System.Diagnostics.Process.Start(gameProduct.GameProductName);
        }
    }
}
