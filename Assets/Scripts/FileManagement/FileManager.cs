using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FileManagement
{
    public class FileManager : MonoBehaviour
    {
        protected const string GAMEDATA_DIR_NAME = "Gamedata";
        protected const string DATA_DIR_NAME = "Data";

        protected const string SETTINGS_FILE_NAME = "Settings.json";
        protected const string PLAYER_PROFILE_DATA_FILE_NAME = "PlayerProfileData.json";

        [HideInInspector] public string gameDataDir;
        [HideInInspector] public string dataDir;

        [HideInInspector] public string settingsFilePath;
        [HideInInspector] public string playerProfileDataFilePath;

        protected void Awake()
        {
            gameDataDir = string.Format("{0}/{1}", Application.persistentDataPath, GAMEDATA_DIR_NAME);
            Directory.CreateDirectory(gameDataDir);
            dataDir = string.Format("{0}/{1}", gameDataDir, DATA_DIR_NAME);
            Directory.CreateDirectory(dataDir);

            settingsFilePath = string.Format("{0}/{1}", gameDataDir, SETTINGS_FILE_NAME);
            playerProfileDataFilePath = string.Format("{0}/{1}", dataDir, PLAYER_PROFILE_DATA_FILE_NAME);
        }
    }
}
