using System.Collections;
using UnityEngine;
using System;
using System.IO;
using Assets.Scripts.Events;

namespace Assets.Scripts.FileManagement
{
    public class FileDownloader : MonoBehaviour
    {
        public static Action<GameErrorEventArgs> ErrorOccurred;
        
        public static string LoadTextFile(string filepath)
        {
            string res = null;
            try
            {
                res = File.ReadAllText(filepath);
            }
            catch (IOException ex)
            {
                ErrorOccurred(new GameErrorEventArgs(ex.Message));
            }
            return res;
        }

        public static byte[] LoadBinaryFile(string filepath)
        {
            byte[] res = null;
            try
            {
                res = File.ReadAllBytes(filepath);
            }
            catch (IOException ex)
            {
                ErrorOccurred(new GameErrorEventArgs(ex.Message));
            }
            return res;
        }
    }
}