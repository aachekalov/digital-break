using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

    public static class Extension
    {
        
        /// <summary>
        /// Copies the contents of input to output. Doesnt close either stream.
        /// </summary>
        public static void CopyStream( Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ( (len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            input.Close();
            output.Close();
        }
    }
