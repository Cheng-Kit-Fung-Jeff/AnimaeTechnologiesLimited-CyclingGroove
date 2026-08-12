using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static CKF_CommentController;

public class CKF_CommentController : MonoBehaviour
{
    public List<CKF_CommentListInterface> profiles;
    private List<int> profilesPool = new(), activeProfiles = new();

    public CKF_CommentList list;
    [System.Serializable]
    public struct Phase
    {
        public string key;
        public int profileCount;
        public int commentCount;
        public float range;
    }

    public List<Phase> phases;
    [ReadonlyField] public int currentPhase = -1;

    public List<Phase> special;
    private Dictionary<string, Phase> mapSpecial = new();

    private void Awake()
    {
        for (int i = 0; i < profiles.Count; ++i)
        {
            profilesPool.Add(i);
        }
        foreach (Phase phase in special)
            mapSpecial.Add(phase.key, phase);
    }

    public void IncrementPhase()
    {
        ++currentPhase;
        SetProfiles(phases[currentPhase].profileCount);
        Comment(phases[currentPhase].key, phases[currentPhase].commentCount, phases[currentPhase].range);
    }

    public void CallPhase(string key)
    {
        SetProfiles(mapSpecial[key].profileCount);
        Comment(key, mapSpecial[key].commentCount, mapSpecial[key].range);
    }

    private void Comment(string key, int count, float range)
    {
        range /= activeProfiles.Count * count;
        Fn.ShuffleFisherYates(activeProfiles);
        float begin = 0;
        foreach (var p in activeProfiles)
            StartCoroutine(Comment(p, key, begin, begin += range));
    }

    private void SetProfiles(int count)
    {
        while (activeProfiles.Count < count && profilesPool.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, profilesPool.Count);
            activeProfiles.Add(profilesPool[index]);
            profilesPool.RemoveAt(index);
        }
    }

    IEnumerator Comment(int profile, string phase, float begin, float end)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(begin, end));
        if (profiles[profile].Comment(phase, out string comment))
            list.AddComment(profiles[profile].key, comment);
    }
}
