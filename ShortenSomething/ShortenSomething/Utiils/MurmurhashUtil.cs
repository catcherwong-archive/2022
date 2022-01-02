namespace ShortenSomething
{
    using Murmur;
    using System.Security.Cryptography;
    using System.Text;

    internal static class MurmurhashUtil
    {
        internal static long GetMurmurHash(this string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            using HashAlgorithm murmur = MurmurHash.Create32();
            byte[] hash = murmur.ComputeHash(data);
            return Math.Abs(BitConverter.ToInt32(hash, 0));
        }
    }
}