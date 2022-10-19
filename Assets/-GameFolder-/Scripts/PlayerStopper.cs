using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopper : Singleton<PlayerStopper>
{
    [SerializeField] private GameObject wall;
    private GameObject cloneWall;
    public void SetWall()
    {
        if (!cloneWall)
        {
            var spawnPos = transform.position;
            cloneWall = Instantiate(wall, spawnPos, Quaternion.identity);
        }
        else
        {
            Destroy(cloneWall);
        }
    }
}
