using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MerchantDistributorService_API.Common
{
    public class CompressionHelper
    {
        public static byte[] DeflateByte(byte[] bytStr) {
            if (bytStr == null)
            {
                return null;
            }
            using (var output=new MemoryStream()) {
                using (var compressor=new Ionic.Zlib.DeflateStream(
                    output, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
                {
                    compressor.Write(bytStr, 0, bytStr.Length);
                }
                return output.ToArray();
            }
        }
    }
}