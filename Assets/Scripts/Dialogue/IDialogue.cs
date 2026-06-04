namespace YourNewFriend
{
    public interface IDialogue
    {
        string Title { get; }
        string Id { get; }
        DialogueEntry[] Entries { get; }
    }
}