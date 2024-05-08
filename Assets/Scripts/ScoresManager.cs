using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI monster1HPText;
    public TextMeshProUGUI monster2HPText;

	//private GameObject targetCharacterGOSM;
	//private GameObject attackerGOSM;

	private int playerHP;
    private int firstMonsterHP;
    private int secondMonsterHP;
	

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //targetCharacterGOSM = GameObject.Find("UIManager").GetComponent<DynamicButtonGenerator>().targetCharacterGO;
        //attackerGOSM = GameObject.Find("UIManager").GetComponent<DynamicButtonGenerator>().attackerGO;
        
		playerHP = GameObject.Find("Player").GetComponent<PlayerStats>().HP;
        firstMonsterHP = GameObject.Find("Zombie 1").GetComponent<MonsterStats>().HP;
        secondMonsterHP = GameObject.Find("Zombie 2").GetComponent<MonsterStats>().HP;

        playerHPText.text = $"Player HP: {playerHP}";
        monster1HPText.text = $"Monster 1: {firstMonsterHP}";
        monster2HPText.text = $"Monster 2: {secondMonsterHP}";


    }
}
