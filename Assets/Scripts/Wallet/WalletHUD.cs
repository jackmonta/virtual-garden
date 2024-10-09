using UnityEngine;
using TMPro;

public class WalletHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI walletText;

    void Update()
    {
        int? walletAmount = GetWalletAmount();
        
        if (walletAmount.HasValue == true)
            walletText.text = walletAmount.Value.ToString();
        else
            walletText.text = "?";
    }

    int? GetWalletAmount()
    {
        if (Wallet.Instance == null) return null;

        return Wallet.Instance.Money;
    }
}
