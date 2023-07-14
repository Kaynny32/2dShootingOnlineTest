using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    string _versionName = "0.1";
    [SerializeField]
    GameObject _userNamePanel;
    [SerializeField]
    GameObject _connectPanel;

    [SerializeField]
    TMP_InputField _userNameInput;
    [SerializeField]
    TMP_InputField _createGameInput;
    [SerializeField]
    TMP_InputField _joinGameInput;

    [SerializeField]
    GameObject _startButton;


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(_versionName);
    }

    private void Start()
    {
        _userNamePanel.SetActive(true);
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }
    public void ChangeUserNameInput()
    {
        if (_userNameInput.text.Length >= 3)
        {
            _startButton.SetActive(true);
        }
        else
        {
            _startButton.SetActive(false);
        }
    }

    public void SetUserName()
    {
        _userNamePanel.SetActive(false);
        PhotonNetwork.playerName = _userNameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(_createGameInput.text, new RoomOptions() { maxPlayers = 5 }, null);
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom(_joinGameInput.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
