using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.FileManagement;
using System;
using Assets.Scripts.Utils;
using System.IO;
using Assets.Scripts.Events;
using Assets.Scripts.Data;

namespace Assets.Scripts.GameManagement
{
    public class GameData : MonoBehaviour
    {
        public event Action DataLoaded;
        public FileManager fileManager;
        
        protected GameDataFileReader gameDataFileReader;
        [HideInInspector] public bool isDataLoaded;
        [HideInInspector] public UserProfileData playerProfileData;
        [HideInInspector] public GameSettings settings;

        protected void Awake()
        {
            gameDataFileReader = new GameDataFileReader(fileManager, this);
        }

        /// <summary>
        /// Calling onDataLoadedCallback when data is loaded
        /// </summary>
        /// <param name="onDataLoadedCallback"></param>
        public void DataWaiting(Action onDataLoadedCallback)
        {
            if (isDataLoaded)
            {
                onDataLoadedCallback();
            }
            else
            {
                DataLoaded += onDataLoadedCallback;
            }
        }

        /// <summary>
        /// Unsubscribe onDataLoadedCallback to DataLoaded event
        /// </summary>
        /// <param name="onDataLoadedCallback"></param>
        public void UnsubscribeDataWaiting(Action onDataLoadedCallback)
        {
            DataLoaded -= onDataLoadedCallback;
        }

        protected void Start()
        {
            InitParameters();
        }

        protected void InitParameters()
        {
            LoadSettings();
            playerProfileData = gameDataFileReader.LoadPlayerProfileData();
            isDataLoaded = true;
            if (DataLoaded != null)
            {
                DataLoaded();
            }
        }

        public void LoadSettings()
        {
            if (File.Exists(fileManager.settingsFilePath))
            {
                string settingsString = FileDownloader.LoadTextFile(fileManager.settingsFilePath);
                settings = SerializeUtils<GameSettings>.Deserialize(settingsString);
            }
            else
            {
                SaveSettings();
            }
        }

        public void SaveSettings()
        {
            string serializedObject = SerializeUtils<GameSettings>.Serialize(settings);
            FileWriter.SaveTextFile(fileManager.settingsFilePath, serializedObject);
        }

        protected void SaveUserProfileData()
        {
            gameDataFileReader.SavePlayerProfileData(playerProfileData);
            DbLog.Log("UserProfileData saved", Color.cyan, this);
        }
    }
}
