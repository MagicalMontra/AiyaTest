public class PlayerHeadChangedSignal
{
    public Avatar head => _head;
    private Avatar _head;

    public PlayerHeadChangedSignal(Avatar head)
    {
        _head = head;
    }
}