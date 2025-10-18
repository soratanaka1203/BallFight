using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectManager : MonoBehaviour
{
    public CharacterData[] characters;
    private int[] selectedCharacters = new int[4];
    private bool[] isReady = new bool[4];
    public PlayerSlot[] playerSlots; // 4人分のUIスロット
    public UnityEngine.GameObject startGameUI;  // ゲーム開始（UI）
    private bool isStart = false;

    [SerializeField] GameManager gameManager;

    void Start()
    {
        CharacterSelectionData.Instance.ResetSelection();
        UpdateAllUI();
        startGameUI.SetActive(false);  // 初期状態ではスタートボタンを非表示
    }

    public void MoveCursor(int playerIndex, int direction)
    {
        if (isReady[playerIndex]) return; // 準備完了したらキャラ変更不可

        selectedCharacters[playerIndex] = (selectedCharacters[playerIndex] + direction + characters.Length) % characters.Length;
        UpdateUI(playerIndex);
    }

    public void LockCharacter(int playerIndex, InputDevice device)
    {
        if (CharacterSelectionData.Instance.selectedCharacters[playerIndex] != null) return; // 既に確定済み

        CharacterData character = characters[selectedCharacters[playerIndex]];
        CharacterSelectionData.Instance.LockCharacter(playerIndex, character, device);
        
        isReady[playerIndex] = true;
        UpdateUI(playerIndex);
        CheckGameStartCondition();
    }

    public void UnlockCharacter(int playerIndex)
    {
        if (!isReady[playerIndex]) return;
        isReady[playerIndex] = false;
        CharacterSelectionData.Instance.ClearCharacter(playerIndex); //登録情報を削除
        UpdateUI(playerIndex);
        CheckGameStartCondition();  // キャラクターが解除された後にゲーム開始条件をチェック
    }


    private void UpdateUI(int playerIndex)
    {
        playerSlots[playerIndex].UpdateUI(characters[selectedCharacters[playerIndex]], isReady[playerIndex]);
    }

    private void UpdateAllUI()
    {
        for (int i = 0; i < 4; i++)
        {
            UpdateUI(i);
        }
    }

    private void CheckGameStartCondition()
    {
        int readyPlayersCount = 0;

        // 準備完了プレイヤーの数をカウント
        foreach (bool ready in isReady)
        {
            if (ready)
            {
                readyPlayersCount++;
            }
        }

        // 2人以上選ばれていればスタートボタンを表示
        startGameUI.SetActive(readyPlayersCount >= 2);
        if (readyPlayersCount >= 2) isStart = true;
    }

    // ゲーム開始ボタンが押されたときの処理
    public void OnStartGameButtonPressed()
    {
        if (isStart)  // ゲーム開始準備ができているさ場合のみ処理
        {
            isStart = false;
            gameManager.LoadScene("mainScene");
        }
    }
}
