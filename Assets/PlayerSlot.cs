using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public Image characterImage; // ƒLƒƒƒ‰‰æ‘œ
    public TextMeshProUGUI characterName;   // ƒLƒƒƒ‰–¼
    public UnityEngine.GameObject readyIndicator; // €”õŠ®—¹‚Ì•\¦

    public void UpdateUI(CharacterData character, bool isReady)
    {
        characterImage.sprite = character.characterIcon;
        characterName.text = character.characterName;
        readyIndicator.SetActive(isReady);
    }
}
