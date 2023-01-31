using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class InputTree : MonoBehaviour {
    [JsonProperty] public BoundingBox boundingBox;
    [JsonProperty] public int Age;
    [JsonProperty] public string Species;
    [JsonProperty] private int Key;

    private string[] possibleSpecies = {
        "Red Maple",
        "English Oak",
        "Black Cherry",
        "American Basswood",
        "American Elm",
        "Honey Locust",
        "Sakura"
    };

    public string JsonSerialize() {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
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
