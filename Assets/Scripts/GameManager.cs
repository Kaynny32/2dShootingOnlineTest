using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField]
    GameObject _playerPrefab;
    [SerializeField]
    GameObject _gameCanvas;
    [SerializeField]
    GameObject _sceneCamera;
    [SerializeField]
    TextMeshProUGUI _pingText;
    [SerializeField]
    GameObject _disconnectUI;
    [SerializeField]
    GameObject _playerFeed;
    [SerializeField]
    GameObject _feedGrid;

    bool _off = false;
    [HideInInspector]
    public GameObject _localPlayer;
    public TextMeshProUGUI _textRespawnTimerText;
    public GameObject _respawnPanel;
    float _timerAmount = 5f;
    bool _runSpawnTimer = false;

    private void Awake()
    {
        instance = this;
        _gameCanvas.SetActive(true);
    }

    private void Update()
    {
        _pingText.text = "Ping:" + PhotonNetwork.GetPing();
        if (_runSpawnTimer)
        {
            StartRespawn();
        }
    }

    public void EnableRespawn()
    {
        _timerAmount = 5f;
        _runSpawnTimer = true;
        _respawnPanel.SetActive(true);
    }

    private void StartRespawn()
    {
        _timerAmount -= Time.deltaTime;
        _textRespawnTimerText.text = "Respawning in " + (int)_timerAmount;
        if (_timerAmount <=0)
        {
            _localPlayer.GetComponent<PhotonView>().RPC("Respawn", PhotonTargets.AllBuffered);
            _localPlayer.GetComponent<Health>().EnableInput();
            RespawnLocation();
            _respawnPanel.SetActive(false);
            _runSpawnTimer = false;
        }
    }

    public void RespawnLocation()
    {
        float random = Random.Range(-3f, 5f);
        _localPlayer.transform.localPosition = new Vector2(random, 3f);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-1f,1f);

        PhotonNetwork.Instantiate(_playerPrefab.name,new Vector3(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
        _gameCanvas.SetActive(false);
        _sceneCamera.SetActive(false);
    }
    public void CheckInput()
    {
        if (_off)
        {
            _disconnectUI.SetActive(false);
            _off = false;
        }
        else
        {
            _disconnectUI.SetActive(true);
            _off = true;
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(_playerFeed, new Vector2(0,0), Quaternion.identity);
        obj.transform.SetParent(_feedGrid.transform, false);
        obj.GetComponent<TextMeshProUGUI>().text = player.name + " joined the game";
        obj.GetComponent<TextMeshProUGUI>().color = Color.green;
    }
    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(_playerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(_feedGrid.transform, false);
        obj.GetComponent<TextMeshProUGUI>().text = player.name + " left the game";
        obj.GetComponent<TextMeshProUGUI>().color = Color.red;
    }
}
