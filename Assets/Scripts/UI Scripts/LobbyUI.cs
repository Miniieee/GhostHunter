using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button createButton;

        [SerializeField] private Button joinWithCodeButton;

        [SerializeField] private TMP_InputField lobbyCodeInputField;

        [SerializeField] private LobbyCreateUI lobbyCreateUI;


        private void Awake()
        {
            mainMenuButton.onClick.AddListener(() => Debug.Log("Main Menu"));
            joinButton.onClick.AddListener(() => LobbyServices.Instance.QuickJoin());
            createButton.onClick.AddListener(() => StartLobbyCreateUI());

            joinWithCodeButton.onClick.AddListener(() => LobbyServices.Instance.JoinLobbyByCode(lobbyCodeInputField.text));
        }

        private void Start()
        {
            Show();
        }

        private void StartLobbyCreateUI()
        {
            lobbyCreateUI.Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
