using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PubKeyEncryptRSA.Common
{
    /// <summary>
    /// 编号--暂无
    /// RSA加密类（公钥加密，私钥解密）
    /// 颜丙鑫
    /// 2016年3月8日12:49:32
    /// </summary>
    public class PubKeyEnRSA
    {
        #region 私有字段
        /// <summary>
        /// 小分隔符，分隔同一byte[]间
        /// </summary>
        private const char splitMinFlag = '-';
        /// <summary>
        /// 大分隔符，分隔不同byte[]间
        /// </summary>
        private const char splitMaxFlag = '|';
        /// <summary>
        /// 最大安全字符数，超出可能会出异常
        /// </summary>
        private const int safeCount = 58;
        /// <summary>
        /// 默认公有密钥
        /// </summary>
        private const string defaultPubKey = @"<RSAKeyValue><Modulus>sYxs2tAEv9JpYycpJQkOFVhMPa7o0ODQ9jwa"+
                                                @"kpxWt8Gx5NYs+nwWC5sBIApRa0Knlz6Y72z6+lTfArL8//ScCRuDVu8sE"+
                                                @"1VUwyRYc/PmtxzM4fctgWAEtBufkUPthA+E40ydeAAoJTgQng9vmZVvg2D"+
                                                @"5ajNCI7H2j01wGS9Y2fU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        /// <summary>
        /// 默认私有密钥
        /// </summary>
        private const string defaultPriKey = @"<RSAKeyValue><Modulus>sYxs2tAEv9JpYycpJQkOFVhMPa7o0ODQ9jwakpxWt"+
                                                @"8Gx5NYs+nwWC5sBIApRa0Knlz6Y72z6+lTfArL8//ScCRuDVu8sE1VUwyRYc/Pm"+
                                                @"txzM4fctgWAEtBufkUPthA+E40ydeAAoJTgQng9vmZVvg2D5ajNCI7H2j01wGS9Y2f"+
                                                @"U=</Modulus><Exponent>AQAB</Exponent><P>5OCjd7vOcaQ7q6TjawIRjcO4+Q"+
                                                @"7CFv05SJgT07JtR1XWEwZmT7uaXlCds8XpFhjWHMhyhjt+14gKzh4tQXo+7Q==</P>"+
                                                @"<Q>xpakMcpSiDmETdO+8u95FusVrRAXHeM1T2ABXZe/pIjwlKkYuL+b80ybqweRu+q"+
                                                @"SCakdjq3kWCWGAJVkm1keKQ==</Q><DP>tc+siwRpLeTkcaj97pcqEo8TdOTAUTq+M"+
                                                @"mh794KZRD1vXJWM2HGSYrFvVMs2KRN5okuojfDdBaFP1AQstwLA0Q==</DP><DQ>hH"+
                                                @"FbOEmzGV8iubUKmILVw08tuaoNgMVTfIiO4JigG1+o87yC3FQN8gmZmEsICse6de7v"+
                                                @"44Rd0dvUrvbzjhe1YQ==</DQ><InverseQ>IOEaVyOSKRwy7Rz4SIlhL3tYcebnc/"+
                                                @"6rI6eYRNG9biCvMLwVrH8OXw+5b1f/smR8hZa3HHUNAiCXwFFg5bkY2A==</Invers"+
                                                @"eQ><D>ELAySx7iU2Vjon47z89aE4eJIwMdvwRin8JmfQFU5VC/d9Lpx2GCcPTQ1kIi"+
                                                @"BQLu3bh0h4ymyoDPtWE9hpFnWQufFpBlWLJgLVwKTZjawqNHsGkeB5CGiEInq/HUiI"+
                                                @"xBQM//zjb5ejsO/ZfcpSeZMbX+rvHLRcz5L3KaJd4LzgE=</D></RSAKeyValue>";
        #endregion

        #region 属性
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; private set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string PrivateKey { get; private set; }

        private List<byte[]> m_SecBuffer = new List<byte[]>();
        /// <summary>
        /// 密文缓存
        /// </summary>
        public List<byte[]> SecBuffer
        {
            set
            {
                m_SecBuffer = value;
            }
            get
            {
                return m_SecBuffer;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PubKeyEnRSA(bool isProvideNewKey = false)
        {
            if (isProvideNewKey)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    PublicKey = rsa.ToXmlString(false); // 公钥
                    PrivateKey = rsa.ToXmlString(true); // 私钥
                }
            }
            else
            {
                PublicKey = defaultPubKey;
                PrivateKey = defaultPriKey;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="privateKey">私钥</param>
        public PubKeyEnRSA(string publicKey, string privateKey)
        {
            PublicKey = publicKey; // 公钥
            PrivateKey = publicKey; // 私钥
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="privateKey">密钥</param>
        public PubKeyEnRSA(string privateKey)
        {
            PrivateKey = privateKey;
        }
        #endregion

        #region 加解密数据
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="isThrowEx">是否抛出异常</param>
        /// <returns></returns>
        public string EncryptString(string source, bool isThrowEx = false)
        {
            int length = source.Length;
            int startIndex = 0;
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();

            try
            {
                RSACryptoServiceProvider instance = new RSACryptoServiceProvider();
                instance.FromXmlString(PublicKey);

                while (length > 0)
                {
                    int count = length > safeCount ? safeCount : length;
                    string temp = source.Substring(startIndex, count);
                    byte[] sec = instance.Encrypt(Encoding.Default.GetBytes(temp), false);

                    builder.Append(ByteToString(sec));
                    length -= count;
                    startIndex += count;
                }

                result = builder.ToString().TrimEnd(splitMaxFlag);
            }
            catch (Exception ex)
            {
                if (isThrowEx)
                    throw ex;
            }
            return result;
        }

        /// <summary>
        /// 解密暗文
        /// </summary>
        /// <param name="target">暗文</param>
        /// <param name="isThrowEx">是否抛出异常</param>
        /// <returns>返回明文</returns>
        public string DecryptString(string target, bool isThrowEx = false)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            try
            {
                SecBuffer.Clear();
                RSACryptoServiceProvider instance = new RSACryptoServiceProvider();
                instance.FromXmlString(PrivateKey);

                if (FillSecBuffer(target))
                {
                    foreach (byte[] item in SecBuffer)
                    {
                        byte[] sec = instance.Decrypt(item, false);
                        builder.Append(Encoding.Default.GetString(sec));
                    }
                    result = builder.ToString();
                }
            }
            catch (Exception ex)
            {
                result ="解密异常："+ ex.Message;
                if (isThrowEx)
                    throw ex;
            }
            return result;
        }
        #endregion

        #region 字符串<=>byte转换方法
        /// <summary>
        /// 将字符串转换成byte[]填充缓存
        /// </summary>
        /// <param name="target">密文</param>
        /// <returns>是否正确执行</returns>
        private bool FillSecBuffer(string target)
        {
            bool res = true;
            try
            {
                string[] secs = target.Split(splitMaxFlag);
                foreach (string item in secs)
                {
                    string[] childs = item.Split(splitMinFlag);
                    byte[] b = new byte[childs.Length];
                    int i = 0;
                    foreach (string child in childs)
                    {
                        b[i] = byte.Parse(child);
                        i++;
                    }
                    SecBuffer.Add(b);
                }
            }
            catch (Exception ex)
            {
                res = false;
                throw ex;
            }
            return res;
        }
        /// <summary>
        /// byte数组转换为字符串
        /// </summary>
        /// <param name="sec">字节数组</param>
        /// <returns></returns>
        private string ByteToString(byte[] sec)
        {
            string res = string.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (byte b in sec)
            {
                builder.Append(b.ToString());
                builder.Append(splitMinFlag);
            }

            res = builder.ToString().TrimEnd(splitMinFlag) + splitMaxFlag;
            return res;
        }
    }
        #endregion
}

