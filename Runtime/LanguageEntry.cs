using TMPro;
using UnityEngine;

namespace Jichaels.Localization
{

    [RequireComponent(typeof(TMP_Text))]
    public class LanguageEntry : MonoBehaviour
    {

        [SerializeField] private string languageEntry;

        [SerializeField] private TMP_Text text;

        private bool _added;

        private void Start()
        {
            AddEntry();
            SetValue();
        }

        private void OnDestroy()
        {
            RemoveEntry();
        }

        private void AddEntry()
        {
            LanguageManager.Instance.AddEntry(this);
            _added = true;
        }

        private void RemoveEntry()
        {
            if(_added) LanguageManager.Instance.RemoveEntry(this);
        }

        public void SetValue()
        {
            text.text = LanguageManager.Instance.RequestValue(languageEntry);
        }

        private void OnValidate()
        {
            if (text == null)
            {
                text = GetComponent<TMP_Text>();
            }
        }
    }
    
}