using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        Debug.Log("Dead state");

        this.player.playerCollider.enabled = false;

        this.player.playerAnim.SetTrigger("Down");

        GameObject.Instantiate(player.coffin, CoffinPosition(), player.transform.rotation);
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {

    }

    private Vector3 CoffinPosition()
    {
        return player.transform.position + Vector3.up * 30f;
    }
}
