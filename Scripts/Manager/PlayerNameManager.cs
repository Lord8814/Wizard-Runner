using UnityEngine;
using TMPro; // Si vous utilisez TextMeshPro pour l'input du nom

public class PlayerNameManager : MonoBehaviour
{
    public TMPro.TMP_InputField nameInputField; // Référence à l'InputField de TextMeshPro
    private string playerName;

    // Appelé lors de la soumission du nom
    public void SetPlayerName()
    {
        playerName = nameInputField.text;
        Debug.Log("Player name set to: " + playerName);

        // Vous pouvez ensuite sauvegarder ou envoyer ce nom à GameManager
        GameManager.Instance.SetPlayerName(playerName);
    }
}
