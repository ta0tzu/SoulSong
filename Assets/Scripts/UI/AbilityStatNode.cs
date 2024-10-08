using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
public class AbilityStatNode : MonoBehaviour
{
    public AbilityStat node;

    public AbilityStatNode[] connectedNodes;
    public GameObject[] connectionLines;

    public bool grieveUnlocked, macUnlocked, fieldUnlocked, riggsUnlocked, solaceUnlocked, blueUnlocked;
    public int nodeIndex;
    public bool skillNode;


    private void Start()
    {
        if (node != null)
        {
            if (node.skill != null)
            {
                node.nodeName = node.skill.skillName;
                node.nodeDescription = node.skill.skillDescription;
            }

            if (node.drop != null)
            {
                node.nodeName = node.drop.dropName;
                node.nodeDescription = node.drop.dropDescription;
            }
        }
    }

    public void OnClickEvent()
    {
        bool connectionCheck = false;

        if (Engine.e.gridReference.grieveScreen)
        {
            if (node != null)
            {
                if (!grieveUnlocked)
                {
                    for (int i = 0; i < connectedNodes.Length; i++)
                    {
                        if (connectedNodes[i].grieveUnlocked)
                        {
                            connectionCheck = true;
                            break;
                        }
                    }

                    if (connectionCheck)
                    {

                        Engine.e.party[0].GetComponent<Character>().maxHealth += node.healthIncrease;
                        Engine.e.party[0].GetComponent<Character>().maxMana += node.manaIncrease;
                        Engine.e.party[0].GetComponent<Character>().maxEnergy += node.energyIncrease;
                        Engine.e.party[0].GetComponent<Character>().strength += node.strengthIncrease;

                        if (node.skill != null)
                        {
                            if (!Engine.e.party[0].GetComponent<Character>().KnowsSkill(node.skill))
                            {
                                Engine.e.party[0].GetComponent<Character>().skills[node.skill.skillIndex] = node.skill;
                            }
                        }

                        if (node.drop != null)
                        {
                            if (!Engine.e.party[0].GetComponent<Character>().KnowsDrop(node.drop))
                            {
                                Engine.e.party[0].GetComponent<Character>().drops[node.drop.dropIndex] = node.drop;
                            }
                        }

                        grieveUnlocked = true;
                        Debug.Log("Unlocked!");

                        for (int i = 0; i < connectionLines.Length; i++)
                        {
                            if (connectionLines[i] != null && connectionLines[i].GetComponent<SpriteShapeRenderer>().color != Color.cyan)
                            {
                                connectionLines[i].GetComponent<SpriteShapeRenderer>().color = Color.cyan;
                            }
                        }
                        Engine.e.gridReference.grievePosition = nodeIndex;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }

        if (Engine.e.gridReference.macScreen)
        {

            Engine.e.gridReference.macPosition = nodeIndex;

            if (node != null)
            {
                if (!macUnlocked)
                {
                    for (int i = 0; i < connectedNodes.Length; i++)
                    {
                        if (connectedNodes[i].macUnlocked)
                        {
                            connectionCheck = true;
                            break;
                        }
                    }

                    if (connectionCheck)
                    {
                        Engine.e.party[1].GetComponent<Character>().maxHealth += node.healthIncrease;
                        Engine.e.party[1].GetComponent<Character>().maxMana += node.manaIncrease;
                        Engine.e.party[1].GetComponent<Character>().maxEnergy += node.energyIncrease;
                        Engine.e.party[1].GetComponent<Character>().strength += node.strengthIncrease;

                        if (node.skill != null)
                        {
                            if (!Engine.e.party[1].GetComponent<Character>().KnowsSkill(node.skill))
                            {
                                Engine.e.party[1].GetComponent<Character>().skills[node.skill.skillIndex] = node.skill;
                            }
                        }

                        if (node.drop != null)
                        {
                            if (!Engine.e.party[1].GetComponent<Character>().KnowsDrop(node.drop))
                            {
                                Engine.e.party[1].GetComponent<Character>().drops[node.drop.dropIndex] = node.drop;
                            }
                            Debug.Log("Unlocked!");

                        }
                        macUnlocked = true;
                        for (int i = 0; i < connectionLines.Length; i++)
                        {
                            if (connectionLines[i] != null && connectionLines[i].GetComponent<SpriteShapeRenderer>().color != Color.blue)
                            {
                                connectionLines[i].GetComponent<SpriteShapeRenderer>().color = Color.blue;
                            }
                        }
                        Engine.e.gridReference.macPosition = nodeIndex;

                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }

        if (Engine.e.gridReference.fieldScreen)
        {
            if (node != null)
            {
                if (!fieldUnlocked)
                {
                    for (int i = 0; i < connectedNodes.Length; i++)
                    {
                        if (connectedNodes[i].fieldUnlocked)
                        {
                            connectionCheck = true;
                            break;
                        }
                    }

                    if (connectionCheck)
                    {
                        Engine.e.party[2].GetComponent<Character>().maxHealth += node.healthIncrease;
                        Engine.e.party[2].GetComponent<Character>().maxMana += node.manaIncrease;
                        Engine.e.party[2].GetComponent<Character>().maxEnergy += node.energyIncrease;
                        Engine.e.party[2].GetComponent<Character>().strength += node.strengthIncrease;

                        if (node.skill != null)
                        {
                            if (!Engine.e.party[2].GetComponent<Character>().KnowsSkill(node.skill))
                            {
                                Engine.e.party[2].GetComponent<Character>().skills[node.skill.skillIndex] = node.skill;
                            }
                        }

                        if (node.drop != null)
                        {
                            if (!Engine.e.party[2].GetComponent<Character>().KnowsDrop(node.drop))
                            {
                                Engine.e.party[2].GetComponent<Character>().drops[node.drop.dropIndex] = node.drop;
                            }
                        }
                        fieldUnlocked = true;
                        for (int i = 0; i < connectionLines.Length; i++)
                        {
                            if (connectionLines[i] != null)
                            {
                                connectionLines[i].GetComponent<SpriteShapeRenderer>().color = Color.green;
                            }
                        }
                        Engine.e.gridReference.fieldPosition = nodeIndex;

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            if (Engine.e.gridReference.riggsScreen)
            {
                if (node != null)
                {
                    if (!riggsUnlocked)
                    {
                        for (int i = 0; i < connectedNodes.Length; i++)
                        {
                            if (connectedNodes[i].riggsUnlocked)
                            {
                                connectionCheck = true;
                                break;
                            }
                        }

                        if (connectionCheck)
                        {
                            Engine.e.party[3].GetComponent<Character>().maxHealth += node.healthIncrease;
                            Engine.e.party[3].GetComponent<Character>().maxMana += node.manaIncrease;
                            Engine.e.party[3].GetComponent<Character>().maxEnergy += node.energyIncrease;
                            Engine.e.party[3].GetComponent<Character>().strength += node.strengthIncrease;

                            if (node.skill != null)
                            {
                                if (!Engine.e.party[3].GetComponent<Character>().KnowsSkill(node.skill))
                                {
                                    Engine.e.party[3].GetComponent<Character>().skills[node.skill.skillIndex] = node.skill;
                                }
                            }

                            if (node.drop != null)
                            {
                                if (!Engine.e.party[3].GetComponent<Character>().KnowsDrop(node.drop))
                                {
                                    Engine.e.party[3].GetComponent<Character>().drops[node.drop.dropIndex] = node.drop;
                                }
                            }
                            riggsUnlocked = true;
                            for (int i = 0; i < connectionLines.Length; i++)
                            {
                                if (connectionLines[i] != null)
                                {
                                    connectionLines[i].GetComponent<SpriteShapeRenderer>().color = Color.yellow;
                                }
                            }
                            Engine.e.gridReference.riggsPosition = nodeIndex;

                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}