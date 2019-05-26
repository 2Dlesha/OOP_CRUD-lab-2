using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EncoderPluginInterface;

namespace DESEncoder
{
    public class DESEncoder : IEncoder
    {
        public void Encode(Stream stream, string key)
        {
            DES desEncoder = DES.Create("DES");
            //byte[] _key = desEncoder.Key;
           // byte[] _IV = desEncoder.IV;
            ///desEncoder.KeySize = 56;
            //byte[] myKey = Encoding.ASCII.GetBytes(key);
            //byte[] byteKey = new byte[14];
            //for (int i = 0; i < byteKey.Length; i++) byteKey[i] = 0;
            //for (int i = 0; (i < byteKey.Length) && (i < myKey.Length); i++) byteKey[i] = myKey[i];

            //desEncoder.Key = byteKey;
            CryptoStream cryptStream = new CryptoStream(stream, desEncoder.CreateEncryptor(desEncoder.Key, desEncoder.IV), CryptoStreamMode.Write);

            // Write the data to the stream 
            // to encrypt it.
            //sWriter.WriteLine(Data);
        }
        public void Decode(Stream stream, string key)
        {
            DES desEncoder = DES.Create("DES");

            //desEncoder.KeySize = 56;
            //byte[] myKey = Encoding.ASCII.GetBytes(key);
            //byte[] byteKey = new byte[14];
            //for (int i = 0; i < byteKey.Length; i++) byteKey[i] = 0;
            //for (int i = 0; (i < byteKey.Length) && (i < myKey.Length); i++) byteKey[i] = myKey[i];
            CryptoStream cryptStream = new CryptoStream(stream, desEncoder.CreateDecryptor(desEncoder.Key, desEncoder.IV), CryptoStreamMode.Write);
        }

        public string Expansion { get; } = ".des";
    }
}

