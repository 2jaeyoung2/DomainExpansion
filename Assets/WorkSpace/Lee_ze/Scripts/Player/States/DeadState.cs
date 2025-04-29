using System.Collections;
using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerControl player;

    public void EnterState(PlayerControl player)
    {
        this.player = player;

        this.player.isDead = true;

        Debug.Log("Dead state");

        this.player.playerCollider.enabled = false;

        this.player.playerAnim.SetTrigger("Dead");

        this.player.StartCoroutine(DropCoffin());
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {

    }

    private IEnumerator DropCoffin()
    {
        yield return new WaitForSeconds(2f);

        GameObject.Instantiate(player.coffin, CoffinPosition(), player.transform.rotation);
    }

    private Vector3 CoffinPosition()
    {
        return player.transform.position + Vector3.up * 15f;
    }
}
