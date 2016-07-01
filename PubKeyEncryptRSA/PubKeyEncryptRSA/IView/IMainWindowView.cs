using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PubKeyEncryptRSA.IView
{
    public interface IMainWindowView
    {
        object DataContext { set; get; }

        void Close();
        void Show();
        //void DoWork(Action action);
    }
}
