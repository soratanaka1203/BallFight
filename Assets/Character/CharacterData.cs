using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterIcon;
    public UnityEngine.GameObject characterPrefab; // キャラのプレハブ
    public SkillData skill;           // キャラクターの固有スキル
}
