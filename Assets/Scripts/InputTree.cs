using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTree : MonoBehaviour
{

    public BoundingBox boundingBox;
    public int Age;
    public string Species;
    private int Key;

    private string[] possibleSpecies = {
        "Red Maple",
        "English Oak",
        "Black Cherry",
        "American Basswood",
        "American Elm",
        "Honey Locust",
        "Sakura"
    };

    public override string ToString() {
        return $"{{Species: {Species}; Age: {Age}; BoundingBox: {boundingBox.ToString()}}}";
    }

    void Start() {
        Age = Random.Range(1, 4_853);
        Species = possibleSpecies[Random.Range(0, 6)];

        boundingBox = transform.GetChild(0).GetChild(0).GetComponent<BoundingBox>();

        Key = GetHashCode();
    }

    void OnBecameVisible() {
        TreeDetection.AddTree(Key, this);
    }

    void OnBecameInvisible() {
        TreeDetection.RemoveTree(Key, this);
    }
}
