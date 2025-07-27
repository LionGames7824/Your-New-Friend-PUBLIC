using System;

namespace YourNewFriend
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        public Action<bool> IsOpenInventory;
        public bool HasInventoryOpen { get; private set; }

        public void OpenInventory(bool isOpen)
        {
            IsOpenInventory?.Invoke(isOpen);
            HasInventoryOpen = isOpen;
        }
    }
}