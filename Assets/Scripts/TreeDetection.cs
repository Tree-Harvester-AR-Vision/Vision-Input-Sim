using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Clients;
using UnityEngine;
using UnityEngine.Serialization;

public class TreeDetection : MonoBehaviour
{
    public static Dictionary<int, InputTree> trees;
    private List<InputTree> _createTrees;
    private Dictionary<int, List<object>> _previousTrees;
    private static Dictionary<int, InputTree> _treesToRemove;
    private int _it = 0;

    public Client Client;

    private void Start()
    {
        trees = new Dictionary<int, InputTree>();
        _previousTrees = new Dictionary<int, List<object>>();
        _treesToRemove = new Dictionary<int, InputTree>();
    }

    public async void FixedUpdate()
    {

        List<InputTree>[] newData = CreateData();
        
            await Client.UpdateTrees(
                new List<InputTree>(newData[0]),
                new List<InputTree>(newData[1]),
                new List<InputTree>(newData[2])
            );


    }

    private List<InputTree>[] CreateData()
    {
        Dictionary<int, InputTree> temp = new Dictionary<int, InputTree>();
        Dictionary<int, List<object>> newPrevTrees = new Dictionary<int, List<object>>();
        List<InputTree> removeTrees = new List<InputTree>();
        _createTrees = new List<InputTree>();

        foreach (KeyValuePair<int, InputTree> tree in trees)
        {
            if (tree.Value.boundingBox.Center == Vector3.zero || tree.Value.boundingBox.Width == 0 || tree.Value.boundingBox.Height == 0)
            {
                if (_previousTrees.ContainsKey(tree.Key))
                    removeTrees.Add(tree.Value);

                continue; // if the tree is not being rendered
            }

            if (_previousTrees.ContainsKey(tree.Key))
            {
                if (InputTree.isEqual(tree.Value, _previousTrees[tree.Key]))
                {
                    continue; // if there's nothing to update
                }
            }

            // if trees don't match what it was last time, prepare to send
            if (_previousTrees.ContainsKey(tree.Key))
            {
                if (tree.Value.boundingBox.Center != (Vector3)_previousTrees[tree.Key][2])
                {
                    temp.Add(tree.Key, tree.Value);
                }
            }
            else { _createTrees.Add(tree.Value); } // if new tree

            newPrevTrees.Add(tree.Key, new List<object> {
                tree.Value.Age,
                tree.Value.Species,
                tree.Value.boundingBox.Center,
                tree.Value.boundingBox.Width,
                tree.Value.boundingBox.Height
            });
        }

        foreach (KeyValuePair<int, InputTree> tree in _treesToRemove)
        {
            if (_previousTrees.ContainsKey(tree.Key) && !removeTrees.Contains(tree.Value))
            {
                removeTrees.Add(tree.Value);
            }
        }

        _treesToRemove.Clear();
        _previousTrees = newPrevTrees;

        return new List<InputTree>[3] { _createTrees, new List<InputTree>(temp.Values), removeTrees };
    }

    public static void AddTree(int key, InputTree tree)
    {
        if (trees.ContainsKey(key))
        {
            trees[key] = tree;
        }
        else
        {
            trees.Add(key, tree);
        }
    }

    public static void RemoveTree(int key, InputTree tree)
    {
        if (trees.ContainsKey(key) && !_treesToRemove.ContainsKey(key))
        {
            _treesToRemove.Add(key, tree);
        }
    }
}
