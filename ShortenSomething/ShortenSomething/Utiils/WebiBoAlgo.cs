namespace ShortenSomething
{
    using System.Security.Cryptography;
    using System.Text;

    internal static class WebiBoAlgo
    {
        private static readonly char[] _base62Chars = "0123456789qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP".ToCharArray();
        private static readonly string _salt = "catcherwong";
        private static readonly string _hexPrefix = "0x";
        private static readonly int _3fffffff = 0x3FFFFFFF;
        private static readonly int _0000003d = 0x0000003D;

        /// <summary>
        /// 算法描述：使用6个字符来表示短链接，我们使用ASCII字符中的'a'-'z','0'-'9','A'-'Z'，共计62个字符做为集合。 
        /// 每个字符有62种状态，六个字符就可以表示62^6（56800235584），那么如何得到这六个字符， 
        /// 具体描述如下： 
        /// 1. 对传入的长URL+设置key值 进行Md5，得到一个32位的字符串(32 字符十六进制数)，即16的32次方； 
        /// 2. 将这32位分成四份，每一份8个字符，将其视作16进制串与0x3fffffff(30位1)与操作, 即超过30位的忽略处理； 
        /// 3. 这30位分成6段, 每5个一组，算出其整数值，然后映射到我们准备的62个字符中, 依次进行获得一个6位的短链接地址。 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static string[] GetShortCodes(this string url)
        {
            var md5 = GetMd5Str($"{_salt}{url}");

            string[] resUrl = new string[4];

            for (int i = 0; i < 4; i++)
            {
                var seg = md5.AsSpan(i * 8, 8);
                var hex = Convert.ToInt32(string.Concat(_hexPrefix, seg), 16);

                int hexint = _3fffffff & hex;
                string tmpCode = string.Empty;

                for (int j = 0; j < 6; j++)
                {
                    int index = _0000003d & hexint;
                    tmpCode = string.Concat(tmpCode, _base62Chars[index]);
                    hexint >>= 5;
                }

                resUrl[i] = tmpCode;
            }

            return resUrl;
        }

        private static string GetMd5Str(string str)
        {
            using MD5 md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var res = BitConverter.ToString(bytes, 0, bytes.Length).Replace("-", "");
            return res;
        }
    }
}