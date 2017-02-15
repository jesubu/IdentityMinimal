using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMinimal.Config
{
    static class Certificate
    {
        public static X509Certificate2 Get()
        {
            var assembly = typeof(Certificate).Assembly;
            using (var stream = assembly.GetManifestResourceStream("IdentityMinimal.Recursos.IM.pfx"))
            {
                return new X509Certificate2(ReadStream(stream), "1A2B3C4F");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
