namespace Encryption
{
    public class AES256 : AESBase
    {
        public const int KeyLength = 32;
        public static readonly int KeySize = KeyLength * 8;

        protected override int keyLength => KeyLength;
        protected override int keySize => KeySize;
    }
}