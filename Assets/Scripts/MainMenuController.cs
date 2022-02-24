using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace CybernautX
{
    public class MainMenuController : MonoBehaviour
    {
        [System.Serializable]
        public class Menu
        {
            public string menuName = "Insert Menu Name";
            public GameObject rootObject;
        }

        [BoxGroup("Settings")]
        [SerializeField]
        private string defaultMenu = "";

        [BoxGroup("References")]
        [SerializeField]
        private List<Menu> menus = new List<Menu>();

        [BoxGroup("Selectables")]
        [SerializeField]
        private Toggle fullscreenToggle = null;

        [BoxGroup("Selectables")]
        [SerializeField]
        private Toggle windowedToggle = null;


        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private List<Menu> activeMenus = new List<Menu>();

        public static UnityAction<MainMenuController> OnAwakeEvent;

        private void Awake()
        {
            if (fullscreenToggle != null)
                fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);

            if (windowedToggle != null)
                windowedToggle.onValueChanged.AddListener(OnWindowedToggleChanged);

            OnAwakeEvent?.Invoke(this);
        }

        private void Start()
        {
            CloseAllMenus();
            OpenMenuSingle(defaultMenu);
        }

        public void OpenMenuSingle(string menuName = "") => OpenMenu(menuName);

        public void OpenMenuAdditive(string menuName = "") => OpenMenu(menuName, true);

        [Button]
        public void OpenMenu(string menuName = "", bool additive = false)
        {
            //if (mainUI != null)
            //    mainUI.SetActive(true);

            if (menus.Count > 0 && menus[0].rootObject != null)
            {
                Menu menuToOpen = GetMenuByName(menuName);

                if (menuToOpen != null)
                {
                    if (!additive)
                        CloseAllMenus();

                    menuToOpen.rootObject.SetActive(true);

                    RegisterMenu(menuToOpen);

                    if (!Cursor.visible)
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                    
                }
            }
        }

        public void CloseMenu(string menuName = "")
        {
            Menu menuToClose = GetMenuByName(menuName);

            if (menuToClose != null)
            {
                menuToClose.rootObject.SetActive(false);
                UnregisterMenu(menuToClose);
            }
        }

        public void CloseAllMenus()
        {
            foreach (Menu menu in menus)
            {
                menu.rootObject.SetActive(false);
                UnregisterMenu(menu);
            }
        }

        private Menu GetMenuByName(string menuName)
        {
            foreach (Menu menu in menus)
            {
                if (menu.menuName == menuName)
                    return menu;
            }

            Debug.LogWarning("[UIManager]: Couldn't find a menu with the name " + menuName);
            return null;
        }

        private void RegisterMenu(Menu menu)
        {
            if (!activeMenus.Contains(menu))
                activeMenus.Add(menu);
        }

        private void UnregisterMenu(Menu menu)
        {
            if (activeMenus.Contains(menu))
                activeMenus.Remove(menu);
        }

        private void OnFullscreenToggleChanged(bool value)
        {
            if (value && Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Debug.Log($"{this.name}: Set ScreenMode to fullscreen.");
            }
        }

        private void OnWindowedToggleChanged(bool value)
        {
            if (value && Screen.fullScreenMode != FullScreenMode.Windowed)
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log($"{this.name}: Set ScreenMode to windowed.");
            }
        }
    }
}

