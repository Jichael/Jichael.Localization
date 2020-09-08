using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Jichaels.Localization
{
    public class LanguageManager : MonoBehaviour
    {
        public static LanguageManager Instance { get; private set; }
        public string CurrentLanguage { get; private set; }
        
        public bool IsReady { get; private set; }
        
        [SerializeField] private string defaultLanguage;

        private Dictionary<string, string> _languageData;

        private readonly HashSet<LanguageEntry> _languageEntries = new HashSet<LanguageEntry>();

        private const string LANGUAGE_FOLDER_NAME = "Languages";
        private const string LANGUAGE_FILE_EXTENSION = "json";
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            GetLanguageData(defaultLanguage);
        }

        private void GetLanguageData(string newLanguage)
        {
            IsReady = false;
            CurrentLanguage = newLanguage;

            string filePath = CombinePath(LANGUAGE_FOLDER_NAME, CurrentLanguage, LANGUAGE_FILE_EXTENSION);

            if (filePath.Contains("://") || filePath.Contains(":///"))
            {
                StartCoroutine(GetLanguageDataCo(filePath));
                return;
            }

            if (!File.Exists(filePath))
            {
                Debug.LogError($"GetLanguageData - Could not load lang file : '{filePath}' doesn't exist !", this);
                return;
            }
            
            string json = File.ReadAllText(filePath);

            _languageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            IsReady = true;
        }

        private IEnumerator GetLanguageDataCo(string uri)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
            {
                Debug.LogError($"Error while getting {uri}. Code : {unityWebRequest.error}");
                yield break;
            }
            _languageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(unityWebRequest.downloadHandler.text);
            IsReady = true;
        }

        public string RequestValue(string key)
        {
            if (_languageData.TryGetValue(key, out string value))
            {
                return value;
            }

            Debug.LogError($"RequestValue - Entry '{key}' not found in language file !", this);
            return string.Empty;
        }

        public void ChangeLanguage(string newLanguage)
        {
            if (string.Equals(CurrentLanguage, newLanguage)) return;
            
            GetLanguageData(newLanguage);

            foreach (var entry in _languageEntries)
            {
                entry.SetValue();
            }
        }

        public void AddEntry(LanguageEntry entry)
        {
            _languageEntries.Add(entry);
        }

        public void RemoveEntry(LanguageEntry entry)
        {
            _languageEntries.Remove(entry);
        }

        private static string CombinePath(string path, string fileName, string extension)
        {
            return Path.Combine(StreamingAssetsPath(path), $"{fileName}.{extension}");
        }

        private static string StreamingAssetsPath(string path)
        {
            return Path.Combine(Application.streamingAssetsPath, $"{path}");
        }
        
    }
}