using System;

namespace YourNewFriend
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        public Action<bool> IsOpenInventory;
        public Action<bool> IsOpenSettings;
        public bool HasInventoryOpen { get; private set; }
        public bool HasSettingsOpen { get; private set; }

        public void OpenInventory(bool isOpen)
        {
            IsOpenInventory?.Invoke(isOpen);
            HasInventoryOpen = isOpen;
        }

        public void OpenSettings(bool isOpen)
        {
            IsOpenSettings?.Invoke(isOpen);
            HasSettingsOpen = isOpen;
        }
    }
}