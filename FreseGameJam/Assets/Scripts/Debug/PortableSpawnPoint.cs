using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableSpawnPoint : MonoBehaviour
{
    [SerializeField] bool useThisSpawnPosition = false;

    GameObject _player;

    void Start()
    {
        if (useThisSpawnPosition)
        {
            _player = GameObject.Find("Third Person Player_GameLevel_1");
            _player.GetComponent<HealthSystem>().respawnPoint = this.transform.position;
            Invoke("Spawn", 1f);
        }
    }

    void Spawn()
    {
        _player.transform.position = this.transform.position;
    }
}
