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
            var spawnPos = transform.position; spawnPos += transform.forward + Vector3.one * 2;
            cloneWall = Instantiate(wall, spawnPos, Quaternion.identity);
            cloneWall.transform.eulerAngles = transform.eulerAngles;
        }
        else
        {
            Destroy(cloneWall);
        }
    }
}
