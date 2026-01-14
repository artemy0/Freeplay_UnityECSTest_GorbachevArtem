using System;
using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Debug
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;

        public Action OnPauseGame;
        public Action OnResumeGame;

        public bool IsPaused { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void PauseGame()
        {
            var initializationSystemGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
            initializationSystemGroup.Enabled = false;
            
            var simulationSystemGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationSystemGroup.Enabled = false;
            
            OnPauseGame?.Invoke();
            IsPaused = true;
        }

        public void ResumeGame()
        {
            var initializationSystemGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
            initializationSystemGroup.Enabled = true;
            
            var simulationSystemGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationSystemGroup.Enabled = true;
            
            OnResumeGame?.Invoke();
            IsPaused = false;
        }
    }
}