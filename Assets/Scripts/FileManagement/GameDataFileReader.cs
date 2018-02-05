using Assets.Scripts.Data;
using Assets.Scripts.GameManagement;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FileManagement
{
    public class GameDataFileReader
    {
        protected FileManager fileManager;
        protected GameData gameData;

        public GameDataFileReader (FileManager fileManager, GameData gameData)
        {
            this.fileManager = fileManager;
            this.gameData = gameData;
        }

        public UserProfileData LoadPlayerProfileData()
        {
            UserProfileData playerProfileData;
            if (File.Exists(fileManager.playerProfileDataFilePath))
            {
                string playerProfileDataString = FileDownloader.LoadTextFile(fileManager.playerProfileDataFilePath);
                playerProfileData = SerializeUtils<UserProfileData>.Deserialize(playerProfileDataString);
            }
            else
            {
                playerProfileData = CreatePlayerProfileData();
            }
            return playerProfileData;
        }

        public UserProfileData CreatePlayerProfileData()
        {
            UserProfileData playerProfileData = new UserProfileData
            {
                name = "TestUser"
            };
            return playerProfileData;
        }

        public void SavePlayerProfileData(UserProfileData playerProfileData)
        {
            string serializedObject = SerializeUtils<UserProfileData>.Serialize(playerProfileData);
            FileWriter.SaveTextFile(fileManager.playerProfileDataFilePath, serializedObject);
        }
    }
}
