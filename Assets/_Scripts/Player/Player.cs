using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int level;
    public int currentDialogueIndex;
    public List<int> CollectibrleIds = new List<int>();
    public List<GameObject> Collectibrles = new List<GameObject>();

    // Game start, For loop, Check ids used
    private void Awake()
    {
        for (int i = 0; i < CollectibrleIds.Count; i++)
        {
            if (!CollectibrleIds.Contains(i))
            {
                Destroy(Collectibrles[i]);
            }
        }
    }
}
