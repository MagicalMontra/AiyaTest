using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GetAllyTest : MonoBehaviour
{
    [SerializeField] private CharacterData[] _pool;
    [Inject] private SignalBus _signalBus;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            var random = Random.Range(0, _pool.Length);
            // _signalBus.Fire(new HeroAllyAcquireSignal(_pool[random]));
        }
    }
}
