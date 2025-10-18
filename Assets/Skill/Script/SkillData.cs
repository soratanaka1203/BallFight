using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    public string skillName;           // スキル名
    public string description;         // スキルの説明
    public float cooldownTime;         // クールダウン時間
    public UnityEngine.GameObject skillEffectPrefab; // スキルのエフェクト
    public AudioClip skillSE;           //効果音

    public abstract void ActivateSkill(PlayerController player);
}
