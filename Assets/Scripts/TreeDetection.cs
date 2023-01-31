using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDetection : MonoBehaviour {
    public static Dictionary<int, InputTree> Trees;
    private Dictionary<int, List<dynamic>> previousTrees;

    private void Start() {
        Trees = new Dictionary<int, InputTree>();
        previousTrees = new Dictionary<int, List<dynamic>>();
    }

    public void Update() {
        Dictionary<int, InputTree> temp = new Dictionary<int, InputTree>();
        foreach (KeyValuePair<int, InputTree> tree in Trees) {
            if (tree.Value.boundingBox.Center == Vector3.zero && tree.Value.boundingBox.Width == 0 && tree.Value.boundingBox.Height == 0) {
                continue; // if the tree is not being rendered
            }

            if (previousTrees.ContainsKey(tree.Key)) {
                if (InputTree.isEqual(tree.Value, previousTrees[tree.Key])) {
                    continue; // if there's nothing to update
                }
            }

            // else prepare it to be sent
            temp.Add(tree.Key, tree.Value);
        }

        previousTrees.Clear();

        foreach(KeyValuePair<int, InputTree> tree in temp) {
            previousTrees.Add(tree.Key, new List<dynamic> {
                tree.Value.Age,
                tree.Value.Species,
                tree.Value.boundingBox.Center,
                tree.Value.boundingBox.Width,
                tree.Value.boundingBox.Height
            });
        }

        WebSocket.UpdateTrees(new List<InputTree>(temp.Values));
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
