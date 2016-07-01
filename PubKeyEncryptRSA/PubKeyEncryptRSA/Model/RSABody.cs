using PubKeyEncryptRSA.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PubKeyEncryptRSA.Model
{
    public class RSABody : NotifyObject
    {
        private string m_PrivateKey;
        /// <summary>
        /// 密钥
        /// </summary>
        public string PrivateKey
        {
            get
            {
                return m_PrivateKey;
            }
            set
            {
                m_PrivateKey = value;
                this.RaisePropertyChanged("PrivateKey");
            }
        }

        private string m_PublicKey;
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey
        {
            get
            {
                return m_PublicKey;
            }
            set
            {
                m_PublicKey = value;
                this.RaisePropertyChanged("PublicKey");
            }
        }

        private string m_SecContent;
        /// <summary>
        /// 暗文
        /// </summary>
        public string SecContent
        {
            get
            {
                return m_SecContent;
            }
            set
            {
                m_SecContent = value;
                this.RaisePropertyChanged("SecContent");
            }
        }

        private string m_DocContent;
        /// <summary>
        /// 明文
        /// </summary>
        public string DocContent
        {
            get
            {
                return m_DocContent;
            }
            set
            {
                m_DocContent = value;
                this.RaisePropertyChanged("DocContent");
            }
        }
    }
}
