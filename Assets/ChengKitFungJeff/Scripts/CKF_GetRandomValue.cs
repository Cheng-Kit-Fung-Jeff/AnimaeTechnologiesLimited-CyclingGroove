using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_GetRandomValue : MonoBehaviour
{
    public string key, setChar;
    [Min(1)] public int minLength = 1, maxLength = 1, maxDuplicates = 1;

    private class CharNode
    {
        private static char NULLCHAR = '\0';
        private readonly char value; // root value is '\0'
        private string sum = "";
        public string Sum { get => sum; }
        private CharNode parent = null;
        private List<CharNode> next = null;
        public CharNode(char value)
        {
            this.value = value;
        }
        public int Count()
        {
            return next == null ? 0 : next.Count;
        }
        public void AddNode(char value)
        {
            next ??= new();
            CharNode newNode = new(value)
            {
                sum = value == NULLCHAR ? sum : (sum + value),
                parent = this
            };
            next.Add(newNode);
        }
        public void AddNode(CharNode node)
        {
            next ??= new();
            node.sum = node.value == NULLCHAR ? sum : (sum + node.value);
            node.parent = this;
            next.Add(node);
        }
        public CharNode RandomGet(out int index)
        {
            index = UnityEngine.Random.Range(0, next.Count);
            return next[index];
        }

        public string PopResult()
        {
            CharNode cur = this;
            List<int> buffer = new();
            while (cur.Count() != 0)
            {
                cur = cur.RandomGet(out int index);
                buffer.Add(index);
                if (cur.value == NULLCHAR) break; // for results of varying lengths
            }
            string res = cur.Sum;
            do
            {
                CharNode parent = cur.parent;
                if (parent == null) break;
                cur.parent = null;
                parent.next.RemoveAt(buffer[^1]);
                buffer.RemoveAt(buffer.Count - 1);
                cur = parent;
            }
            while (cur.Count() == 0);

            return res;
        }
    }

    private static readonly Dictionary<string, CharNode> randomPool = new();

    public UnityEvent<string> getValue = new();

    [ReadonlyField] public int DebugCount = 0, DebugCountMax = 0;

    public string ReturnValue()
    {
        if (randomPool.ContainsKey(key) && randomPool[key].Count() == 0) randomPool.Remove(key);
        if (!randomPool.ContainsKey(key))
        {
            Debug.Log("New random pool");
            DebugCountMax = DebugCount;
            DebugCount = 0;
            List<CharNode> curBuffer = new() { new('\0') };
            randomPool[key] = curBuffer[^1];
            HashSet<char> duplicates;
            for (int i = 0; i++ < maxLength;)
            {

                if (i > minLength)
                {
                    foreach (CharNode n in curBuffer)
                    {
                        n.AddNode(new CharNode('\0'));
                    }
                }

                if (i == maxLength)
                {
                    foreach (CharNode n in curBuffer)
                    {
                        duplicates = Fn.StringCountDuplicates(n.Sum);
                        if (duplicates.Count < maxDuplicates)
                        {
                            foreach (char v in setChar)
                            {
                                n.AddNode(new CharNode(v));
                            }
                        }
                        else
                        {
                            foreach (char v in duplicates)
                            {
                                n.AddNode(new CharNode(v));
                            }
                        }
                    }
                }
                else
                {
                    List<CharNode> nextBuffer = new();
                    foreach (CharNode n in curBuffer)
                    {
                        duplicates = Fn.StringCountDuplicates(n.Sum);
                        if (duplicates.Count < maxDuplicates)
                        {
                            foreach (char v in setChar)
                            {
                                nextBuffer.Add(new(v));
                                n.AddNode(nextBuffer[^1]);
                            }
                        }
                        else
                        {
                            foreach (char v in duplicates)
                            {
                                nextBuffer.Add(new(v));
                                n.AddNode(nextBuffer[^1]);
                            }
                        }
                    }
                    List<CharNode> temp = curBuffer;
                    curBuffer = nextBuffer;
                    temp.Clear();
                }
            }
        }
        DebugCount++;
        return randomPool[key].PopResult();
    }

    public void GetValue()
    {
        getValue?.Invoke(ReturnValue());
    }
}
