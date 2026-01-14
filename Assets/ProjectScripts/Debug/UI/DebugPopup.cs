using ProjectScripts.Character.Enemy.Spawn;
using ProjectScripts.Weapon.Bullet;
using ProjectScripts.Weapon.General;
using ProjectScripts.Weapon.Granade;
using ProjectScripts.Weapon.Grenade;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectScripts.Debug.UI
{
    public class DebugPopup : MonoBehaviour
    {
        public static DebugPopup Instance;

        [SerializeField] private Button closeButton;

        private EntityManager entityManager;

        private Entity enemySpawnEntity;
        private Entity bulletsWeaponEntity;
        private Entity grenadeWeaponEntity;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            gameObject.SetActive(false);
            Instance = this;
        }
        
        void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            enemySpawnEntity = entityManager.CreateEntityQuery(typeof(EnemySpawnData)).GetSingletonEntity();
            grenadeWeaponEntity = entityManager.CreateEntityQuery(typeof(GrenadeWeaponTag)).GetSingletonEntity();
            bulletsWeaponEntity = entityManager.CreateEntityQuery(typeof(BulletWeaponTag)).GetSingletonEntity();
            
            closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Close);
        }

        private void OnGUI()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            SetupGUIElementSize();

            GUILayout.BeginArea(GetGUIWindowSize(), GUI.skin.box);
            ShowEnemySpawnParameters();
            ShowBulletWeaponParameters();
            ShowGrenadeWeaponParameters();
            GUILayout.EndArea();
        }

        public void Show()
        {
            PauseManager.Instance.PauseGame();
            gameObject.SetActive(true);
        }

        private void Close()
        {
            PauseManager.Instance.ResumeGame();
            gameObject.SetActive(false);
        }

        private Rect GetGUIWindowSize()
        {
            var width = Screen.width / 2f;
            var height = Screen.height - 100;

            var rect = new Rect(
                (Screen.width - width) * 0.5f,
                (Screen.height - height) * 0.5f,
                width,
                height
            );
            return rect;
        }

        private void SetupGUIElementSize()
        {
            var baseSize = Mathf.Min(Screen.height, Screen.width) / 30f;
            
            GUI.skin.label.fontSize = Mathf.RoundToInt(baseSize);
            GUI.skin.horizontalSlider.fixedHeight = baseSize * 0.75f;
            GUI.skin.horizontalSliderThumb.fixedHeight = baseSize;
            GUI.skin.horizontalSliderThumb.fixedHeight = baseSize;
        }
        
        private void ShowEnemySpawnParameters()
        {
            var enemySpawnData = entityManager.GetComponentData<EnemySpawnData>(enemySpawnEntity);

            GUILayout.Label($"Enemy Spawn Rate: {enemySpawnData.SpawnCooldown}");
            enemySpawnData.SpawnCooldown = GUILayout.HorizontalSlider(enemySpawnData.SpawnCooldown, 0.05f, 1f);

            GUILayout.Label($"Enemy Limit Count: {enemySpawnData.EnemyLimit}");
            enemySpawnData.EnemyLimit = (int)GUILayout.HorizontalSlider(enemySpawnData.EnemyLimit, 1, 1000);
            
            entityManager.SetComponentData(enemySpawnEntity, enemySpawnData);
        }

        private void ShowBulletWeaponParameters()
        {
            var bulletWeaponData = entityManager.GetComponentData<WeaponAttackData>(bulletsWeaponEntity);
            
            GUILayout.Label($"Bullet Attack Cooldown: {bulletWeaponData.AttackCooldown}");
            bulletWeaponData.AttackCooldown = GUILayout.HorizontalSlider(bulletWeaponData.AttackCooldown, 0.05f, 2f);

            GUILayout.Label($"Bullets Attack Count: {bulletWeaponData.AttackCount}");
            bulletWeaponData.AttackCount = (int)GUILayout.HorizontalSlider(bulletWeaponData.AttackCount, 1, 50);
            
            GUILayout.Label($"Bullet Additional Attack Cooldown: {bulletWeaponData.AdditionalAttacksCooldown}");
            bulletWeaponData.AdditionalAttacksCooldown = GUILayout.HorizontalSlider(bulletWeaponData.AdditionalAttacksCooldown, 0.05f, 0.5f);
            
            entityManager.SetComponentData(bulletsWeaponEntity, bulletWeaponData);
        }

        private void ShowGrenadeWeaponParameters()
        {
            var grenadeWeaponData = entityManager.GetComponentData<WeaponAttackData>(grenadeWeaponEntity);
            
            GUILayout.Label($"Grenade Attack Cooldown: {grenadeWeaponData.AttackCooldown}");
            grenadeWeaponData.AttackCooldown = GUILayout.HorizontalSlider(grenadeWeaponData.AttackCooldown, 0.1f, 4f);

            GUILayout.Label($"Grenades Attack Count: {grenadeWeaponData.AttackCount}");
            grenadeWeaponData.AttackCount = (int)GUILayout.HorizontalSlider(grenadeWeaponData.AttackCount, 1, 25);
            
            GUILayout.Label($"Grenade Additional Attack Cooldown: {grenadeWeaponData.AdditionalAttacksCooldown}");
            grenadeWeaponData.AdditionalAttacksCooldown = GUILayout.HorizontalSlider(grenadeWeaponData.AdditionalAttacksCooldown, 0.1f, 1f);

            entityManager.SetComponentData(grenadeWeaponEntity, grenadeWeaponData);
        }
    }
}