namespace StellarFactor
{
    public interface IInteractable
    {
        public void PlayerEnterRange(PlayerControl player);
        public void Interact();
        public void PlayerExitRange(PlayerControl player);
    }
}