using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseSystems.EventSystem;
using BaseSystems.Managers;

namespace GameplayLogic
{
    [System.Serializable]
    public class Wave
    {
        public string[] SpawnIds;
        public float[] Delays;
        public EnemyType[] EnemyToSpawn;
        public float WaveDelay;
    }

    public class WaveSequence : MonoBehaviour
    {
        enum WaveSequenceState
        {
            TransitionWave,
            ProcessWave,
            Count
        }

        [SerializeField]
        private WaveSequenceConfig _wavesConfig;
        [SerializeField]
        private bool _cycle;
        private int _currentWave;
        private int _currentWaveSpawn;
        private bool _active;
        private float[] _timer;

        // Simple static state machine
        delegate void WaveSequenceStateDelegate();
        WaveSequenceStateDelegate[] _states;
        WaveSequenceState _currentState;
        WaveSequenceState _nextState;

        SpawnerManager _spawnerManager;

        private void Awake()
        {
            _spawnerManager = ManagerProvider.Get<SpawnerManager>();
        }

        public void Initialize()
        {
            _active = false;
            _currentWave = 0;
            _currentState = WaveSequenceState.ProcessWave;
            _nextState = WaveSequenceState.ProcessWave;

            // Initializing static methods state machine
            _states = new WaveSequenceStateDelegate[(int)WaveSequenceState.Count];
            _states[(int)WaveSequenceState.ProcessWave] = ProcessWave;
            _states[(int)WaveSequenceState.TransitionWave] = Transition;

            // 0 - wave delay timer
            // 1 - wave spawn delay
            _timer = new float[2];
        }

        public void Activate()
        {
            _active = true;
        }

        public void Deactivate()
        {
            _active = false;
        }

        private void Update()
        {
            if (!_active)
                return;

            if(_nextState == _currentState)
            {
                _states[(int)_currentState]();
            } else
            {
                _currentState = _nextState;
            }
        }

        private void ProcessWave()
        {
            var currentWave = _wavesConfig.Waves[_currentWave];
            if (_timer[0] <= currentWave.WaveDelay)
            {
                _timer[0] += Time.deltaTime;
                return;
            }

            if (_timer[1] <= currentWave.Delays[_currentWaveSpawn])
            {
                _timer[1] += Time.deltaTime;
                return;
            }

            // Spawn current wave spawn enemy in spawn id
            _spawnerManager.SpawnEnemy(currentWave.SpawnIds[_currentWaveSpawn], currentWave.EnemyToSpawn[_currentWaveSpawn]);

            // Reset timer for next spawn
            _timer[1] = 0;
            _currentWaveSpawn++;

            if (_currentWaveSpawn >= currentWave.EnemyToSpawn.Length)
            {
                _nextState = WaveSequenceState.TransitionWave;
            }
        }

        private void Transition()
        {
            // Transition wave
            _currentWave++;
            // Reset timers for next wave
            _timer[0] = _timer[1] = 0;
            _currentWaveSpawn = 0;
            // Cycle option
            if (_cycle)
            {
                _currentWave %= _wavesConfig.Waves.Length;
                _nextState = WaveSequenceState.ProcessWave;
            }
            else
            {
                _active = false;
                // Trigger level completed
                GenericEvent.Trigger(GenericEventType.LevelCompleted, null);
            }
        }
    }
}