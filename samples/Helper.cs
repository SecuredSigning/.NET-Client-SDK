using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Helper
    {
        #region to read code from Console in one line more then 256 charactors.
        /// <summary>
        /// see http://stackoverflow.com/questions/5557889/console-readline-max-length
        /// </summary>
        const int READLINE_BUFFER_SIZE = 1024;
        public static string ReadLine()
        {
            Stream inputStream = Console.OpenStandardInput(READLINE_BUFFER_SIZE);
            byte[] bytes = new byte[READLINE_BUFFER_SIZE];
            int outputLength = inputStream.Read(bytes, 0, READLINE_BUFFER_SIZE);
            //Console.WriteLine(outputLength);
            char[] chars = Encoding.UTF7.GetChars(bytes, 0, outputLength);
            return new string(chars);
        }
        #endregion

    }
}
