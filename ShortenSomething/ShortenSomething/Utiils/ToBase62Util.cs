namespace ShortenSomething
{
    internal static class ToBase62Util
    {
        private static readonly char[] _base62Chars = "0123456789qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP".ToCharArray();
        private static readonly int _toBase = 62;

        internal static string ConvertToBase62(this long num)
        { 
            Span<char> buffer = stackalloc char[11];

            for (int i = 0; i < buffer.Length; i++)
            {
                var div = num / _toBase;
                var charVal = num - (div * _toBase);
                num = div;

                buffer[i] = _base62Chars[charVal];

                if (num == 0) break;
            }

            return buffer.ToString();
        }

        internal static string ConvertToBase62(this int num)
        {
            Span<char> buffer = stackalloc char[6];

            for (int i = 0; i < buffer.Length; i++)
            {
                var div = num / _toBase;
                var charVal = num - (div * _toBase);
                num = div;

                buffer[i] = _base62Chars[charVal];

                if (num == 0) break;
            }

            return buffer.ToString();
        }
    }
}
