using UnityEngine;
using UnityEngine.UI;

namespace ProjectScripts.Debug.UI
{
    [RequireComponent(typeof(Button))]
    public class DebugWidget : MonoBehaviour
    {
        [SerializeField]
        private Button targetButton;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            targetButton = GetComponent<Button>();
        }
        #endif

        private void Awake()
        {
            targetButton.onClick.AddListener(ShowDebug);
        }

        private void OnDestroy()
        {
            targetButton.onClick.RemoveListener(ShowDebug);
        }

        private void ShowDebug()
        {
            DebugPopup.Instance.Show();
        }
    }
}