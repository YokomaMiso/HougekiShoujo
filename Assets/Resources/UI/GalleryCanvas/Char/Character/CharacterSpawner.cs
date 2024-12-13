using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public CharacterDatabase characterDatabase;

    public void SpawnCharacter(string characterName, Vector3 position, int animatorIndex)
    {
        CharacterData data = characterDatabase.GetCharacterData(characterName);
        if (data != null)
        {
            if (animatorIndex < 0 || animatorIndex >= data.animatorControllers.Count)
            {
                return;
            }

            GameObject newCharacter = new GameObject(data.charData.characterName);
            Animator animator = newCharacter.AddComponent<Animator>();
            animator.runtimeAnimatorController = data.animatorControllers[animatorIndex];
            newCharacter.transform.position = position;
        }
        else
        {
            Debug.LogError($"Character {characterName} not found in database.");
        }
    }
}
