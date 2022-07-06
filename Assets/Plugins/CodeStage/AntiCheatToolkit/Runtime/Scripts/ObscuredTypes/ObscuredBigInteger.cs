#if (UNITY_WINRT || UNITY_WINRT_10_0 || UNITY_WSA || UNITY_WSA_10_0) && !ENABLE_IL2CPP
#define ACTK_UWP_NO_IL2CPP
#endif

namespace CodeStage.AntiCheat.ObscuredTypes
{
    using System;
    using UnityEngine;
    using Utils;
    using System.Numerics;

    //[Serializable]
    public struct ObscuredBigInteger : IObscuredType, IFormattable, IEquatable<ObscuredBigInteger>, IComparable<ObscuredBigInteger>, IComparable<BigInteger>, IComparable
    {
        [SerializeField]
        private BigInteger currentCryptoKey;

        [SerializeField]
        private BigInteger hiddenValue;

        [SerializeField]
        private bool inited;

        [SerializeField]
        private BigInteger fakeValue;

        [SerializeField]
        private bool fakeValueActive;

        private ObscuredBigInteger(BigInteger value)
        {
            currentCryptoKey = GenerateKey();
            hiddenValue = Encrypt(value, currentCryptoKey);

#if UNITY_EDITOR
            fakeValue = value;
            fakeValueActive = true;
#else
			var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = detectorRunning ? value : 0;
			fakeValueActive = detectorRunning;
#endif
            inited = true;
        }

        /// <summary>
		/// Encrypts passed value using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static BigInteger Encrypt(BigInteger value, BigInteger key)
        {
            return value ^ key;
        }

        /// <summary>
        /// Decrypts passed value you got from Encrypt() using same key.
        /// </summary>
        /// \sa Encrypt()
        public static BigInteger Decrypt(BigInteger value, BigInteger key)
        {
            return value ^ key;
        }

        /// <summary>
        /// Creates and fills obscured variable with raw encrypted value previously got from GetEncrypted().
        /// </summary>
        /// Literally does same job as SetEncrypted() but makes new instance instead of filling existing one,
        /// making it easier to initialize new variables from saved encrypted values.
        ///
        /// <param name="encrypted">Raw encrypted value you got from GetEncrypted().</param>
        /// <param name="key">Encryption key you've got from GetEncrypted().</param>
        /// <returns>New obscured variable initialized from specified encrypted value.</returns>
        /// \sa GetEncrypted(), SetEncrypted()
        public static ObscuredBigInteger FromEncrypted(BigInteger encrypted, BigInteger key)
        {
            var instance = new ObscuredBigInteger();
            instance.SetEncrypted(encrypted, key);
            return instance;
        }

        /// <summary>
        /// Generates random key. Used internally and can be used to generate key for manual Encrypt() calls.
        /// </summary>
        /// <returns>Key suitable for manual Encrypt() calls.</returns>
        public static BigInteger GenerateKey()
        {
            return RandomUtils.GenerateIntKey();
        }

        /// <summary>
        /// Allows to pick current obscured value as is.
        /// </summary>
        /// <param name="key">Encryption key needed to decrypt returned value.</param>
        /// <returns>Encrypted value as is.</returns>
        /// Use it in conjunction with SetEncrypted().<br/>
        /// Useful for saving data in obscured state.
        /// \sa FromEncrypted(), SetEncrypted()
        public BigInteger GetEncrypted(out BigInteger key)
        {
            key = currentCryptoKey;
            return hiddenValue;
        }

        /// <summary>
        /// Allows to explicitly set current obscured value. Crypto key should be same as when encrypted value was got with GetEncrypted().
        /// </summary>
        /// Use it in conjunction with GetEncrypted().<br/>
        /// Useful for loading data stored in obscured state.
        /// \sa FromEncrypted()
        public void SetEncrypted(BigInteger encrypted, BigInteger key)
        {
            inited = true;
            hiddenValue = encrypted;
            currentCryptoKey = key;

            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
            {
                fakeValueActive = false;
                fakeValue = InternalDecrypt();
                fakeValueActive = true;
            }
            else
            {
                fakeValueActive = false;
            }
        }

        /// <summary>
        /// Alternative to the type cast, use if you wish to get decrypted value
        /// but can't or don't want to use cast to the regular type.
        /// </summary>
        /// <returns>Decrypted value.</returns>
        public BigInteger GetDecrypted()
        {
            return InternalDecrypt();
        }

        public void RandomizeCryptoKey()
        {
            hiddenValue = InternalDecrypt();
            currentCryptoKey = GenerateKey();
            hiddenValue = Encrypt(hiddenValue, currentCryptoKey);
        }

        private BigInteger InternalDecrypt()
        {
            if (!inited)
            {
                currentCryptoKey = GenerateKey();
                hiddenValue = Encrypt(0, currentCryptoKey);
                fakeValue = 0;
                fakeValueActive = false;
                inited = true;

                return 0;
            }

            var decrypted = Decrypt(hiddenValue, currentCryptoKey);

            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != fakeValue)
            {
                Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }

            return decrypted;
        }


        #region operators, overrides, interface implementations

        //! @cond
        public static implicit operator ObscuredBigInteger(int value)
        {
            return new ObscuredBigInteger(value);
        }
        public static implicit operator ObscuredBigInteger(BigInteger value)
        {
            return new ObscuredBigInteger(value);
        }

        public static implicit operator BigInteger(ObscuredBigInteger value)
        {
            return value.InternalDecrypt();
        }

        //public static implicit operator ObscuredFloat(ObscuredBigInteger value)
        //{
        //    return value.InternalDecrypt();
        //}

        //public static implicit operator ObscuredDouble(ObscuredBigInteger value)
        //{
        //    return value.InternalDecrypt();
        //}

        public static explicit operator ObscuredUInt(ObscuredBigInteger value)
        {
            return (uint)value.InternalDecrypt();
        }

        public static ObscuredBigInteger operator +(ObscuredBigInteger lhs, ObscuredBigInteger rhs)
        {
            return lhs.InternalDecrypt() + rhs.InternalDecrypt();
        }
        public static ObscuredBigInteger operator +(ObscuredBigInteger lhs, BigInteger rhs)
        {
            return lhs.InternalDecrypt() + rhs;
        }
        public static ObscuredBigInteger operator +(BigInteger lhs, ObscuredBigInteger rhs)
        {
            return lhs + rhs.InternalDecrypt();
        }
        public static ObscuredBigInteger operator +(ObscuredBigInteger lhs, int rhs)
        {
            return lhs.InternalDecrypt() + rhs;
        }
        public static ObscuredBigInteger operator +(int lhs, ObscuredBigInteger rhs)
        {
            return lhs + rhs.InternalDecrypt();
        }

        public static ObscuredBigInteger operator -(ObscuredBigInteger lhs, ObscuredBigInteger rhs)
        {
            return lhs.InternalDecrypt() - rhs.InternalDecrypt();
        }
        public static ObscuredBigInteger operator -(ObscuredBigInteger lhs, BigInteger rhs)
        {
            return lhs.InternalDecrypt() - rhs;
        }
        public static ObscuredBigInteger operator -(BigInteger lhs, ObscuredBigInteger rhs)
        {
            return lhs - rhs.InternalDecrypt();
        }
        public static ObscuredBigInteger operator -(ObscuredBigInteger lhs, int rhs)
        {
            return lhs.InternalDecrypt() - rhs;
        }
        public static ObscuredBigInteger operator -(int lhs, ObscuredBigInteger rhs)
        {
            return lhs - rhs.InternalDecrypt();
        }

        public static ObscuredBigInteger operator ++(ObscuredBigInteger input)
        {
            return Increment(input, 1);
        }

        public static ObscuredBigInteger operator --(ObscuredBigInteger input)
        {
            return Increment(input, -1);
        }

        private static ObscuredBigInteger Increment(ObscuredBigInteger input, int increment)
        {
            var decrypted = input.InternalDecrypt() + increment;
            input.hiddenValue = Encrypt(decrypted, input.currentCryptoKey);

            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
            {
                input.fakeValue = decrypted;
                input.fakeValueActive = true;
            }
            else
            {
                input.fakeValueActive = false;
            }

            return input;
        }

        public override int GetHashCode()
        {
            return InternalDecrypt().GetHashCode();
        }

        public override string ToString()
        {
            return InternalDecrypt().ToString();
        }

        public string ToString(string format)
        {
            return InternalDecrypt().ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return InternalDecrypt().ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return InternalDecrypt().ToString(format, provider);
        }

        public override bool Equals(object obj)
        {
            return obj is ObscuredBigInteger && Equals((ObscuredBigInteger)obj);
        }

        public bool Equals(ObscuredBigInteger obj)
        {
            if (currentCryptoKey == obj.currentCryptoKey)
            {
                return hiddenValue.Equals(obj.hiddenValue);
            }

            return Decrypt(hiddenValue, currentCryptoKey).Equals(Decrypt(obj.hiddenValue, obj.currentCryptoKey));
        }

        public int CompareTo(ObscuredBigInteger other)
        {
            return InternalDecrypt().CompareTo(other.InternalDecrypt());
        }

        public int CompareTo(BigInteger other)
        {
            return InternalDecrypt().CompareTo(other);
        }

        public int CompareTo(object obj)
        {
#if !ACTK_UWP_NO_IL2CPP
            return InternalDecrypt().CompareTo(obj);
#else
			if (obj == null) return 1;
			if (!(obj is int)) throw new ArgumentException("Argument must be int");
			return CompareTo((int)obj);
#endif
        }

        #endregion

    }
}