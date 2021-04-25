using System.Collections.Generic;
using nanoid;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class EnemyController
{
    [Inject] private SignalBus _signalBus;
    [Inject] private EnemySettings _settings;
    private List<Avatar> _avatars = new List<Avatar>();
    private List<AvatarPlaceHolder> _avatarPlaceHolders = new List<AvatarPlaceHolder>();

    public void SpawnEnemy(EnemySpawnSignal signal)
    {
        var placeHolder = Object.Instantiate(_settings.avatarPlaceHolder, signal.position, Quaternion.identity);
        var instanceId = NanoId.Generate(8);
        var tag = new AvatarTag("Enemy");
        var newAvatar = new Avatar(instanceId, placeHolder.Init(instanceId, signal.data.sprite, tag), signal.data);
        placeHolder.gameObject.name += "_enemy_" + instanceId;
        _avatars.Add(newAvatar);
        _avatarPlaceHolders.Add(placeHolder);
    }
    public void OnEncounter(EncounterSignal signal)
    {
        var index = _avatars.FindIndex(avatar => avatar.instanceId == signal.encountedId);
        
        if (index < 0)
            return;
        
        _signalBus.Fire(new CombatStartSignal(_avatars[index]));
    }

    public void OnEnemyKilled(EnemyKilledSignal signal)
    {
        var index = _avatars.FindIndex(avatar => avatar.instanceId == signal.enemyKilled.instanceId);
        
        if (index < 0)
            return;
        
        Object.Destroy(_avatarPlaceHolders[index].gameObject);
        _avatarPlaceHolders.RemoveAt(index);
        _avatars.RemoveAt(index);
    }
}

public class EnemyKilledSignal
{
    public Avatar enemyKilled => _enemyKilled;
    private Avatar _enemyKilled;

    public EnemyKilledSignal(Avatar enemyKilled)
    {
        _enemyKilled = enemyKilled;
    }
}