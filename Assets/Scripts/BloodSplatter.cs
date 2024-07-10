using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
	private GameObject bloodSplatterGO;
	private ParticleSystem bloodSplatterPS;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ActivateCorBlood(GameObject characterHit)
	{
		StartCoroutine(ActivateBlood(characterHit));
	}
	public IEnumerator ActivateBlood(GameObject characterHit)
	{
	
		Destroy(GameObject.Find("BloodPSPrefabM(Clone)"));
		Destroy(GameObject.Find("BloodPSPrefabM(Clone)(Clone)"));
		Destroy(GameObject.Find("BloodPSPrefabP(Clone)"));
		Destroy(GameObject.Find("BloodPSPrefabP(Clone)(Clone)"));
		
		//GameObject bloodSplatterGO = new GameObject();
		//ParticleSystem bloodSplatterPS = new ParticleSystem();

		if (characterHit != null && characterHit.CompareTag("Monster"))
		{
			bloodSplatterGO = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPSPrefabM"));
		}
		else if (characterHit != null && characterHit.CompareTag("Player"))
		{
			bloodSplatterGO = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPSPrefabP"));
		}

		bloodSplatterGO.SetActive(false);
		bloodSplatterPS = bloodSplatterGO.GetComponent<ParticleSystem>();
		bloodSplatterGO.transform.position = characterHit.transform.position;
		Vector3 neckPosition = bloodSplatterGO.transform.position;
		neckPosition.y += 0.7f;
		bloodSplatterGO.transform.position = neckPosition;
		bloodSplatterGO.transform.rotation = characterHit.transform.rotation;
		yield return new WaitForSeconds(0.35f);
		bloodSplatterGO.SetActive(true);
		bloodSplatterPS.Play();
		
	}
}
