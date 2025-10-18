using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CharacterSelectionData", menuName = "Game/CharacterSelectionData")]
public class CharacterSelectionData : ScriptableObject
{
    private static CharacterSelectionData _instance;

    public CharacterData[] selectedCharacters = new CharacterData[4];
    public InputDevice[] playerDevices = new InputDevice[4];

    public static CharacterSelectionData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CharacterSelectionData>("CharacterSelectionData");

                if (_instance == null)
                {
                    Debug.LogError("CharacterSelectionData.asset が Resources フォルダ内にありません！");
                }
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        if (selectedCharacters == null || selectedCharacters.Length != 4)
        {
            selectedCharacters = new CharacterData[4]; // 初期化
            playerDevices = new InputDevice[4];
        }
    }

    public void ResetSelection()
    {
        for (int i = 0; i < selectedCharacters.Length; i++)
        {
            selectedCharacters[i] = null; // 未選択状態
            playerDevices[i] = null;
        }
    }

    public void AssignDeviceToPlayer(int playerIndex, InputDevice device)
    {
            playerDevices[playerIndex] = device;
    }

    public void LockCharacter(int playerIndex, CharacterData character, InputDevice device)
    {
        selectedCharacters[playerIndex] = character;
        AssignDeviceToPlayer(playerIndex, device);
    }



    public void ClearCharacter(int playerIndex)
    {
        selectedCharacters[playerIndex] = null;
        playerDevices[playerIndex] = null;
    }
}
