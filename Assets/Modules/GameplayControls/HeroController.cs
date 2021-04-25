using System;
using System.Collections.Generic;
using nanoid;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class HeroController
{
    [Inject] private SignalBus _signalBus;
    [Inject] private HeroSettings _settings;
    
    private List<Avatar> _avatars = new List<Avatar>();
    private List<AvatarPlaceHolder> _avatarPlaceHolders = new List<AvatarPlaceHolder>();
    
    public void SpawnHero(HeroSpawnSignal signal)
    {
        var placeHolder = Object.Instantiate(_settings.avatarPlaceHolder, signal.position, Quaternion.identity);
        var instanceId = NanoId.Generate(8);
        var tag = new AvatarTag("FreeAgent");
        var newAvatar = new Avatar(instanceId, placeHolder.Init(instanceId, signal.data.sprite, tag), signal.data);
        placeHolder.gameObject.name += "_freeAgent";
        _avatars.Add(newAvatar);
        _avatarPlaceHolders.Add(placeHolder);
    }
    public void OnEncounter(EncounterSignal signal)
    {
        var index = _avatars.FindIndex(avatar => avatar.instanceId == signal.encountedId);
        
        if (index < 0)
            return;
        
        _signalBus.Fire(new HeroAllyAcquireSignal(_avatars[index], _avatarPlaceHolders[index]));
        _avatars.RemoveAt(index);
        _avatarPlaceHolders.RemoveAt(index);
    }
}

[Serializable]
public class HeroSettings
{
    public AvatarPlaceHolder avatarPlaceHolder;
}