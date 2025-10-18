using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "IceWallSkill", menuName = "Skills/IceWall")]
public class IceWallSkill : SkillData
{
    public UnityEngine.GameObject iceWallPrefab; // 氷の壁のプレハブ
    private UnityEngine.GameObject activeWall; // 現在の氷の壁
    public float wallDuration = 5f; // 壁の持続時間

    public override void ActivateSkill(PlayerController player)
    {
        if (activeWall == null)
        {
            // プレイヤーの移動方向を取得
            Rigidbody rb = player.GetComponent<Rigidbody>();
            Vector3 moveDirection = rb.velocity.normalized;

            // もし移動していない場合は、向いている方向を使う
            if (moveDirection.magnitude < 0.1f)
            {
                moveDirection = player.transform.forward;
            }

            // プレイヤーの進行方向に壁を生成
            Vector3 spawnPosition = player.transform.position + moveDirection * 2f;
            activeWall = Instantiate(iceWallPrefab, spawnPosition, Quaternion.identity);

            // 壁の向きをプレイヤーの向きに合わせる
            activeWall.transform.rotation = Quaternion.LookRotation(moveDirection);

            // 一定時間後に壁を消す
            player.StartCoroutine(DeactivateWall());
        }

        if (skillEffectPrefab != null)
        {
            Instantiate(skillEffectPrefab, player.transform.position, Quaternion.identity);
        }
    }

    private IEnumerator DeactivateWall()
    {
        yield return new WaitForSeconds(wallDuration);

        if (activeWall != null)
        {
            Destroy(activeWall);
            activeWall = null;
        }
    }
}
