using PubKeyEncryptRSA.IView;
using PubKeyEncryptRSA.IViewModel;
using PubKeyEncryptRSA.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PubKeyEncryptRSA
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IMainWindowView view = new MainWindow();
            IMainWindowViewModel vm = new MainWindowViewModel(view);
            view.Show();
        }
    }
}
