using System.Text;

namespace TextToAscii
{
  public  static class stringExtensions
    {
        public static string ConvertToASCII(this string value)
        {
            StringBuilder @return = new StringBuilder();

            foreach (char c in value)
            {
                @return.Append((int)c);
            }
            return @return.ToString();
        }
    }
}
