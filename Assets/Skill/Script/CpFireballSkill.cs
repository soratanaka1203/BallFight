//using UnityEngine;

//[CreateAssetMenu(fileName = "FireballSkill", menuName = "Skills/Fireball")]
//public class CpFireballSkill : SkillData
//{
//    public UnityEngine.GameObject fireballPrefab; // 発射する火の玉のプレハブ
//    public float fireballSpeed = 30f;
//    public float fireballOffset = 10f; // プレイヤーの前方に発射位置をオフセットする距離

//    public override void CpActivateSkill(CpPlayer player)
//    {
//        if (fireballPrefab != null)
//        {
//            // CpPlayer の Rigidbody から進行方向を取得
//            Vector3 direction = player.GetComponent<Rigidbody>().velocity.normalized;

//            if (direction != Vector3.zero) // 進行方向がゼロでない場合
//            {
//                // プレイヤーの進行方向にオフセットを加えて弾の生成位置を決定
//                Vector3 spawnPosition = player.transform.position + (direction * fireballOffset);

//                // プレイヤーの進行方向に合わせて弾をインスタンス化
//                UnityEngine.GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.LookRotation(direction));

//                // 弾の Rigidbody を取得して速度を設定
//                Rigidbody rb = fireball.GetComponent<Rigidbody>();
//                if (rb != null)
//                {
//                    Vector3 velocity = direction * fireballSpeed;
//                    velocity.y = 0f; // Y軸の速度をゼロに設定

//                    // 設定した速度で弾を発射
//                    rb.velocity = velocity;
//                }
//            }
//        }

//        if (skillEffectPrefab != null)
//        {
//            Instantiate(skillEffectPrefab, player.transform.position, Quaternion.identity);
//        }
//    }
//}
