public class HeroAllyAcquireSignal
{
    public Avatar avatar => _avatar;
    public AvatarPlaceHolder placeholder => _placeholder;
    private Avatar _avatar;
    private AvatarPlaceHolder _placeholder;

    public HeroAllyAcquireSignal(Avatar avatar, AvatarPlaceHolder placeholder)
    {
        _avatar = avatar;
        _placeholder = placeholder;
    }
}