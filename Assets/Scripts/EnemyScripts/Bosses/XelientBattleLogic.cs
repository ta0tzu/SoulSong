using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XelientBattleLogic : MonoBehaviour
{
    Enemy enemy;
    int moveCounter = 0;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void Logic(int target)
    {

        if (Engine.e.battleSystem.enemies[0].currentHealth < Engine.e.battleSystem.enemies[0].maxHealth * (0.2f)
        && enemy.currentMana >= enemy.drops[0].dropCost)
        {

            Engine.e.battleSystem.attackingTeam = true;
            Engine.e.battleSystem.enemyAttackDrop = true;
            Engine.e.battleSystem.lastDropChoice = enemy.drops[0];
            enemy.DropEffect(enemy.drops[0]);

            Engine.e.battleSystem.InstantiateDropAnim(this.gameObject, enemy.drops[0]);
            enemy.currentMana -= enemy.drops[0].dropCost;

            Engine.e.battleSystem.enemyAttacking = false;
        }
        else
        {
            if (moveCounter == 0 || moveCounter == 1 || moveCounter == 3 || moveCounter == 4
            || moveCounter == 5)
            {
                Engine.e.battleSystem.enemyMoving = true;
                Engine.e.battleSystem.enemyAttacking = true;
                Engine.e.battleSystem.isDead = Engine.e.activeParty.activeParty[target].GetComponent<Character>().TakePhysicalDamage(target, enemy.physicalDamage);

                moveCounter++;
            }

            if (moveCounter == 2 || moveCounter == 6)
            {
                int enemyDropChoice = Random.Range(0, enemy.drops.Length - 1);

                if (enemy.currentMana >= enemy.drops[enemyDropChoice].dropCost)
                {
                    Engine.e.battleSystem.enemyAttackDrop = true;

                    Engine.e.battleSystem.lastDropChoice = enemy.drops[enemyDropChoice];
                    Engine.e.battleSystem.InstantiateDropAnim(this.gameObject, enemy.drops[enemyDropChoice]);

                    Engine.e.activeParty.activeParty[target].GetComponent<Character>().DropEffect(enemy.drops[enemyDropChoice]);

                    enemy.currentMana -= enemy.drops[enemyDropChoice].dropCost;

                    Engine.e.battleSystem.enemyAttacking = false;

                }
                else
                {
                    Engine.e.battleSystem.enemyMoving = true;
                    Engine.e.battleSystem.enemyAttacking = true;
                    Engine.e.battleSystem.isDead = Engine.e.activeParty.activeParty[target].GetComponent<Character>().TakePhysicalDamage(target, enemy.physicalDamage);
                }
                moveCounter++;
            }

            if (moveCounter == 7)
            {
                if (enemy.currentMana >= enemy.drops[2].dropCost)
                {
                    Engine.e.battleSystem.enemyAttackDrop = true;

                    Engine.e.battleSystem.lastDropChoice = enemy.drops[2];
                    Engine.e.battleSystem.InstantiateDropAnim(this.gameObject, enemy.drops[2]);

                    Engine.e.activeParty.activeParty[target].GetComponent<Character>().DropEffect(enemy.drops[2]);

                    enemy.currentMana -= enemy.drops[2].dropCost;

                    Engine.e.battleSystem.enemyAttacking = false;

                }
                else
                {
                    Engine.e.battleSystem.enemyMoving = true;
                    Engine.e.battleSystem.enemyAttacking = true;
                    Engine.e.battleSystem.isDead = Engine.e.activeParty.activeParty[target].GetComponent<Character>().TakePhysicalDamage(target, enemy.physicalDamage);
                }
                moveCounter = 0;
            }

            Engine.e.battleSystem.partyCheckNext = false;


        }
    }
}
