using System;
using UI_Scripts;
using UnityEngine;

namespace GameManagers
{
    public class UIManager : MonoBehaviour
    {
        private PlayerControls playerControls;
        private LobbyUI lobbyUI;

        private void Awake()
        {
            playerControls = new PlayerControls();
        }

        private void Start()
        {
            playerControls.UI.Esc.performed += _ => ToggleVisibility();
            lobbyUI = FindFirstObjectByType<LobbyUI>();
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void ToggleVisibility()
        {
            lobbyUI.gameObject.SetActive(!lobbyUI.gameObject.activeSelf);
        }
    }
}