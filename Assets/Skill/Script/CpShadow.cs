using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CpShadow : MonoBehaviour
{
    public enum CpState
    {
        Attack,    //攻撃
        GetItem,   // アイテムを取りに行く
        RandomMove, // ランダム移動
        Return,     //復帰
    }

    public CpState currentState = CpState.RandomMove; // 初期状態をランダム移動に設定
    private List<Transform> players = new List<Transform>(); // 全プレイヤーの位置
    public Transform target; // ターゲットの位置

    private float moveSpeed = 6f; // 移動速度
    private Rigidbody rb;

    // ダッシュやジャンプなどの追加機能
    public float dashSpeed = 5f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    public float dashCooldown = 1f;
    private bool canDash = true;

    public float jumpForce = 3f;
    private bool isGrounded = true;

    [SerializeField] GameObject groundObject;  //ステージのオブジェクト
    Transform groundTr; //ステージの位置

    // ステージの境界
    Bounds bounds;

    // 除外するプレイヤーを指定
    public Transform excludePlayer; // 除外するプレイヤー

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // プレイヤーのターゲットを設定
        players.AddRange(GameObject.FindGameObjectsWithTag("Player").Select(player => player.transform));

        if (players.Count > 0)
        {
            target = players[0]; // 最初のプレイヤーをターゲットに設定
        }

        if (groundObject == null)
        {
            groundObject = GameObject.FindGameObjectWithTag("Ground");
            groundTr = groundObject.transform;
        }
        if (groundTr == null)
        {
            groundTr = groundObject.transform;
        }

        // ステージの境界を取得
        Collider collider = groundObject.GetComponent<Collider>();
        bounds = collider.bounds;

        // 召喚元のプレイヤーを除外するために指定
        // 例えば、CpShadowを呼び出したPlayerをexcludePlayerに設定

        // 除外処理を追加
        players.Remove(excludePlayer);

        StartCoroutine(AIBehavior());
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case CpState.Attack:
                ChasePlayerBehavior();
                break;

            case CpState.GetItem:
                // アイテム取りに行く動き
                break;

            case CpState.RandomMove:
                RandomMoveBehavior();
                break;

            case CpState.Return:
                ReturnToGroundPosition();
                break;
        }
    }

    private void ChasePlayerBehavior()
    {
        // プレイヤーの位置に向かって移動
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed);
        }
    }

    private void RandomMoveBehavior()
    {
        
        // ランダムな方向を決定
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        rb.AddForce(randomDirection * moveSpeed);
    }

    private void ReturnToGroundPosition()
    {
        // 初期位置に戻る動き
        Vector3 direction = (Vector3.zero - (Vector3)transform.position).normalized;
        rb.AddForce(direction * moveSpeed);
        Jump();
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        if (canDash && !isDashing)
        {
            canDash = false;
            isDashing = true;
            Vector3 dashDirection = (target.position - transform.position).normalized;
            rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

            yield return new WaitForSeconds(dashDuration);

            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);

            canDash = true;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private IEnumerator AIBehavior()
    {
        while (true)
        {
            if (target == null)SwitchTarget();

            // ランダムに行動を選択
            float randomAction = Random.Range(0f, 1f);

            if (randomAction < 0.4f) // 攻撃 (40%)
            {
                // ステージ外にいるなら復帰する
                if (!IsWithinStage(transform.position))
                {
                    currentState = CpState.Return;
                }
                else
                {
                    currentState = CpState.Attack;
                    StartCoroutine(Dash());
                }
            }
            else // ランダム移動(60%)
            {
                // ステージ外にいるなら復帰する
                if (!IsWithinStage(transform.position))
                {
                    currentState = CpState.Return;
                }
                else
                {
                    currentState = CpState.RandomMove;
                    // ランダムでターゲットを切り替え
                    SwitchTarget();
                }
            }

            yield return new WaitForSeconds(1f); // 次の行動までの待機時間
        }
    }

    // ターゲットをランダムに切り替え
    private void SwitchTarget()
    {
        if (players.Count > 0)
        {
            target = players[Random.Range(0, players.Count)];
        }
    }

    // 地面に触れたか
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // ステージ範囲内かどうかを判定するヘルパーメソッド
    bool IsWithinStage(Vector3 pos)
    {
        return pos.x >= bounds.min.x && pos.x <= bounds.max.x &&
               pos.z >= bounds.min.z && pos.z <= bounds.max.z;
    }
}
