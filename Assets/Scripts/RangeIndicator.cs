using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    /*ublic bool isTargetInRange = false;*/
    public List<GameObject> targetsInRange = new List<GameObject>();
    public Material originalMaterial;
    public Material newMaterial;
    public CombatManager combatManagerScript;
    public float detectionRadius = 5f;
    public LayerMask detectionLayer;

    // Start is called before the first frame update
    void Start()
    {
        //originalMaterial = GetComponent<Material>();
        combatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        targetsInRange = new List<GameObject>();
        targetsInRange = GetInRangeTargets();
    }
    public void ResetValues()
    {
        //isTargetInRange = false;
        //originalMaterial = GetComponent<Material>();
        combatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        targetsInRange = new List<GameObject>();
        targetsInRange = GetInRangeTargets();
        
    }

    public void CheckRange()
    {
        //targetsInRange.Clear();
        detectionRadius = gameObject.transform.localScale.x / 2;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        foreach (Collider collider in colliders)
        {
            GameObject target = collider.gameObject;
            if (!targetsInRange.Contains(target))
            {
                targetsInRange.Add(target);
                Debug.Log($"Added {target.name} to targetsInRange. InstanceID: {target.GetInstanceID()}");
            }
        }
        foreach (GameObject target in targetsInRange)
        {
            Debug.Log($"Range Indicator contains: {target.name} with InstanceID: {target.GetInstanceID()}");
        }
        

    }

    public List<GameObject> GetInRangeTargets()
    {
        CheckRange();
        if ( combatManagerScript.movesLeft < 2)
        {

            GetComponent<Renderer>().material = newMaterial;
        }

        if ( combatManagerScript.movesLeft < 2)
        {

            GetComponent<Renderer>().material = newMaterial;
        }
        if (combatManagerScript.movesLeft == 2)
        {
            GetComponent<Renderer>().material = originalMaterial;
        }
        return targetsInRange;
    }
}
