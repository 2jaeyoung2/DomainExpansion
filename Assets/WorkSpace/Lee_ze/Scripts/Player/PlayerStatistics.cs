using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHealthChanged; // 플레이어 체력 체크

    public event Action<float, float> OnStaminaChanged; // 플레이어 스테미나 체크

    [SerializeField]
    private PlayerControl player;

    private float playerMaxHP;

    private float playerCurrentHP;

    private float playerMaxStamina;

    private float playerCurrentStamina;
    
    private int playerDownCount;

    #region 프로퍼티

    public float PlayerMaxHP
    {
        get
        {
            return playerMaxHP;
        }

        set
        {
            playerMaxHP = value;
        }
    }

    public float PlayerCurrentHP
    {
        get
        {
            return playerCurrentHP;
        }
        set
        {
            playerCurrentHP = value;
        }
    }

    public float PlayerMaxStamina
    {
        get
        {
            return playerMaxStamina;
        }
        set
        {
            playerMaxStamina = value;
        }
    }

    public float PlayerCurrentStamina
    {
        get
        {
            return playerCurrentStamina;
        }
        set
        {
            playerCurrentStamina = value;
        }
    }

    public int PlayerDownCount
    {
        get
        {
            return playerDownCount;
        }
        set
        {
            playerDownCount = value;
        }
    }

    #endregion

    private void Awake()
    {
        PlayerMaxHP = 200; // TODO: 200쯤으로 설정하면 될 듯.

        PlayerCurrentHP = PlayerMaxHP;

        PlayerMaxStamina = 100;

        PlayerCurrentStamina = PlayerMaxStamina;

        PlayerDownCount = 0;
    }

    private void Start()
    {
        UpdateHealthBar();

        UpdateStaminaBar();
    }

    public void GetDamage(int damage, int downCountStack)
    {
        player.isHit = true;

        PlayerCurrentHP -= damage; // 입은 데미지 만큼 체력 깎임

        if (PlayerCurrentHP <= 0)
        {
            PlayerCurrentHP = 0;

            player.ChangeStateTo(new DeadState());
        }

        UpdateHealthBar();

        PlayerDownCount += downCountStack; // 다운 스택 쌓임

        player.playerAnim.SetTrigger("Hit");
    }

    public void GetDashDamage(int damage)
    {
        PlayerCurrentHP -= damage;

        if (PlayerCurrentHP <= 0)
        {
            PlayerCurrentHP = 0;

            player.ChangeStateTo(new DeadState());
        }

        UpdateHealthBar();
    }

    public void EndHit() // 피격 애니메이션 끝 부분에 호출되는 이벤트 함수
    {
        player.isHit = false;
    }

    public void UseStamina(float usedStamina) // 스테미나 사용
    {
        PlayerCurrentStamina -= usedStamina;

        if (PlayerCurrentStamina <= 0)
        {
            PlayerCurrentStamina = 0;
        }

        UpdateStaminaBar();
    }

    public void UpdateHealthBar() // HP바 업데이트
    {
        OnHealthChanged?.Invoke(PlayerCurrentHP, PlayerMaxHP);
    }

    public void UpdateStaminaBar() // Stamina바 업데이트
    {
        OnStaminaChanged?.Invoke(PlayerCurrentStamina, PlayerMaxStamina);
    }
}
