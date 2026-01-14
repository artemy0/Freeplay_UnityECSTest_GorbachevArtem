using UnityEngine;

namespace ProjectScripts.Camera
{
    public class CameraTargetSingleton : MonoBehaviour
    {
        public static CameraTargetSingleton Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}