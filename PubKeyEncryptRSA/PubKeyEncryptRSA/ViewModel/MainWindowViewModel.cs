using PubKeyEncryptRSA.IView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PubKeyEncryptRSA.IViewModel;
using System.Windows.Input;
using PubKeyEncryptRSA.Common;
using PubKeyEncryptRSA.Model;
using System.Windows;
using System.Threading;

namespace PubKeyEncryptRSA.ViewModel
{
    public class MainWindowViewModel : NotifyObject, IMainWindowViewModel
    {
        #region 命令
        /// <summary>
        /// 解密命令
        /// </summary>
        public ICommand DescryptCommand { get; set; }
        /// <summary>
        /// 最小化命令
        /// </summary>
        public ICommand MinCommand { get; set; }
        /// <summary>
        /// 最大化命令
        /// </summary>
        public ICommand MaxCommand { get; set; }
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand { get; set; }
        /// <summary>
        /// 拖动命令
        /// </summary>
        public ICommand DragMoveCommand { get; set; }
        #endregion

        #region 属性
        private string m_time;
        /// <summary>
        /// 花费时间
        /// </summary>
        public string Time
        {
            get
            {
                return m_time;
            }
            set
            {
                m_time = value;
                this.RaisePropertyChanged(() => Time);
            }
        }

        private string m_btnContent;
        /// <summary>
        /// 按钮内容
        /// </summary>
        public string btnContent
        {
            get
            {
                return m_btnContent;
            }
            set
            {
                m_btnContent = value;
                this.RaisePropertyChanged(() => btnContent);
            }
        }

        private RSABody m_RSABody;
        /// <summary>
        /// rsa加密解密实体
        /// </summary>
        public RSABody RSABody
        {
            get
            {
                return m_RSABody;
            }
            set
            {
                m_RSABody = value;
                this.RaisePropertyChanged(() => RSABody);
            }
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 页面
        /// </summary>
        private IMainWindowView m_View;
        /// <summary>
        /// 运算线程
        /// </summary>
        private Thread _desThread;
        /// <summary>
        /// 计时线程
        /// </summary>
        private Thread _countThread;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="view"></param>
        public MainWindowViewModel(IMainWindowView view)
        {
            m_View = view;
            m_View.DataContext = this;
            InitData();
            InitCommand();
        }

        /// <summary>
        /// 初始命令
        /// </summary>
        private void InitCommand()
        {
            DescryptCommand = new RelayCommand(DescryptExecute);
            MinCommand = new RelayCommand(MinWindowExecute);
            MaxCommand = new RelayCommand(MaxWindowExecute);
            CloseCommand = new RelayCommand(CloseExecute);
            DragMoveCommand = new RelayCommand(DragMoveExecute);
        }

        /// <summary>
        /// 初始数据
        /// </summary>
        private void InitData()
        {
            btnContent = string.Format("解密");
            RSABody = new RSABody();
            RSABody.SecContent = string.Empty;
        }
        #endregion

        #region 方法处理
        /// <summary>
        /// 解密处理
        /// </summary>
        /// <param name="prarmeter"></param>
        private void DescryptExecute(object prarmeter)
        {
            //m_View.DoWork(WorkDescrypt);
            WorkDescrypt();
            CountTime();
        }

        private void CountTime()
        {
            if (_countThread != null && _desThread.IsAlive)
                _countThread.Abort();
            _countThread = new Thread(new ThreadStart(Count));
            _countThread.IsBackground = true;
            _countThread.Start();
        }
        /// <summary>
        /// 计时
        /// </summary>
        private void Count()
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                TimeSpan span = DateTime.Now - start;
                Time = string.Format("已耗时：{0}:{1}:{2}", span.Hours, span.Minutes, span.Seconds);
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        private void WorkDescrypt()
        {
            if (_desThread != null && _desThread.IsAlive)
                _desThread.Abort();
            _desThread = new Thread(new ThreadStart(Descrypt));
            _desThread.IsBackground = true;
            _desThread.Start();
        }
        /// <summary>
        /// 解密线程方法
        /// </summary>
        private void Descrypt()
        {
            PubKeyEnRSA rsa;
            if (!string.IsNullOrEmpty(RSABody.PrivateKey) && !string.IsNullOrEmpty(RSABody.SecContent))
            {
                rsa = new PubKeyEnRSA(RSABody.PrivateKey);
            }
            else
            {
                rsa = new PubKeyEnRSA();
            }

            if (RSABody.SecContent == "加密" && !string.IsNullOrEmpty(RSABody.DocContent))
            {
                RSABody.SecContent = rsa.EncryptString(RSABody.DocContent);
            }
            else
            {
                RSABody.DocContent = rsa.DecryptString(RSABody.SecContent, false);
            }
            if (_countThread != null && _desThread.IsAlive)
                _countThread.Abort();
        }
        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="parameter"></param>
        private void MinWindowExecute(object parameter)
        {
            if ((m_View as Window) != null)
                (m_View as Window).WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// 最大化窗口
        /// </summary>
        /// <param name="parameter"></param>
        private void MaxWindowExecute(object parameter)
        {
            Window win = (m_View as Window);
            if (win != null)
            {
                win.WindowState = win.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }
        /// <summary>
        ///关闭窗口 
        /// </summary>
        /// <param name="parameter"></param>
        private void CloseExecute(object parameter)
        {
            m_View.Close();
        }

        /// <summary>
        /// 拖动窗口
        /// </summary>
        /// <param name="obj"></param>
        private void DragMoveExecute(object obj)
        {
            Window win = (m_View as Window);
            if (win != null)
            {
                win.DragMove();
            }
        }
        #endregion
    }
}
