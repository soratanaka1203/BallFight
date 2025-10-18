using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LightningDashSkill", menuName = "Skills/LightningDash")]
public class LightningDashSkill : SkillData
{
    public float dashMultiplier = 2.5f;  // ダッシュ速度の倍率
    public float duration = 3f;         // スキルの持続時間
    public float knockbackForce = 15f;  // ノックバックの強さ

    private UnityEngine.GameObject activeEffect;

    public override void ActivateSkill(PlayerController player)
    {
        player.StartCoroutine(ApplyLightningDash(player));
    }

    private IEnumerator ApplyLightningDash(PlayerController player)
    {
        float originalSpeed = player.dashSpeed;
        player.dashSpeed *= dashMultiplier;

        // ノックバック処理を有効化
        LightningDashEffect effect = player.GetComponent<LightningDashEffect>();
        effect.Activate(knockbackForce);

        // エフェクト生成
        if (skillEffectPrefab)
        {
            activeEffect = Instantiate(skillEffectPrefab, player.transform.position, Quaternion.identity);
            activeEffect.transform.SetParent(player.transform);
        }

        yield return new WaitForSeconds(duration);

        // スキル終了
        player.dashSpeed = originalSpeed;
        effect.Deactivate();

        if (activeEffect) Destroy(activeEffect);
    }
}
