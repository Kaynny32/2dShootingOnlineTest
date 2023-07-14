using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    [SerializeField]
    Image _fillImage;
    [SerializeField]
    float _healthAmount;

    [SerializeField]
    Rigidbody2D _rb;
    [SerializeField]
    BoxCollider2D _boxCollider;
    [SerializeField]
    SpriteRenderer _spriteRenderer;

    [SerializeField]
    GameObject _palyerCanvas;
    [SerializeField]
    Player _player;

    

    private void Awake()
    {
        if (photonView.isMine)
        {
            GameManager.instance._localPlayer = this.gameObject; 
        }
    }

    [PunRPC]
    public void ReduceHealth(float amount)
    {
        ModigyHealth(amount);
    }

    private void CheckHealth()
    {
        _fillImage.fillAmount = _healthAmount / 100f;
        if (photonView.isMine && _healthAmount <= 0 )
        {
            GameManager.instance.EnableRespawn();
            _player._disableInput = true;
            this.GetComponent<PhotonView>().RPC("Dead", PhotonTargets.AllBuffered);
        }
    }
    public void EnableInput()
    {
        _player._disableInput = false;
    }

    [PunRPC]
    private void Dead()
    { 
        _rb.gravityScale = 0f;
        _boxCollider.enabled = false;
        _spriteRenderer.enabled = false;
        _palyerCanvas.SetActive(false);
    }

    [PunRPC]
    private void Respawn()
    {
        _rb.gravityScale = 20.0f;
        _boxCollider.enabled = true;
        _spriteRenderer.enabled = true;
        _palyerCanvas.SetActive(true);
        _fillImage.fillAmount = 1f;
        _healthAmount = 100f;
    }

    private void ModigyHealth(float amount)
    {
        if (photonView.isMine)
        {
            _healthAmount -= amount;
            _fillImage.fillAmount -= amount;
        }
        else 
        {
            _healthAmount -= amount;
            _fillImage.fillAmount = amount;
        }
        CheckHealth();
    }
}
