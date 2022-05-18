using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public static class AES
    {
        public enum KeySizeType : byte
        {
            L128 = 1,
            L192 = 2,
            L256 = 3,
        }

        public static string Encrypt(KeySizeType keyType, byte[] key, byte[] iv, string data) => Encoding.UTF8.GetString(Encrypt(keyType, key, iv, Encoding.UTF8.GetBytes(data)));
        public static byte[] Encrypt(KeySizeType keyType, byte[] key, byte[] iv, byte[] data) => DoCrypt(true, keyType, key, iv, data, 0, data.Length);
        public static byte[] Encrypt(KeySizeType keyType, byte[] key, byte[] iv, byte[] data, int offset, int count) => DoCrypt(true, keyType, key, iv, data, offset, count);

        public static string Decrypt(KeySizeType keyType, byte[] key, byte[] iv, string data) => Encoding.UTF8.GetString(Decrypt(keyType, key, iv, Encoding.UTF8.GetBytes(data)));
        public static byte[] Decrypt(KeySizeType keyType, byte[] key, byte[] iv, byte[] data) => DoCrypt(false, keyType, key, iv, data, 0, data.Length);
        public static byte[] Decrypt(KeySizeType keyType, byte[] key, byte[] iv, byte[] data, int offset, int count) => DoCrypt(false, keyType, key, iv, data, offset, count);

        static byte[] DoCrypt(bool isEncrypt, KeySizeType keyType, byte[] key, byte[] iv, byte[] data, int offset, int count)
        {
            using (var rijndael = new RijndaelManaged())
            {
                AESBase.SetRijndaelDefault(rijndael, KeySize(keyType), key, iv);
                AESBase.KeyCheck(rijndael, KeyLength(keyType));
                return AESBase.CryptoTransform(isEncrypt ? rijndael.CreateEncryptor() : rijndael.CreateDecryptor(), data, offset, count);
            }
        }

        public static int KeySize(KeySizeType size)
        {
            switch (size)
            {
            case KeySizeType.L128: return 128;
            case KeySizeType.L192: return 192;
            case KeySizeType.L256: return 256;
            default: return 0;
            };
        }

        public static int KeyLength(KeySizeType size)
        {
            switch(size)
            {
            case KeySizeType.L128: return 16;
            case KeySizeType.L192: return 24;
            case KeySizeType.L256: return 32;
            default: return 0;
            };
        }
    }
}