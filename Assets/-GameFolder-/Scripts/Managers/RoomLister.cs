using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLister : Singleton<RoomLister>
{
    public List<Room> rooms = new List<Room>();

    private void Awake()
    {
        Room[] roomArray = FindObjectsOfType<Room>();

        foreach (Room r in roomArray)
        {
            rooms.Add(r);
        }
    }
}
