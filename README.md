The `CombatManager` class is responsible for creating the turn list and initiating the `TurnManager` method, which manages the instantiation of the `PlayerController` and `MonsterController` scripts. `CombatManager` uses yield return new WaitUntil() to check if a character has finished their turn. It also tracks the remaining moves for each character, with each character having 2 moves available per turn.

When activated, the `PlayerController` and `MonsterController` classes handle character movements. The player (or Zombie Slayer) is user-controlled, utilizing the mouse to navigate and attack via the ScreenPointToRay method. If the user clicks within the range indicator, the Zombie Slayer will move to the specified location. Clicking on a zombie prompts the game to present a list of available attacks (currently, only "Punch" is available). The `PlayerController` class also configures the player as a NavMeshAgent.

The `MonsterController` class manages similar functionality, but uses AI to move the monster towards the player and attack when in range. When a character attacks, both `PlayerController` and `MonsterController` instantiate the `UIManager` class, which retrieves the character game objects and their attached scripts, preparing all necessary elements to calculate damage and HP. While both controllers use `UIManager`, the `PlayerController` additionally creates the "select attack" button.

The `UIManager` class instantiates the `ActionChoices` class, where elements such as attack type and character stats are calculated using the HandleAttackChoice method to determine the attack outcome. Once the process completes, the objects are destroyed, and the process restarts for the next character.

## Notes

- If the Zombie Slayer decides to use a potion, the `UIManager` is bypassed.
- Currently, the only class that properly implements Dependency Injection (DI) is `ActionChoices`. Implementing DI throughout the project is still a work in progress.
- Object pooling is likely to be implemented instead of the current initialize/destroy object approach.
- The zombies and the Zombie Slayer are assets obtained from the Unity Asset Store.
