using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHealthChanged; // �÷��̾� ü�� üũ

    public event Action<float, float> OnStaminaChanged; // �÷��̾� ���׹̳� üũ

    [SerializeField]
    private PlayerControl player;

    private float playerMaxHP;

    private float playerCurrentHP;

    private float playerMaxStamina;

    private float playerCurrentStamina;
    
    private int playerDownCount;

    #region ������Ƽ

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
        PlayerMaxHP = 200; // TODO: 200������ �����ϸ� �� ��.

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

        PlayerCurrentHP -= damage; // ���� ������ ��ŭ ü�� ����

        if (PlayerCurrentHP <= 0)
        {
            PlayerCurrentHP = 0;

            player.ChangeStateTo(new DeadState());
        }

        UpdateHealthBar();

        PlayerDownCount += downCountStack; // �ٿ� ���� ����

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

    public void EndHit() // �ǰ� �ִϸ��̼� �� �κп� ȣ��Ǵ� �̺�Ʈ �Լ�
    {
        player.isHit = false;
    }

    public void UseStamina(float usedStamina) // ���׹̳� ���
    {
        PlayerCurrentStamina -= usedStamina;

        if (PlayerCurrentStamina <= 0)
        {
            PlayerCurrentStamina = 0;
        }

        UpdateStaminaBar();
    }

    public void UpdateHealthBar() // HP�� ������Ʈ
    {
        OnHealthChanged?.Invoke(PlayerCurrentHP, PlayerMaxHP);
    }

    public void UpdateStaminaBar() // Stamina�� ������Ʈ
    {
        OnStaminaChanged?.Invoke(PlayerCurrentStamina, PlayerMaxStamina);
    }
}
