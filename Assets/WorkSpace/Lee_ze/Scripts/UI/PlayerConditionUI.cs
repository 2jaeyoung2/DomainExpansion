using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerConditionUI : MonoBehaviourPun
{
    [SerializeField]
    private PlayerStatistics playerStats;

    [Header("Health Point")]
    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private TextMeshProUGUI hpText;

    [Header("Stamina")]
    [SerializeField]
    private Slider staminaBar;

    [SerializeField]
    private TextMeshProUGUI staminaText;

    private void Start()
    {
        playerStats.OnHealthChanged += UpdateHPbar;

        playerStats.OnStaminaChanged += UpdateStaminaBar;
    }

    public void SetMyPlayer(PlayerStatistics myPlayer)
    {
        playerStats = myPlayer;
    }

    private void UpdateHPbar(float currentHealth, float maxHealth)
    {
        if (hpBar != null)
        {
            hpBar.value = currentHealth / maxHealth;
        }

        if (hpText != null)
        {
            hpText.text = $"{(int)currentHealth} / {(int)maxHealth}";
        }
    }

    private void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina / maxStamina;
        }

        if (staminaText != null)
        {
            staminaText.text = $"{(int)currentStamina} / {(int)maxStamina}";
        }
    }

    private void OnDisable()
    {
        playerStats.OnHealthChanged -= UpdateHPbar;

        playerStats.OnStaminaChanged -= UpdateStaminaBar;
    }
}
