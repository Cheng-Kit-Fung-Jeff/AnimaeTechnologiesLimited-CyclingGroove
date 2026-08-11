using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_CommentListInterface : MonoBehaviour
{
    public string key;
    public CKF_CommentList list;

    [System.Serializable]
    public class State
    {
        public string state;
        public List<string> comment;
        [System.NonSerialized] public List<int> pool;
    }

    public List<State> states = new();

    private readonly Dictionary<string, State> mapState = new();

    public void Awake()
    {
        foreach (var state in states)
        {
            state.pool = new();
            for (int i = 0; i < state.comment.Count; ++i)
            {
                state.pool.Add(i);
            }
            mapState.Add(state.state, state);
        }
    }

    public void Comment(string key)
    {
        if (!mapState.ContainsKey(key) || mapState[key].pool.Count == 0) return;

        int poolIndex = UnityEngine.Random.Range(0, mapState[key].pool.Count);
        int commentIndex = mapState[key].pool[poolIndex];
        Debug.Log(key+":"+ commentIndex);
        mapState[key].pool.RemoveAt(poolIndex);
        list.AddComment(this.key, mapState[key].comment[commentIndex]);
    }
}
