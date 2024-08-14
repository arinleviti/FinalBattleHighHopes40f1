using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

	public class Midpoint : MonoBehaviour
	{
		private List<GameObject> turnList = new List<GameObject>();
		private CombatManager combatManagerRef;
		public Camera mainCamera;
		public Vector3 cameraOffset; // Offset from the midpoint for camera position

		void Start()
		{
			combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
			mainCamera.transform.SetParent(transform, true);
		}

		void Update()
		{
			if (combatManagerRef != null)
			{
				turnList = combatManagerRef.TurnList;
				Transform player = turnList.FirstOrDefault(go => go.CompareTag("Player"))?.transform;
				Transform[] monsters = turnList.Where(go => go.CompareTag("Monster")).Select(go => go.transform).ToArray();

				if (player != null && monsters.Length > 0)
				{
					Vector3 averageMonsterPosition = Vector3.zero;
					foreach (Transform monster in monsters)
					{
						averageMonsterPosition += monster.position;
					}
					averageMonsterPosition /= monsters.Length;

					Vector3 midpointPosition = (player.position + averageMonsterPosition) / 2;
					transform.position = midpointPosition;

					// Make the midpoint face the player
					transform.LookAt(player);

					// Calculate the new camera position based on the midpoint position
					Vector3 directionToPlayer = (player.position - midpointPosition).normalized;

					// Calculate the new camera position by adding the midpoint's position to a vector that is perpendicular to the line from the midpoint to the player, rotated 90 degrees around the Y-axis, scaled by the magnitude of the camera offset.
					Vector3 newCameraPosition = midpointPosition + Quaternion.Euler(0, 90, 30) * directionToPlayer * cameraOffset.magnitude;

					// Ensure the Y offset is correctly applied
					newCameraPosition.y = midpointPosition.y + cameraOffset.y;

					// Set the new camera position
					mainCamera.transform.position = newCameraPosition;
					
					// Find a point which is a few meters above the midpoint and have the camera look at it.
					Vector3 cameraLookHere = new Vector3(transform.position.x,transform.position.y +1f, transform.position.z);

					// Make the camera look at the midpoint
					mainCamera.transform.LookAt(cameraLookHere);
				}
			}
		}
	}

