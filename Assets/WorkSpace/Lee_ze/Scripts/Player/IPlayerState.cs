public interface IPlayerState
{
    public void EnterState(PlayerControl player);

    public void UpdateState();

    public void ExitState();
}