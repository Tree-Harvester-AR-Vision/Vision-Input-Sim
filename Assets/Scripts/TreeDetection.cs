using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDetection : MonoBehaviour {
    public static Dictionary<int, InputTree> Trees;

    private void Start() {
        Trees = new Dictionary<int, InputTree>();
    }

    public void Update() {
        WebSocket.UpdateTrees(new List<InputTree>(Trees.Values));
    }

    public static void AddTree(int key, InputTree tree) {
        if (Trees.ContainsKey(key)) {
            Trees[key] = tree;
        } else {
            Trees.Add(key, tree);
        }
    }

    public static void RemoveTree(int key, InputTree tree) {
        if (Trees.ContainsKey(key)) {
            Trees.Remove(key);
        }
    }
}
