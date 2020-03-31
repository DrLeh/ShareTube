using System;

namespace ShareTube.Core.Extensions
{
    public static class GuidEncoder
    {
        public static string Encode(string guidText)
        {
            Guid guid = new Guid(guidText);
            return Encode(guid);
        }

        public static string Encode(this Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22);
        }

        public static bool TryDecode(string encoded, out Guid guid)
        {
            guid = Guid.NewGuid();
            try
            {
                guid = DecodeGuid(encoded);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static Guid DecodeGuid(this string encoded)
        {
            Guid attempt = Guid.Empty;
            if (Guid.TryParse(encoded, out attempt))
                return attempt;

            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(encoded + "==");
            return new Guid(buffer);
        }
    }
}