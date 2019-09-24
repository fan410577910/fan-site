#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：杨焕文 
     * 文件名：  RSACryptoServiceHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  杨焕文 
     * 创建时间：2015/7/13 11:51:26 
     * 描述    : RSA非对称加密解密类，登录采用RSA非对称加密解密
     * =====================================================================
     * 修改时间：2015/7/13 11:51:26 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：支持由OpenSSL生成的公钥/私钥字符串。
*/
#endregion
using System;
using System.Dynamic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FAN.Helper
{
    /// <summary>
    /// RSA非对称加密解密类，登录采用RSA非对称加密解密
    /// </summary>
    public class RSACryptoServiceHelper
    {
        #region 登录
        private const string RSA_PRIVATE_KEY = "MIICXAIBAAKBgQDFUUB2BclL5mH2IKAY2K4nb45gVJRkAgqd2eXOqNKeoTy4cqsd6ShbNWmFoiMsYgTq40XJeU9EJcCq0zTv06vUV4YyPBjpjR4aXT7VmtiploHxy/xR1SEedrlqe82vtKSRTOG1KMRbHSGXYKxs2BUmNUsXGy6eUE+mnnpLlg7dVQIDAQABAoGBAJNGdiqhXwH7zxp6d4SUFkjxBv/3oVqT+4CeZty9PZeRGVVCoV4N7UVGtKA9yKsT7j+zqx8aIIPo+cmHI3fuMc1XSbGk7x0mq2PiMZt54qkra2TXDfhJA9OHcySIw9abNj5i3F/8ZWTGppU7pAcLGeRy1jmx+/fNFhxMmF4UsBOBAkEA9Vvw2VlWwBV22k0W1ngrbe6XtR2KvZ1Tz8Sg0JczPB41s2P7k+Qj/h/bIebwsUwc++GesQhNy+QBygoOtsrMCQJBAM3f73U1QKsIyvaoqAkczDW9ctIBDMPXdhqqHeg2TRTgEmi4w7BFYejgDaqNnpJBODeI7mAyh9GoB9/oHKZAce0CQHGCUl9LDG4av/xoM4uO1pCqE7cvbpMgKLjy/27gtEw4saFHPQkDP39+X1NE0s9DJhmFHHRMIimUVxuQIZSBJ3kCQBCJPCKd9GPIgQTu5xBUT9LBNMfJbc0NMV9S6rQMKITUuqXOsWknKYYa+P4KAgKdWnabeQohBOfCJ1/EtQhlhMECQHJVYRDHj8GAnxOJpA/PaHRzDw87lLBbqWhyuzXrRo4T1RAecoHibS6Glpq6sbezSbofygVMEObvYXWhCEKCxWc=";

        //此公钥与私钥不匹配，加密的数据不能用私钥解密 edit by fanjianyu 2017-6-5
        //private const string RSA_PUBLIC_KEY =
        //     "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDOoZ1xVeMCLag5WJbMGe5gUTd6M1sGuexl4XmwYHk5mYnHHF+2tQCr7HEYNZFLwsGcUgG4mVQqRi13oSx8uESI4H9voexEx8sLwS4rri1plwxQauUvL+ywmCfHBE9s/YHuJDcbAEjAwmGN8fIEgd2Qo7HigR8CgVg0CoaKCFhC0wIDAQAB";

        //此公钥从 JS 中复制
        private const string RSA_PUBLIC_KEY =
             "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDFUUB2BclL5mH2IKAY2K4nb45gVJRkAgqd2eXOqNKeoTy4cqsd6ShbNWmFoiMsYgTq40XJeU9EJcCq0zTv06vUV4YyPBjpjR4aXT7VmtiploHxy/xR1SEedrlqe82vtKSRTOG1KMRbHSGXYKxs2BUmNUsXGy6eUE+mnnpLlg7dVQIDAQAB";

        private readonly RSACryptoServiceProvider _privateKeyRsaProvider;
        private readonly RSACryptoServiceProvider _publicKeyRsaProvider;

        public RSACryptoServiceHelper()
            : this(RSA_PRIVATE_KEY, RSA_PUBLIC_KEY)
        {
        }

        public RSACryptoServiceHelper(string privateKey, string publicKey = null)
        {
            if (!string.IsNullOrEmpty(privateKey))
            {
                this._privateKeyRsaProvider = this.CreateRsaProviderFromPrivateKey(privateKey);
            }

            if (!string.IsNullOrEmpty(publicKey))
            {
                this._publicKeyRsaProvider = this.CreateRsaProviderFromPublicKey(publicKey);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text">密文</param>
        /// <returns></returns>
        public string Decrypt(string text)
        {
            if (this._privateKeyRsaProvider == null)
            {
                throw new Exception("_privateKeyRsaProvider is null");
            }
            else if (text.IndexOf(" ") > -1)
            {
                //解决：url地址的加密数据，后台获取后加号会被替换成空格
                text = text.Replace(" ", "+");
            }
            return Encoding.UTF8.GetString(this._privateKeyRsaProvider.Decrypt(Convert.FromBase64String(text), false));
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (this._publicKeyRsaProvider == null)
                {
                    throw new Exception("_publicKeyRsaProvider is null");
                }
                return Convert.ToBase64String(this._publicKeyRsaProvider.Encrypt(Encoding.UTF8.GetBytes(text), false));
            }
            return text;
        }

        private RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(string privateKey)
        {
            byte[] privateKeyBits = Convert.FromBase64String(privateKey);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters rsaParams = new RSAParameters();
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binaryReader.ReadUInt16();
                if (twobytes == 0x8130)
                {
                    binaryReader.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    binaryReader.ReadInt16();
                }
                else
                {
                    throw new Exception("Unexpected value read binr.ReadUInt16()");
                }
                twobytes = binaryReader.ReadUInt16();
                if (twobytes != 0x0102)
                {
                    throw new Exception("Unexpected version");
                }
                bt = binaryReader.ReadByte();
                if (bt != 0x00)
                {
                    throw new Exception("Unexpected value read binr.ReadByte()");
                }
                rsaParams.Modulus = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
                rsaParams.Exponent = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
                rsaParams.D = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
                rsaParams.P = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
                rsaParams.Q = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
                rsaParams.DP = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
                rsaParams.DQ = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
                rsaParams.InverseQ = binaryReader.ReadBytes(this.GetIntegerSize(binaryReader));
            }

            rsa.ImportParameters(rsaParams);
            return rsa;
        }

        private int GetIntegerSize(BinaryReader binaryReader)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binaryReader.ReadByte();
            if (bt != 0x02)
            {
                return 0;
            }
            bt = binaryReader.ReadByte();

            if (bt == 0x81)
            {
                count = binaryReader.ReadByte();
            }
            else if (bt == 0x82)
            {
                highbyte = binaryReader.ReadByte();
                lowbyte = binaryReader.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binaryReader.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binaryReader.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private RSACryptoServiceProvider CreateRsaProviderFromPublicKey(string publicKeyString)
        {
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            int x509size;

            byte[] x509Key = Convert.FromBase64String(publicKeyString);
            x509size = x509Key.Length;
            using (MemoryStream mem = new MemoryStream(x509Key))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    {
                        binr.ReadByte();    //advance 1 byte
                    }
                    else if (twobytes == 0x8230)
                    {
                        binr.ReadInt16();   //advance 2 bytes
                    }
                    else
                    {
                        return null;
                    }
                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!this.CompareByteArrays(seq, seqOid))    //make sure Sequence for OID is correct
                    {
                        return null;
                    }
                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    {
                        binr.ReadByte();    //advance 1 byte
                    }
                    else if (twobytes == 0x8203)
                    {
                        binr.ReadInt16();   //advance 2 bytes
                    }
                    else
                    {
                        return null;
                    }
                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                    {
                        return null;
                    }
                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    {
                        binr.ReadByte();    //advance 1 byte
                    }
                    else if (twobytes == 0x8230)
                    {
                        binr.ReadInt16();   //advance 2 bytes
                    }
                    else
                    {
                        return null;
                    }
                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                    {
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    }
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                    {
                        return null;
                    }
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                    {
                        return null;
                    }
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    RSAParameters rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);

                    return rsa;
                }
            }
        }

        private bool CompareByteArrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                {
                    return false;
                }
                i++;
            }
            return true;
        }

        #endregion

        #region WebAPI http://www.360doc.com/content/14/0319/16/1355383_361905993.shtml
        /// <summary>
        /// 生成密钥对
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public static dynamic GenarateRSAKeyPair(int keySize = 1024)
        {
            dynamic rsaModel = new ExpandoObject();
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(keySize))
            {
                string publicKey = rsaModel.publicKey = provider.ToXmlString(false);//公钥
                string privateKey = rsaModel.privateKey = provider.ToXmlString(true);//私钥
                rsaModel.publicKeyXml = publicKey.Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                rsaModel.privateKeyXml = privateKey.Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;").Replace("\"", "&quot;");
            }
            return rsaModel;
        }
        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="publicKey">KEY是XML格式的</param>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string RSAEncryptXML(string publicKey, string plainText)
        {
            string crypoText = null;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(publicKey);
                int maxBlockSize = rsaProvider.KeySize / 8 - 11;    //加密块最大长度限制
                byte[] plainTexts = Encoding.UTF8.GetBytes(plainText);
                if (plainTexts.Length <= maxBlockSize)
                {
                    crypoText = Convert.ToBase64String(rsaProvider.Encrypt(plainTexts, false));
                }
                else
                {
                    using (MemoryStream plainStream = new MemoryStream(plainTexts))
                    {
                        using (MemoryStream crpyStream = new MemoryStream())
                        {
                            byte[] fromBuffers = new byte[maxBlockSize];
                            int blockSize = plainStream.Read(fromBuffers, 0, maxBlockSize);

                            while (blockSize > 0)
                            {
                                byte[] toEncrypts = new byte[blockSize];
                                Array.Copy(fromBuffers, 0, toEncrypts, 0, blockSize);
                                Byte[] cryptoBuffers = rsaProvider.Encrypt(toEncrypts, false);
                                crpyStream.Write(cryptoBuffers, 0, cryptoBuffers.Length);
                                blockSize = plainStream.Read(fromBuffers, 0, maxBlockSize);
                            }
                            crypoText = Convert.ToBase64String(crpyStream.ToArray(), Base64FormattingOptions.None);
                        }
                    }
                }
            }
            return crypoText;
        }
        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="privateKey">KEY是XML格式的</param>
        /// <param name="crpyoText">密文</param>
        /// <returns>明文</returns>
        public static string RSADecryptXML(string privateKey, string crpyoText)
        {
            string plainText = null;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(privateKey);
                byte[] crpyoTexts = Convert.FromBase64String(crpyoText);
                int maxBlockSize = rsaProvider.KeySize / 8;    //解密块最大长度限制
                if (crpyoTexts.Length <= maxBlockSize)
                {
                    plainText = Encoding.UTF8.GetString(rsaProvider.Decrypt(crpyoTexts, false));
                }
                else
                {
                    using (MemoryStream crypStream = new MemoryStream(crpyoTexts))
                    {
                        using (MemoryStream plainStream = new MemoryStream())
                        {
                            byte[] buffers = new byte[maxBlockSize];
                            int blockSize = crypStream.Read(buffers, 0, maxBlockSize);
                            while (blockSize > 0)
                            {
                                byte[] toDecrypts = new byte[blockSize];
                                Array.Copy(buffers, 0, toDecrypts, 0, blockSize);

                                byte[] plainTexts = rsaProvider.Decrypt(toDecrypts, false);
                                plainStream.Write(plainTexts, 0, plainTexts.Length);
                                blockSize = crypStream.Read(buffers, 0, maxBlockSize);
                            }
                            plainText = Encoding.UTF8.GetString(plainStream.ToArray());
                        }
                    }
                }
            }
            return plainText;
        }

        /// <summary>
        /// 签名(RSA)
        /// </summary>
        /// <param name="privateKey">私钥（KEY是XML格式的）</param>
        /// <param name="input">要被签名的数据</param>
        /// <returns>数据签名之后的结果</returns>
        public static string SignatureRSA(string privateKey, string input)
        {
            string signature = null;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(privateKey);
                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsaProvider);
                rsaFormatter.SetHashAlgorithm("MD5");
                byte[] bytes = Convert.FromBase64String(input);
                signature = Convert.ToBase64String(rsaFormatter.CreateSignature(bytes));
            }
            return signature;
        }
        /// <summary>
        /// 签名(MD5)
        /// </summary>
        /// <param name="privateKey">私钥（KEY是XML格式的）</param>
        /// <param name="input">要被签名的数据</param>
        /// <returns>数据签名之后的结果</returns>
        public static string SignatureMD5(string privateKey, string input)
        {
            string signature = null;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(privateKey);
                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsaProvider);
                rsaFormatter.SetHashAlgorithm("MD5");
                using (MD5 md5 = MD5.Create())
                {
                    byte[] bytes = Convert.FromBase64String(input);
                    byte[] hashs = md5.ComputeHash(bytes);
                    signature = Convert.ToBase64String(rsaFormatter.CreateSignature(hashs));
                }
            }
            return signature;
        }
        /// <summary>
        /// 验证签名(RSA)
        /// </summary>
        /// <param name="publickKey">公钥（KEY是XML格式的）</param>
        /// <param name="input">被验证的数据</param>
        /// <param name="signText">签名</param>
        /// <returns></returns>
        public static bool VerifySignatureRSA(string publickKey, string input, string signText)
        {
            bool valid = false;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(publickKey);
                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsaProvider);
                rsaDeformatter.SetHashAlgorithm("MD5");
                byte[] bytes = Convert.FromBase64String(input);
                valid = rsaDeformatter.VerifySignature(bytes, Convert.FromBase64String(signText));
            }
            return valid;
        }
        /// <summary>
        /// 验证签名(MD5)
        /// </summary>
        /// <param name="publickKey">公钥（KEY是XML格式的）</param>
        /// <param name="input">被验证的数据</param>
        /// <param name="signText">签名</param>
        /// <returns></returns>
        public static bool VerifySignatureMD5(string publickKey, string input, string signText)
        {
            bool valid = false;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(publickKey);
                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsaProvider);
                rsaDeformatter.SetHashAlgorithm("MD5");
                using (MD5 md5 = MD5.Create())
                {
                    byte[] bytes = Convert.FromBase64String(input);
                    byte[] hashs = md5.ComputeHash(bytes);
                    valid = rsaDeformatter.VerifySignature(hashs, Convert.FromBase64String(signText));
                }
            }
            return valid;
        }
        #endregion

        /// <summary>
        /// 从字符串中取得Hash描述 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Hash(string input)
        {
            string hashInput = null;
            //usign(HashAlgorithm md5 = HashAlgorithm.Create("MD5"))
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputs = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashInputs = md5.ComputeHash(inputs);
                hashInput = Convert.ToBase64String(hashInputs);
            }
            return hashInput;
        }
    }
}
