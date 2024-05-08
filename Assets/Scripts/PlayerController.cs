using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Canvas canvas;
	public GameObject Marker;
	public float movespeed = 5f;
	private Vector3 clickPosition;
	private RaycastHit Hit;
	private bool isUIManagerActivated = false;
	public GameObject UIManagerRef;

	// Start is called before the first frame update
	void Start()
	{
		Marker.SetActive(false);
		canvas.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			clickPosition = new Vector3();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				clickPosition = hit.point;
				Hit = hit;
			}

		}
		if (clickPosition != Vector3.zero && Hit.collider.CompareTag("Monster"))
		{

			Vector3 direction = clickPosition - transform.position;
			direction.Normalize();
			transform.position += direction * movespeed * Time.deltaTime;

			if (Vector3.Distance(transform.position, clickPosition) < 1f)
			{
				clickPosition = Vector3.zero;
				ActivateUIManager();
			}
		}

	}

	void ActivateUIManager()
	{
		if (!isUIManagerActivated)
		{
			UIManagerRef.SetActive(true);
			isUIManagerActivated = true;
			DynamicButtonGenerator DBGScriptRef = UIManagerRef.GetComponent<DynamicButtonGenerator>();
			DBGScriptRef.targetCharacterGO = Hit.collider.gameObject;
			Debug.Log(DBGScriptRef.targetCharacterGO);
			DBGScriptRef.attackerGO = gameObject;
		}
	}
}
