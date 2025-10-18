using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 25f;
    private Vector2 moveInput;
    public float jumpForce = 10f;
    private bool isGrounded = true;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    public float dashCooldown = 1f;
    private bool canDash = true;

    private bool canUseSkill = true;

    private PlayerInput playerInput;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private CharacterData characterData; // キャラデータ（スキル情報含む）
    [SerializeField] private GameObject chargeEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>(); // 各プレイヤーごとに異なるPlayerInputを取得
        gameManager = FindObjectOfType<GameManager>(); // ゲームマネージャーを検索
    }

    void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputValue value)
    {
        if (playerInput != null) // 各プレイヤーの入力を区別
        {
            moveInput = value.Get<Vector2>();
        }
    }

    public void OnJump(InputValue value)
    {
        if (playerInput != null && isGrounded && value.isPressed)
        {
            Jump();
        }
    }

    public void OnDash(InputValue value)
    {
        if (playerInput != null && value.isPressed && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnSkill(InputValue value)
    {
        if (playerInput != null && value.isPressed && canUseSkill && characterData.skill != null)
        {
            StartCoroutine(UseSkill());
        }
    }

    private void Move()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        if (!isGrounded)
        {
            rb.AddForce(movement * (speed * 0.5f));
            return;
        }

        rb.AddForce(movement * speed);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector3 dashDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private IEnumerator UseSkill()
    {
        characterData.skill.ActivateSkill(this); // キャラごとのスキルを発動
        if (characterData.skill.skillSE != null)
        {
            gameManager.PlaySE(characterData.skill.skillSE, 0.5f);//効果音再生
        }
        canUseSkill = false;

        yield return new WaitForSeconds(characterData.skill.cooldownTime);

        canUseSkill = true;

        // パーティクルシステムのインスタンスを生成
        if (chargeEffect != null)
        {
            UnityEngine.GameObject effectInstance = Instantiate(chargeEffect, transform.position, Quaternion.identity);
            Destroy(effectInstance.gameObject, 1f);
        }
    }

    public void OnPause(InputValue value)
    {
        if (playerInput != null && value.isPressed)
        {
            gameManager.Pause();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
