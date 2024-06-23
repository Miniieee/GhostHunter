using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;

public class CharacterSelectionUI : MonoBehaviour
{
    private const string lobbyNameString = "Lobby Name: ";
    private const string lobbyCodeString = "Lobby Code: ";

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI LobbyNameText;
    [SerializeField] private TextMeshProUGUI LobbyCodeText;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button readyButton;

    public void Start() {

        Lobby lobby = LobbyServices.Instance.GetLobby();

        LobbyNameText.text = lobbyNameString + lobby.Name;
        LobbyCodeText.text = lobbyCodeString + lobby.LobbyCode;
    }



}
