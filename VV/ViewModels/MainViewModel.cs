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
using System.Collections.ObjectModel;

namespace VV.ViewModels
{
    public partial class MainViewModel : BindableBase
    {
        private readonly GameProductDbService gameProductDbService;

        private ObservableCollection<GameProduct> gameProducts;
        public ObservableCollection<GameProduct> GameProducts
        {
            get { return gameProducts; }
            set
            {
                gameProducts = value;
                RaisePropertyChanged(nameof(GameProducts));
            }
        }

        private GameProduct gameProduct = new GameProduct(); //Возможно надо будет перенести в более защищенное место
        private User currentUser = new User();

        public string GameName
        {
            get { return gameProduct.GameProductName; }
            set
            {
                gameProduct.GameProductName = value;
                RaisePropertyChanged(nameof(GameName));
            }
        }

        public byte[] GameSplashScreen
        {
            get { return gameProduct.GameProductSplashScreen; }
            set
            {
                gameProduct.GameProductSplashScreen = value;
                RaisePropertyChanged(nameof(GameSplashScreen));
            }
        }

        public void UserBroadcast(User tempUser)
        {
            if(tempUser.IsValid) currentUser = tempUser;
        }

        public string CurrentUserNickname
        {
            get { return currentUser.NickName + ""; }
        }

        public MainViewModel()
        {
            string connectionString = "server=DESKTOP-QOVSA9S;Trusted_Connection=Yes;DataBase=VVDB;";
            this.gameProductDbService = new GameProductDbService(connectionString);

            FindProductCommand = new DelegateCommand(FindProduct_OnExecute, FindProduct_CanExecute); //Поиск .exe в проводнике
            StartProductCommand = new DelegateCommand(StartProduct_OnExecute, StartProduct_CanExecute); //Запуск процесса игры
        }
        public ICommand FindProductCommand { get; }
        public ICommand StartProductCommand { get; }

        private bool FindProduct_CanExecute() => true;

        public void FindProduct_OnExecute()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Исполняемые файлы (*.exe)|*.exe|Все файлы (*.*)|*.*";

            if(fileDialog.ShowDialog() == true)
            {
                string selectedFilePath = fileDialog.FileName; //Некоторые костыли
                gameProduct.GameFileDialog = fileDialog;       //Возможно стоит добавить конкретные методы для инициализации GameProduct
                gameProduct.GameProductName = selectedFilePath;

                try
                {
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(selectedFilePath);
                    string programName = fileVersionInfo.ProductName;

                    //GameName = programName;
                    
                    gameProductDbService.SelectGameProduct(programName);
                    gameProduct = gameProductDbService.setGameProduct();

                    //if (gameProduct.GameProductSplashScreen != null) MessageBox.Show("nonull");

                    GameName = programName;

                    if(gameProduct.GameProductSplashScreen != null) 
                        GameSplashScreen = gameProduct.GameProductSplashScreen;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
