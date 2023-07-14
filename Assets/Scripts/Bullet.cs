using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Photon.MonoBehaviour
{
    bool _moveDir = false;    

    [SerializeField]
    float _destroyTime;
    [SerializeField]
    float _moveSpeed;
    [SerializeField]
    float _bulletDamage;

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    { 
        yield return new WaitForSeconds(_destroyTime);
        this.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void ChangeDir_left()
    {
        _moveDir = true;
    }
    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    private void Update()
    {
        if (!_moveDir)
        {
            transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * _moveSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.isMine)
            return;
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();
        if (target !=null && (!target.isMine || target.isSceneView))
        {
            if (target.tag == "Player")
            {
                target.RPC("ReduceHealth", PhotonTargets.AllBuffered, _bulletDamage);
            }
            this.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.AllBuffered);
        }
    }
}
