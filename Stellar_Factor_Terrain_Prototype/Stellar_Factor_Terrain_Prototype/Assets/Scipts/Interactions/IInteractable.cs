namespace StellarFactor
{
    public interface IInteractable
    {
        public void PlayerEnterRange(PlayerControl playerControl);
        public void Interact();
        public void PlayerExitRange(PlayerControl playerControl);
    }
}