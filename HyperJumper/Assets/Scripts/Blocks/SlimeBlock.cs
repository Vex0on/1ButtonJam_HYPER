public class SlimeBlock : Block
{
    public override void OnEnter(PlayerController player)
    {
        player.canJump = true;
    }
}
