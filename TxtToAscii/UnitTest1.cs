using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextToAscii;

namespace TxtToAscii
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Check_String_Converted_To_Ascii()
        {
            var value = ("hello john").ConvertToASCII();

            Assert.AreEqual("10410110810811132106111104110", value);
        }
    }
}
