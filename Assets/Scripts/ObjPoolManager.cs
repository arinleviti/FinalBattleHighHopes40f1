using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System.Linq;

public class ObjPoolManager : MonoBehaviour
{
    private static ObjPoolManager instance;
    
    private Dictionary<string, GameObject> objectPools = new Dictionary<string, GameObject>();
    public static ObjPoolManager Instance
    { get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjPoolManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ObjectPoolManager");
                    instance = obj.AddComponent<ObjPoolManager>();
                }
            }
            return instance;
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public GameObject GetPooledObject(string objectName)
    {
        if (objectPools.ContainsKey(objectName))
        {
            GameObject obj = objectPools[objectName];
            if (obj != null)
            {
                obj.SetActive(true);                
            }
            if (obj.CompareTag("PlayerController"))
            {
                PlayerController objScript = obj.GetComponent<PlayerController>();
                objScript.ResetValues();
            }
            if (obj.CompareTag("MonstersController"))
            {
                MonstersController objScript = obj.GetComponent<MonstersController>();
                objScript.ResetValues();
            }          
            if (obj.CompareTag("RangeIndicator"))
            {
                RangeIndicator objScript = obj.GetComponent<RangeIndicator>();
                objScript.ResetValues();
            }
            if (obj.CompareTag("UIManager"))
            {
                UIManager objScript = obj.GetComponent<UIManager>();
                objScript.ResetValues();
            }
            return obj;
        }
        Debug.LogWarning("No available object in the pool with name: " + objectName);
        return null;
    }
    public void ReturnToPool(string objectName, GameObject obj)
    {
        
        obj.SetActive(false);
        //if (obj.CompareTag("UIManager"))
        //{
        //    UIManager objScript = obj.GetComponent<UIManager>();
        //    objScript.ResetValues();
        //}
        if (!objectPools.ContainsKey(objectName))
        {
            objectPools[objectName] = obj;
        }
    }
    
    public void CreateObjectPool(GameObject prefab, string objectName)
    {
        if (!objectPools.ContainsKey(objectName))
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPools[objectName] = obj;
        }
        else
        {
            Debug.LogWarning("ObjectPool with name " + objectName + " already exists.");
        }
    }
    public void ClearAllPools()
    {
        foreach (var pool in objectPools.Values)
        {
            Destroy(pool);
        }
        objectPools.Clear();
    }
    public bool IsObjectInPoolAndActive(string objectName)
    {
        // Check if the dictionary contains the key
        if (objectPools.ContainsKey(objectName))
        {
            // Retrieve the GameObject associated with the key
            GameObject obj = objectPools[objectName];
            // Return true if the object is active in the hierarchy
            return obj.activeInHierarchy;
        }

        // If the key is not present in the dictionary, return false
        return false;
    }
    public bool IsObjectInPoolAndInactive(string objectName)
    {
        // Check if the dictionary contains the key
        if (objectPools.ContainsKey(objectName))
        {
            // Retrieve the GameObject associated with the key
            GameObject obj = objectPools[objectName];
            // Return true if the object is active in the hierarchy
            return !obj.activeInHierarchy;
        }

        // If the key is not present in the dictionary, return false
        return false;
    }
    public bool IsObjectInPool(string objectName)
    {
        if (objectPools.ContainsKey(objectName))
            return true;
        else return false;

    }
}
