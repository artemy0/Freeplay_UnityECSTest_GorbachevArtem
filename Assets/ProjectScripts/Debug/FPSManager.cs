using UnityEngine;

namespace ProjectScripts.Debug
{
    public class FPSManager : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
        }
    }
}