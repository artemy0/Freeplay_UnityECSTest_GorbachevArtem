using UnityEngine;

namespace ProjectScripts.Debug
{
    public class FPSManager : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR && ANDROID
            Application.targetFrameRate = 60;
#endif
        }
    }
}