using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
	public TextMeshProUGUI playerHPText;
	public TextMeshProUGUI monster1HPText;
	public TextMeshProUGUI monster2HPText;
	public TextMeshProUGUI movesLeft;

	private PlayerStats playerStats;
	private MonsterStats firstMonsterStats;
	private MonsterStats secondMonsterStats;
	private CombatManager combatManager;
	//private GameObject targetCharacterGOSM;
	//private GameObject attackerGOSM;

	private int playerHP;
	private int firstMonsterHP;
	private int secondMonsterHP;
	private int movesLeftInt;

	//private List<ICharacter> charactersIOList;

	// Start is called before the first frame update
	private void Start()
	{
		// Cache the references to GameObjects and components
		GameObject player = GameObject.Find("Player");
		playerStats = player.GetComponent<PlayerStats>();

		GameObject firstMonster = GameObject.Find("Zombie 1");
		firstMonsterStats = firstMonster.GetComponent<MonsterStats>();

		GameObject secondMonster = GameObject.Find("Zombie 2");
		secondMonsterStats = secondMonster.GetComponent<MonsterStats>();

		GameObject combatManagerObject = GameObject.Find("CombatManager");
		combatManager = combatManagerObject.GetComponent<CombatManager>();

		//CreateIOList();
		
	}

	// Update is called once per frame
	void Update()
	{
		//targetCharacterGOSM = GameObject.Find("UIManager").GetComponent<DynamicButtonGenerator>().targetCharacterGO;
		//attackerGOSM = GameObject.Find("UIManager").GetComponent<DynamicButtonGenerator>().attackerGO;

		//playerHP = GameObject.Find("Player").GetComponent<PlayerStats>().HP;
		//firstMonsterHP = GameObject.Find("Zombie 1").GetComponent<MonsterStats>().HP;
		//secondMonsterHP = GameObject.Find("Zombie 2").GetComponent<MonsterStats>().HP;
		playerHP = playerStats.HP;
		firstMonsterHP = firstMonsterStats.HP;
		secondMonsterHP = secondMonsterStats.HP;
		movesLeftInt = combatManager.movesLeft;

		playerHPText.text = $"Player HP: {playerHP}";
		monster1HPText.text = $"Monster 1: {firstMonsterHP}";
		monster2HPText.text = $"Monster 2: {secondMonsterHP}";


		//movesLeftInt = GameObject.Find("CombatManager").GetComponent<CombatManager>().movesLeft;
		movesLeft.text = $"Moves left: {movesLeftInt}";

		
	}

	//private void CreateIOList()
	//{
	//	if (charactersIOList == null)
	//		charactersIOList = new List<ICharacter>();

	//	foreach (GameObject characterGO in combatManager.TurnList)
	//	{
	//		ICharacter character = characterGO.GetComponent<CharacterClass>();

	//		if (character != null)
	//		{
	//			// Add the character to the characters list
	//			charactersIOList.Add(character);
	//		}
	//	}	

	//}
}
