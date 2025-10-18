using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject victoryText; // 勝利メッセージ表示用
    [SerializeField] private float victoryDuration = 3f; // 勝利メッセージの表示時間
    public Transform[] spawnPoints;
    public CharacterSelectionData selectionData;

    private PlayerInputManager playerInputManager;
    private int remainingPlayers = 0;
    [SerializeField] AudioSource audioSource;

    [SerializeField] Camera camera;

    private GameObject winnerPlayer;
    private bool isVictory = false;
    private List<GameObject> players = new List<GameObject>();  // プレイヤーのリスト

    private void Start()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        pauseUI.gameObject.SetActive(false);

        Time.timeScale = 1.0f;

        if (SceneManager.GetActiveScene().name == "mainScene")
        {
            if (playerInputManager == null)
            {
                return;
            }

            for (int i = 0; i < selectionData.selectedCharacters.Length; i++)
            {
                if (selectionData.selectedCharacters[i] != null)
                {
                    playerInputManager.playerPrefab = selectionData.selectedCharacters[i].characterPrefab;

                    // 保存されたデバイスを取得
                    InputDevice assignedDevice = selectionData.playerDevices[i];

                    // デバイスが設定されていない場合処理を飛ばす
                    if (assignedDevice == null)
                    {
                        Debug.Log("デバイスがありません");
                        continue;
                    }

                    // プレイヤーをデバイスと共に参加させる
                    GameObject player = playerInputManager.JoinPlayer(i, -1, null, assignedDevice).gameObject;

                    player.transform.position = spawnPoints[i].transform.position;
                    players.Add(player);
                    remainingPlayers++;
                }
            }
        }
    }


    private void Update()
    {
        if (isVictory)
        {
            FocusPlayer();
        }
    }

    // プレイヤーが死亡した時に呼び出す
    public void PlayerEliminated(GameObject eliminatedPlayer)
    {
        players.Remove(eliminatedPlayer);  // 排除されたプレイヤーをリストから削除
        remainingPlayers--;

        if (remainingPlayers == 1)
        {
            winnerPlayer = players[0];  // リストに残った唯一のプレイヤーが勝者
            StartCoroutine(HandleVictory());
        }
    }

    private IEnumerator HandleVictory()
    {
        victoryText.SetActive(true);  // 勝利メッセージを表示
        isVictory = true;

        yield return new WaitForSeconds(victoryDuration);  // 指定時間待つ

        // キャラクター選択画面に戻る
        LoadScene("CharaSelect");
    }

    // ポーズの処理
    public void Pause()
    {
        bool isPaused = Time.timeScale == 0;
        Time.timeScale = isPaused ? 1 : 0;
        pauseUI.gameObject.SetActive(!isPaused);
    }

    // シーンをロードする処理
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //効果音を鳴らす
    public void PlaySE(AudioClip audioClip, float volume)
    {
        if (audioClip == null)
        {
            Debug.LogError("PlaySE: AudioClip が null です！");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("PlaySE: AudioSource が設定されていません！");
            return;
        }

        audioSource.PlayOneShot(audioClip, volume);
    }

    private void FocusPlayer()
    {
        // プレイヤーの位置を元にカメラの目標位置を計算
        Vector3 desiredPosition = winnerPlayer.transform.position + new Vector3(3,3,3);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.125f);

        // スムーズにカメラを移動させる
        camera.transform.position = transform.position = smoothedPosition;

        // カメラをプレイヤーに向ける
        camera.transform.LookAt(winnerPlayer.transform);
    }


    // ゲーム終了処理
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
