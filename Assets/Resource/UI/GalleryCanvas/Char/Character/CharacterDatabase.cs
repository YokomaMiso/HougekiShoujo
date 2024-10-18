using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public List<CharacterData> characterList;

    public CharacterData GetCharacterData(string name)
    {
        return characterList.Find(character => character.characterName == name);
    }
}
