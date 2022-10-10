using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnabler : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    void Start()
    {
        characters[Random.Range(0, characters.Count)].SetActive(true);
    }
}
