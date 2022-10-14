using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDelivery : MonoBehaviour
{
    [SerializeField] private Transform carryTransform;
    public Transform CarryTransform { get { return carryTransform; } }

    private Food food;
    public void SetFood(Food food) { this.food = food; }
    public Food GetFood() { return food; }
}
