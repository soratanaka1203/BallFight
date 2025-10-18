//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CpPlayer : MonoBehaviour
//{
//    public enum CpState
//    {
//        Attack,    //攻撃
//        GetItem,   // アイテムを取りに行く
//        RandomMove, // ランダム移動
//        Return,     //復帰
//    }

//    public CpState currentState = CpState.RandomMove; // 初期状態をランダム移動に設定
//    private List<Transform> players = new List<Transform>(); // 全プレイヤーの位置
//    public Transform target; // ターゲットの位置

//    private float moveSpeed = 25f; // 移動速度
//    public float detectionRange = 5f; // プレイヤーを検知する距離
//    private Rigidbody rb;

//    // ダッシュやジャンプなどの追加機能
//    public float dashSpeed = 10f;
//    public float dashDuration = 0.2f;
//    private bool isDashing = false;
//    public float dashCooldown = 1f;
//    private bool canDash = true;

//    public float jumpForce = 10f;
//    private bool isGrounded = true;

//    [SerializeField] GameObject groundObject;  //ステージのオブジェクト
//    Transform groundTr; //ステージの位置

//    // ステージの境界
//    Bounds bounds;

//    [SerializeField] private GameManager gameManager;
//    [SerializeField] private CharacterData characterData; // キャラデータ（スキル情報含む）
//    [SerializeField] private GameObject chargeEffect;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        if (target == null)
//        {
//            target = GameObject.FindGameObjectWithTag("Player").transform;
//        }
//        if (groundObject == null)
//        {
//            groundObject = GameObject.FindGameObjectWithTag("Ground");
//            groundTr = groundObject.transform;
//        }
//        if (groundTr == null)
//        {
//            groundTr = groundObject.transform;
//        }

//        // ステージの境界を取得
//        Collider collider = groundObject.GetComponent<Collider>();
//        bounds = collider.bounds;

//        StartCoroutine(AIBehavior());
//    }

//    void FixedUpdate()
//    {
//        switch (currentState)
//        {
//            case CpState.Attack:
//                ChasePlayerBehavior();
//                break;

//            case CpState.GetItem:
//                // アイテム取りに行く動き
//                break;

//            case CpState.RandomMove:
//                RandomMoveBehavior();
//                break;

//            case CpState.Return:
//                ReturnToGroundPosition();
//                break;
//        }
//    }

//    private void ChasePlayerBehavior()
//    {
//        // プレイヤーの位置に向かって移動
//        Vector3 direction = (target.position - transform.position).normalized;
//        rb.AddForce(direction * moveSpeed);
//    }

//    private void RandomMoveBehavior()
//    {
//        if (rb.velocity == Vector3.zero)
//        {
//            // ランダムな方向を決定
//            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
//            rb.AddForce(randomDirection * moveSpeed);
//        }
//    }

//    private void ReturnToGroundPosition()
//    {
//        // 初期位置に戻る動き
//        Vector3 direction = (Vector3.zero - (Vector3)transform.position).normalized;
//        rb.AddForce(direction * moveSpeed);
//        Jump();
//        StartCoroutine(Dash());
//    }

//    private IEnumerator Dash()
//    {
//        if (canDash && !isDashing)
//        {
//            canDash = false;
//            isDashing = true;
//            Vector3 dashDirection = (target.position - transform.position).normalized;
//            rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

//            yield return new WaitForSeconds(dashDuration);

//            isDashing = false;
//            yield return new WaitForSeconds(dashCooldown);

//            canDash = true;
//        }
//    }

//    private void Jump()
//    {
//        if (isGrounded)
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//            isGrounded = false;
//        }
//    }

//    private IEnumerator UseSkill()
//    {
//        characterData.skill.ActivateSkill(this); // キャラごとのスキルを発動
//        if (characterData.skill.skillSE != null)
//        {
//            gameManager.PlaySE(characterData.skill.skillSE, 0.5f);//効果音再生
//        }
//        canUseSkill = false;

//        yield return new WaitForSeconds(characterData.skill.cooldownTime);

//        canUseSkill = true;

//        // パーティクルシステムのインスタンスを生成
//        if (chargeEffect != null)
//        {
//            GameObject effectInstance = Instantiate(chargeEffect, transform.position, Quaternion.identity);
//            Destroy(effectInstance.gameObject, 1f);
//        }
//    }

//    private IEnumerator AIBehavior()
//    {
//        while (true)
//        {
//            // ランダムに行動を選択
//            float randomAction = Random.Range(0f, 1f);

//            if (randomAction < 0.4f) // 攻撃 (40%)
//            {
//                // ステージ外にいるなら復帰する
//                if (!IsWithinStage(transform.position))
//                {
//                    currentState = CpState.Return;
//                }
//                else
//                {
//                    currentState = CpState.Attack;
//                    StartCoroutine(Dash());
//                }
//            }
//            else if (randomAction < 0.6f) // スキル発動 (20%)
//            {
//                // ステージ外にいるなら復帰する
//                if (!IsWithinStage(transform.position))
//                {
//                    currentState = CpState.Return;
//                }
//                else
//                {
//                    StartCoroutine(UseSkill());
//                }
//            }
//            else // ランダム移動(40%)
//            {
//                // ステージ外にいるなら復帰する
//                if (!IsWithinStage(transform.position))
//                {
//                    currentState = CpState.Return;
//                }
//                else
//                {
//                    currentState = CpState.RandomMove;
//                }
//            }

//            yield return new WaitForSeconds(1f); // 次の行動までの待機時間
//        }
//}

//    // 地面に触れたか
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
//        }
//    }

//    // ステージ範囲内かどうかを判定するヘルパーメソッド
//    bool IsWithinStage(Vector3 pos)
//    {
//        return pos.x >= bounds.min.x && pos.x <= bounds.max.x &&
//               pos.z >= bounds.min.z && pos.z <= bounds.max.z;
//    }
//}
