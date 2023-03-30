using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDetection : MonoBehaviour {
    public static Dictionary<int, InputTree> Trees;
    private List<InputTree> createTrees;
    private Dictionary<int, List<object>> previousTrees;
    private static Dictionary<int, InputTree> treesToRemove;
    private int it = 0;

    private void Start() {
        Trees = new Dictionary<int, InputTree>();
        previousTrees = new Dictionary<int, List<object>>();
        treesToRemove = new Dictionary<int, InputTree>();
    }

    public async void FixedUpdate() {

        List<InputTree>[] newData = CreateData();

        await TCPSocket.UpdateTrees(
            new List<InputTree>(newData[0]),
            new List<InputTree>(newData[1]),
            new List<InputTree>(newData[2])
        );
    }

    private List<InputTree>[] CreateData() {
        Dictionary<int, InputTree> temp = new Dictionary<int, InputTree>();
        Dictionary<int, List<object>> newPrevTrees = new Dictionary<int, List<object>>();
        List<InputTree> removeTrees = new List<InputTree>();
        createTrees = new List<InputTree>();

        foreach (KeyValuePair<int, InputTree> tree in Trees) {
            if (tree.Value.boundingBox.Center == Vector3.zero || tree.Value.boundingBox.Width == 0 || tree.Value.boundingBox.Height == 0) {
                if (previousTrees.ContainsKey(tree.Key))
                    removeTrees.Add(tree.Value);

                continue; // if the tree is not being rendered
            }

            if (previousTrees.ContainsKey(tree.Key)) {
                if (InputTree.isEqual(tree.Value, previousTrees[tree.Key])) {
                    continue; // if there's nothing to update
                }
            }

            // if trees don't match what it was last time, prepare to send
            if (previousTrees.ContainsKey(tree.Key)) {
                if (tree.Value.boundingBox.Center != (Vector3)previousTrees[tree.Key][2]) {
                    temp.Add(tree.Key, tree.Value);
                }
            } else { createTrees.Add(tree.Value); } // if new tree

            newPrevTrees.Add(tree.Key, new List<object> {
                tree.Value.Age,
                tree.Value.Species,
                tree.Value.boundingBox.Center,
                tree.Value.boundingBox.Width,
                tree.Value.boundingBox.Height
            });
        }

        foreach (KeyValuePair<int, InputTree> tree in treesToRemove) {
            if (previousTrees.ContainsKey(tree.Key) && !removeTrees.Contains(tree.Value)) {
                removeTrees.Add(tree.Value);
            }
        }

        treesToRemove.Clear();
        previousTrees = newPrevTrees;

        return new List<InputTree>[3]{ createTrees, new List<InputTree>(temp.Values), removeTrees };
    }

    public static void AddTree(int key, InputTree tree) {
        if (Trees.ContainsKey(key)) {
            Trees[key] = tree;
        } else {
            Trees.Add(key, tree);
        }
    }

    public static void RemoveTree(int key, InputTree tree) {
        if (Trees.ContainsKey(key) && !treesToRemove.ContainsKey(key)) {
            treesToRemove.Add(key, tree);
        }
    }
}
