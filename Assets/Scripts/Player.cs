using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{
    [SerializeField]
    PhotonView _photonView;
    [SerializeField]
    Rigidbody2D _rb;
    [SerializeField]
    GameObject _playerCamera;
    [SerializeField]
    SpriteRenderer _spriteRenderer;
    [SerializeField]
    TextMeshProUGUI _playerName;
    [SerializeField]
    Animator _animator;

    [SerializeField]
    bool _isGrounded = false;
    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    GameObject _prefabBullet;
    [SerializeField]
    Transform _firePos;

    public bool _disableInput = false;

    Joystick _joystick;
    float x;
    float speed = 5;
    bool _isF = true;

    Button _buttonFire;
    bool _isShoot = false;

    private void Awake()
    {
        if (photonView.isMine)
        {
            _playerCamera.SetActive(true);
            _playerName.text = PhotonNetwork.playerName;
        }
        else
        {
            _playerName.text = _photonView.owner.name;
            _playerName.color = Color.red;
        }
        
    }
    private void Start()
    {
        _joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        _buttonFire = GameObject.FindGameObjectWithTag("Fire").GetComponent<Button>();
        _buttonFire.onClick.AddListener(() =>
        {
            _isShoot = true;
        });
    }
    private void Update()
    {
        x = _joystick.Horizontal * speed; 
        if (photonView.isMine && !_disableInput)
        {
            CheckInput();            
        }
        
    }
    private void CheckInput()
    {
        _rb.velocity = new Vector2(x, 0);
        if (x > 0)
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
        if (x < 0)
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }
        if (_isShoot == true)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (_spriteRenderer.flipX == false)
        {
            GameObject obj = PhotonNetwork.Instantiate(_prefabBullet.name, new Vector2(_firePos.transform.position.x, _firePos.transform.position.y), Quaternion.identity, 0);
        }
        if (_spriteRenderer.flipX == true)
        {
            GameObject obj = PhotonNetwork.Instantiate(_prefabBullet.name, new Vector2(_firePos.transform.position.x, _firePos.transform.position.y), Quaternion.identity, 0);
            obj.GetComponent<PhotonView>().RPC("ChangeDir_left", PhotonTargets.AllBuffered);
        }
        _isShoot = false;
    }

    [PunRPC]
    private void FlipTrue()
    {
        _spriteRenderer.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        _spriteRenderer.flipX = false;
    }
    [PunRPC]
    private void Flip()
    {
        _isF = !_isF;
        transform.Rotate(0f, 180f, 0f);
    }
}
