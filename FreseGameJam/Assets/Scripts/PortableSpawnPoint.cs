using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableSpawnPoint : MonoBehaviour
{
    GameObject _player;
    [SerializeField] bool useThisSpawnPosition = false;
    void Start()
    {
        if (useThisSpawnPosition)
        {
            //Debug.Log("Spawn location: " + this.transform.position);
            _player = GameObject.Find("Third Person Player");
            _player.GetComponent<HealthSystem>().RespawnPoint = this.transform.position;
            //Debug.Log("New Respawn location: " + _player.GetComponent<HealthSystem>().RespawnPoint);
            Invoke("Spawn", 1f);
            
            //_player.transform.position = this.transform.position;
        }
        /*Debug.Log("Spawn location: " + this.transform.position);
        _player = GameObject.Find("Third Person Player");
        _player.GetComponent<HealthSystem>().RespawnPoint = this.transform.position;
        Debug.Log("New Respawn location: " + _player.GetComponent<HealthSystem>().RespawnPoint);
        if(useThisSpawnPosition == true)
        {
            _player.transform.position = this.transform.position;
        }*/
    }
    void Spawn()
    {
        _player.transform.position = this.transform.position;

    }
}
