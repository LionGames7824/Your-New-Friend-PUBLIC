using UnityEngine;
using YourNewFriend;

/// <summary>
/// Data for a complete dialogue.
/// Implements IDialogue 
/// To create: right-click inside the Assets folder → Create → Dialogue
/// </summary>
namespace YourNewFriend
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject, IDialogue
    {
        [SerializeField] private string _id;
        [SerializeField] private string _title;
        [SerializeField] private DialogueEntry[] _entries;

        // --- IDialogue ---
        public string Id => _id;
        public string Title => _title;
        public DialogueEntry[] Entries => _entries;
    }
}