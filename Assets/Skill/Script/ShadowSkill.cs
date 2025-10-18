using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowSkill", menuName = "Skills/Shadow")]
public class ShadowSkill : SkillData
{
    public GameObject shadowPrefab; // シャドウのプレハブ
    public float spawnOffset = 3f;  // プレイヤーからのオフセット

    public override void ActivateSkill(PlayerController player)
    {
        if (shadowPrefab != null)
        {
            // プレイヤーのRigidbodyから進行方向を取得
            Vector3 direction = player.GetComponent<Rigidbody>().velocity.normalized;

            if (direction != Vector3.zero) // 進行方向がゼロでない場合
            {
                // プレイヤーの進行方向にオフセットを加えて弾の生成位置を決定
                Vector3 spawnPosition = player.transform.position + (direction * spawnOffset);

                // シャドウをプレイヤーの位置に召喚
                GameObject shadow = Instantiate(shadowPrefab, spawnPosition, Quaternion.identity);

                // シャドウを初期化（必要に応じて）
                CpShadow shadowScript = shadow.GetComponent<CpShadow>();
                if (shadowScript != null)
                {
                    shadowScript.excludePlayer = player.gameObject.transform;
                    shadowScript.currentState = CpShadow.CpState.RandomMove; // ランダム移動状態で開始
                }
            }

            if (skillEffectPrefab != null)
            {
                Instantiate(skillEffectPrefab, player.transform.position, Quaternion.identity);
            }
        }
    }
}
