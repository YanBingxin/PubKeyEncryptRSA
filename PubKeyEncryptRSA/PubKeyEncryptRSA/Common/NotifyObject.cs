using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace PubKeyEncryptRSA.Common
{
    /// <summary>
    /// 属性改变基类
    /// 2016年3月8日17:15:40
    /// </summary>
    public class NotifyObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 激发属性改变
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 监听属性改变并发出通知
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="proertyExpression">属性表达式</param>
        public void RaisePropertyChanged<T>(Expression<Func<T>> proertyExpression)
        {
            if (PropertyChanged != null)
            {
                string propertyName = GetPropertyName<T>(proertyExpression);
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 从表达式获取属性名
        /// </summary>
        /// <typeparam name="T">属性</typeparam>
        /// <param name="proertyExpression">Linq表达式</param>
        /// <returns></returns>
        private string GetPropertyName<T>(Expression<Func<T>> proertyExpression)
        {
            if (proertyExpression == null)
            {
                throw new ArgumentNullException("proertyExpression");
            }
            MemberExpression mp = proertyExpression.Body as MemberExpression;
            if (mp == null)
            {
                throw new ArgumentException("Invalid Argument", "proertyExpression");
            }
            PropertyInfo proInfo = mp.Member as PropertyInfo;
            if (proInfo == null)
            {
                throw new ArgumentException("Argument is not a property", "proertyExpression");
            }
            return proInfo.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
