using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Assets.Scripts.Utils;
using Assets.Scripts.Events;

namespace Assets.Scripts.FileManagement
{
    public static class FileWriter
    {
        public static Action<GameErrorEventArgs> ErrorOccurred;
        
        public static void SaveTextFile(string filePath, string contents)
        {
            try
            {
                File.WriteAllText(filePath, contents);
            }
            catch (IOException ex)
            {
                if (ErrorOccurred != null)
                {
                    ErrorOccurred(new GameErrorEventArgs(ex.Message));
                }
            }
        }

        public static void SaveBinaryFile(string filePath, byte[] contents)
        {
            try
            {
                File.WriteAllBytes(filePath, contents);
            }
            catch (IOException ex)
            {
                if (ErrorOccurred != null)
                {
                    ErrorOccurred(new GameErrorEventArgs(ex.Message));
                }
            }
        }
    }
}
