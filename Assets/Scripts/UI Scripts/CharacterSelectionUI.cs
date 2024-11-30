using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.Serialization;

public class CharacterSelectionUI : MonoBehaviour
{
    private const string LobbyNameString = "Lobby Name: ";
    private const string LobbyCodeString = "Lobby Code: ";

    [Title("Text Elements")] [SerializeField]
    private TextMeshProUGUI lobbyNameText;

    [SerializeField] private TextMeshProUGUI lobbyCodeText;

    [Title("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button readyButton;

    public void Start() {

        Lobby lobby = LobbyServices.Instance.GetLobby();

        lobbyNameText.text = LobbyNameString + lobby.Name;
        lobbyCodeText.text = LobbyCodeString + lobby.LobbyCode;
    }



}
