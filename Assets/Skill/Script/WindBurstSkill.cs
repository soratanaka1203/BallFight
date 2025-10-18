using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WindBurstSkill", menuName = "Skills/WindBurst")]
public class WindBurstSkill : SkillData
{
    public UnityEngine.GameObject windEffectPrefab; // 風のエフェクトのプレハブ
    public float pushForce = 20f; // 押し出す力
    public float skillDuration = 3f; // スキルの持続時間
    private UnityEngine.GameObject playEffect;
    private UnityEngine.GameObject activeEffect; // 現在の風エフェクト

    public override void ActivateSkill(PlayerController player)
    {
        if (activeEffect == null)
        {
            // 親を設定せずにエフェクトを生成
            activeEffect = Instantiate(windEffectPrefab, player.transform.position, Quaternion.identity);
            playEffect = Instantiate(skillEffectPrefab, player.transform.position, Quaternion.identity);

            // 追従スクリプトを追加してプレイヤーをターゲットに設定
            activeEffect.AddComponent<FollowPlayer>().SetTarget(player.transform);
            playEffect.AddComponent<FollowPlayer>().SetTarget(player.transform);

            activeEffect.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

            // 風の影響を与える処理を開始
            player.StartCoroutine(ApplyWindEffect(player));
        }
    }
    private IEnumerator ApplyWindEffect(PlayerController player)
    {
        float elapsedTime = 0f;

        while (elapsedTime < skillDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, 6f);

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player") || col.CompareTag("Shadow"))
                {
                    Rigidbody rb = col.GetComponent<Rigidbody>();
                    if (rb != null && col.gameObject != player.gameObject)
                    {
                        Vector3 pushDirection = (col.transform.position - player.transform.position).normalized;
                        rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                    }
                }
            }

            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        // スキルが終わったらエフェクトを消す
        if (activeEffect != null)
        {
            Destroy(activeEffect);
            Destroy(playEffect);
            activeEffect = null;
        }
    }
}
