using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterSelector : MonoBehaviour
{
    public int playerIndex;
    public CharacterSelectManager selectManager;

    [SerializeField] private GameManager gameManager;

    private PlayerInput playerInput;
    private Vector2 moveInput;

    private void Start()
    {
        if (selectManager == null) selectManager = FindObjectOfType<CharacterSelectManager>();
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex; // 自動でプレイヤー番号を取得
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        float threshold = 0.99f;  // スティックが0.99以上の入力で切り替え
        if (Mathf.Abs(moveInput.x) > threshold)
        {
            if (moveInput.x > 0)
                selectManager.MoveCursor(playerIndex, 1);
            else if (moveInput.x < 0)
                selectManager.MoveCursor(playerIndex, -1);
        }
    }


    public void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            selectManager.LockCharacter(playerIndex, playerInput.devices[0]);
        }
    }

    public void OnCancel(InputValue value) // Bボタンでキャンセル
    {
        if (value.isPressed)
        {
            selectManager.UnlockCharacter(playerIndex);
        }
    }

    public void OnStart(InputValue value)
    {
        if (value.isPressed)
        {
            selectManager.OnStartGameButtonPressed();
        }
    }

    public void OnPause(InputValue value)
    {
        if (value.isPressed)gameManager.Pause();
    }
}
