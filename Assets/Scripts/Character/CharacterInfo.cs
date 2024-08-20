using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{

    private GameObject character;
    private Vector3 initialGlobalScale;
    [SerializeField] Camera mainCam;

    private void Awake()
    {
        character = transform.parent.gameObject;
        initialGlobalScale = transform.lossyScale;
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        gameObject.transform.rotation = Quaternion.Euler(50f, 0f, 0f);
        ChangeScale(FindCharacterWithHighestKillCount());
    }

    public void ChangeScale(GameObject highestKillCharacter)
    {
        if (highestKillCharacter != null)
        {
            Attack highestAttack = highestKillCharacter.GetComponent<Attack>();
            if (highestAttack != null)
            {
                float scaleMultiplier = 1.0f + (highestAttack.killCount * 0.1f);
                Vector3 newGlobalScale = initialGlobalScale * scaleMultiplier;
                transform.localScale = new Vector3(
                    newGlobalScale.x / character.transform.lossyScale.x,
                    newGlobalScale.y / character.transform.lossyScale.y,
                    newGlobalScale.z / character.transform.lossyScale.z
                );
            }
        }
    }

    private GameObject FindCharacterWithHighestKillCount()
    {
        Attack[] allCharacters = FindObjectsOfType<Attack>();
        GameObject highestKillCharacter = null;
        int highestKillCount = 0;

        foreach (Attack attack in allCharacters)
        {
            if (attack.killCount > highestKillCount)
            {
                highestKillCount = attack.killCount;
                highestKillCharacter = attack.gameObject;
            }
        }

        return highestKillCharacter;
    }
}
