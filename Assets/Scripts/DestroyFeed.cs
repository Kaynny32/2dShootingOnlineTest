using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeed : MonoBehaviour
{
    float _destroyTime = 4f;

    private void OnEnable()
    {
        Destroy(gameObject, _destroyTime);
    }
}
