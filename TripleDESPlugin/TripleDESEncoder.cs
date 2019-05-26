using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EncoderPluginInterface;

namespace TripleDESPlugin
{
    public class TripleDESEncoder: IEncoder
    {
        // Для работы TripleDES требуется вектор инициализации (IV) и ключ (Key)
        // Операции шифрования/деширования должны использовать одинаковые значения IV и Key

        public void EncryptStream(Stream sourcestream, Stream deststream, string key)
        {
            byte[] _key = new byte[24];
            byte[] _IV = new byte[8];
            byte[] myKey = Encoding.ASCII.GetBytes(key);
            for (int i = 0; i < _key.Length; i++) _key[i] = 0;
            for (int i = 0; (i < _key.Length) && (i < myKey.Length); i++) _key[i] = myKey[i];
            for (int i = 0; (i < _key.Length) && (i < _IV.Length); i++) _IV[i] = _key[i];
            _IV.Reverse();

            TripleDES tdes = TripleDESCryptoServiceProvider.Create();
            tdes.IV = _IV;
            tdes.Key = _key;


            var decStream = new CryptoStream(sourcestream, tdes.CreateEncryptor(), CryptoStreamMode.Read);
            deststream.SetLength(0);
            decStream.CopyTo(deststream);
        }

        public void DecryptStream(Stream sourcestream, Stream deststream, string key)
        {
            byte[] _key = new byte[24];
            byte[] _IV = new byte[8];

            byte[] myKey = Encoding.ASCII.GetBytes(key);

            for (int i = 0; i < _key.Length; i++) _key[i] = 0;
            for (int i = 0; (i < _key.Length) && (i < myKey.Length); i++) _key[i] = myKey[i];
            for (int i = 0; (i < _key.Length) && (i < _IV.Length); i++) _IV[i] = _key[i];
            _IV.Reverse();

            TripleDES tdes = TripleDESCryptoServiceProvider.Create();
            tdes.IV = _IV;
            tdes.Key = _key;

            var decStream = new CryptoStream(sourcestream, tdes.CreateDecryptor(), CryptoStreamMode.Read);
            deststream.SetLength(0);
            decStream.CopyTo(deststream);
        }

        public string Expansion { get; } = ".tdes";
    }
}
