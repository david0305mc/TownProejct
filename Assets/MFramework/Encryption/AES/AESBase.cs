using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public abstract class AESBase
    {
        const string TAG = nameof(AESBase);

        readonly RijndaelManaged rijndael = new RijndaelManaged();
        protected abstract int keyLength { get; }
        protected abstract int keySize { get; }



        void Clear() => rijndael?.Clear();
        public virtual void Close() => Clear();



        public string Encrypt(string data) => Encoding.UTF8.GetString(Encrypt(Encoding.UTF8.GetBytes(data)));
        public byte[] Encrypt(byte[] data) => DoCrypt(true, data, 0, data.Length);
        public byte[] Encrypt(byte[] data, int offset, int count) => DoCrypt(true, data, offset, count);

        public string Decrypt(string data) => Encoding.UTF8.GetString(Decrypt(Encoding.UTF8.GetBytes(data)));
        public byte[] Decrypt(byte[] data) => DoCrypt(false, data, 0, data.Length);
        public byte[] Decrypt(byte[] data, int offset, int count) => DoCrypt(false, data, offset, count);

        byte[] DoCrypt(bool isEncrypt, byte[] data, int offset, int count)
        {
            KeyCheck(rijndael, keyLength);
            return CryptoTransform(isEncrypt ? rijndael.CreateEncryptor() : rijndael.CreateDecryptor(), data, offset, count);
        }



        public void SetKey(byte[] key, byte[] iv) => SetRijndaelDefault(rijndael, keySize, key, iv);
        public static void SetRijndaelDefault(RijndaelManaged rijndael, int keySize, byte[] key, byte[] iv)
        {
            rijndael.KeySize = keySize;
            rijndael.BlockSize = 128;
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;
            rijndael.Key = key;
            rijndael.IV = iv;
        }

        public static void KeyCheck(RijndaelManaged rijndael, int keyLength)
        {
            var lenKey = rijndael.Key?.Length ?? 0;
            if (lenKey != keyLength)
                throw new ArgumentException($"{TAG}::{nameof(KeyCheck)} : Invalid key length ({lenKey}/{keyLength})");

            var lenIV = rijndael.IV?.Length ?? 0;
            if (lenIV != 16)
                throw new ArgumentException($"{TAG}::{nameof(KeyCheck)} : Invalid iv length ({lenIV}/{16})");
        }

        public static byte[] CryptoTransform(ICryptoTransform crypto, byte[] data) => CryptoTransform(crypto, data, 0, data.Length);
        public static byte[] CryptoTransform(ICryptoTransform crypto, byte[] data, int offset, int count)
        {
            if (offset < 0)     throw new ArgumentException($"{TAG}::{nameof(CryptoTransform)} : Offset is less than 0");
            if (count < 0)      throw new ArgumentException($"{TAG}::{nameof(CryptoTransform)} : Count is less than 0");
            if (data == null)   throw new ArgumentException($"{TAG}::{nameof(CryptoTransform)} : Data is null");
            if (data.Length < offset + count) throw new ArgumentException($"{TAG}::{nameof(CryptoTransform)} : Data is not long enough.");

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, crypto, CryptoStreamMode.Write))
                    cs.Write(data, offset, count);

                return ms.ToArray();
            }
        }
    }
}