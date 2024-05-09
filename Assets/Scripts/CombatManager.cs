using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //private string tag1 = "Player";
    //private string tag2 = "Monster";

    public List<GameObject> TurnList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        TurnList.Add(GameObject.FindGameObjectWithTag("Player"));
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Monster")) 
        {
            TurnList.Add(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
