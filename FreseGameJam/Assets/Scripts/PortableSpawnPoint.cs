using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableSpawnPoint : MonoBehaviour
{
    GameObject _player;
    [SerializeField] bool useThisSpawnPosition = false;
    void Start()
    {
        Debug.Log(this.transform.position);
        _player = GameObject.Find("Third Person Player");
        _player.GetComponent<HealthSystem>().RespawnPoint = this.transform.position;
        Debug.Log(_player.GetComponent<HealthSystem>().RespawnPoint);
        if(useThisSpawnPosition == true)
        {
            _player.transform.position = this.transform.position;
        }
    }
}
