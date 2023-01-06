using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDelivery : MonoBehaviour
{
    [SerializeField] private Transform carryTransform;
    public Transform CarryTransform { get { return carryTransform; } }

    private List<Food> foods = new List<Food>();
    public int FoodCount { get => foods.Count; }

    public void SetFood(Food food) { foods.Add(food); }
    public Food GetFood() { return foods[FoodCount - 1]; }
    public void RemoveFood() { foods.Remove(GetFood()); }
    public int GetFoodOrder(Food food) { return foods.IndexOf(food); }
}
