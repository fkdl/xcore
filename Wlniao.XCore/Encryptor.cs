/*==============================================================================
    文件名称：Encryptor.cs
    适用环境：CoreCLR 5.0,.NET Framework 2.0/4.0/5.0
    功能描述：HASH算法加密工具类
================================================================================
 
    Copyright 2014 XieChaoyi

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

               http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

===============================================================================*/
using System;
using System.IO;
using System.Security.Cryptography;
namespace Wlniao
{
    /// <summary>
    /// HASH算法加密工具类
    /// </summary>
    public class Encryptor
    {
        /// <summary>
        /// 32位MD5算法加密（多次加密）
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="time">需要加密的次数</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encryptor32(string str, int time)
        {
            do
            {
                str = Md5Encryptor32(str);
                time--;
            } while (time > 0);
            return str;
        }
        /// <summary>
        /// 32位MD5算法加密（大写）
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encryptor32(string str)
        {
            string password = "";
            var s = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            foreach (byte b in s)
            {
                password += b.ToString("X2");
            }
            return password;
        }
        /// <summary>
        /// 16位MD5算法加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encryptor16(string str)
        {
            string password = "";
            var s = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            password = BitConverter.ToString(s, 4, 8).Replace("-", "");
            return password;
        }
        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encrypt(string str)
        {
            return IO.Base64Encoder.Encoder.GetEncoded(Encoding.UTF8.GetBytes(str));
        }
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string str)
        {
            return Encoding.UTF8.GetString(IO.Base64Decoder.Decoder.GetDecoded(str));
        }
        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="pToEncrypt">需要加密的字符串</param>
        /// <param name="sKey">加密密钥</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>返回加密后的密文</returns>
        public static string AesEncrypt(string pToEncrypt, string sKey, string sIV = "")
        {
            var aes = Aes.Create();
            var key = new char[32];
            for (var i = 0; i < key.Length && i < sKey.Length; i++)
            {
                key[i] = sKey[i];
            }
            aes.Key = Encoding.ASCII.GetBytes(key);
            var iv = new char[16];
            for (var i = 0; i < iv.Length && i < sIV.Length; i++)
            {
                iv[i] = sIV[i];
            }
            aes.IV = Encoding.ASCII.GetBytes(iv);
            aes.Padding = PaddingMode.PKCS7;
            var inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
            using (var ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return System.Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">加密密钥</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>返回加密前的明文</returns>
        public static string AesDecrypt(string pToDecrypt, string sKey, string sIV = "")
        {
            try
            {
                var aes = Aes.Create();
                var key = new char[32];
                for (var i = 0; i < key.Length && i < sKey.Length; i++)
                {
                    key[i] = sKey[i];
                }
                aes.Key = Encoding.ASCII.GetBytes(key);
                var iv = new char[16];
                for (var i = 0; i < iv.Length && i < sIV.Length; i++)
                {
                    iv[i] = sIV[i];
                }
                aes.IV = Encoding.ASCII.GetBytes(iv);
                aes.Padding = PaddingMode.PKCS7;
                var inputByteArray = System.Convert.FromBase64String(pToDecrypt);
                using (var ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch { }
            return "";
        }
        /// <summary>
        /// 获取SHA1值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSHA1(string str)
        {
            var dataToHash = Encoding.ASCII.GetBytes(str); //将str转换成byte[]
            var dataHashed = SHA1.Create().ComputeHash(dataToHash);//Hash运算
            return BitConverter.ToString(dataHashed).Replace("-", "");//将运算结果转换成string
        }
        /// <summary>
        /// HMACMD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] GetHMACSHA1(string str, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var hashAlgorithm = new HMACSHA1(keyBytes);
            return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
        }
    }
}