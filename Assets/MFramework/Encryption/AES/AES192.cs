namespace Encryption
{
    public sealed class AES192 : AESBase
    {
        public const int KeyLength = 24;
        public static readonly int KeySize = KeyLength * 8;

        protected override int keyLength => KeyLength;
        protected override int keySize => KeySize;
    }
}