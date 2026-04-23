public interface ICounter
{
    void Interact(PlayerController player);
    void InteractAlternate(PlayerController player) { } // default = ไม่ทำอะไร
}