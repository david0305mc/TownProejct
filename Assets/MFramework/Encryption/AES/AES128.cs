﻿namespace Encryption
{
    public sealed class AES128 : AESBase
    {
        public const int KeyLength = 16;
        public static readonly int KeySize = KeyLength * 8;

        protected override int keyLength => KeyLength;
        protected override int keySize => KeySize;
    }
}