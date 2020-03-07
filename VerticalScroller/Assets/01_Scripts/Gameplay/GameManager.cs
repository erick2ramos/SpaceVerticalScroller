using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using BaseSystems.EventSystem;
using BaseSystems.DataPersistance;

namespace GameplayLogic
{
    public class GameManager : Manager, IEventListener<GenericEvent>
    {
        public int CurrentLives { get; private set; }
        public int MaxLives { get; private set; }
        public int CurrentScore { get; private set; }
        public int HighScore { get; private set; }
        public bool IsPaused { get; private set; }

        private float _previousTimeScale;
        private bool _isPlaying;
        private float _playthroughTime;
        private DataPersistanceManager _dataPersistance;
        
        public override void Initialize()
        {
            base.Initialize();
            _previousTimeScale = Time.timeScale;
            // Request the data persistance from the provider
            _dataPersistance = ManagerProvider.Get<DataPersistanceManager>();
            HighScore = _dataPersistance.PlayerData.CurrentHighScore;
        }

        public void SetupLevel(int playerLives)
        {
            MaxLives = CurrentLives = playerLives;
            ScoreUpdateEvent.Trigger(CurrentScore);
        }

        public void AddScore(int scoreToAdd)
        {
            CurrentScore += scoreToAdd;
            ScoreUpdateEvent.Trigger(CurrentScore);
        }

        private void RespawnPlayer()
        {

        }

        private void Update()
        {
            if (_isPlaying)
            {
                _playthroughTime += Time.deltaTime;
            }
        }

        public void OnEvent(GenericEvent eventType)
        {
            switch (eventType.EventType)
            {
                // If pause event is triggered we make sure to toggle the time scale
                case GenericEventType.Pause:
                    if (!_isPlaying)
                        return;
                    IsPaused = !IsPaused;
                    Time.timeScale = IsPaused ? 0 : _previousTimeScale;
                    break;
                // If player died is triggered, lower the lives for current playthrough 
                // and check if it's game over
                case GenericEventType.PlayerDied:
                    CurrentLives--;
                    if (CurrentLives <= 0)
                    {
                        _isPlaying = false;
                        // Store the high score in persistance
                        _dataPersistance.PlayerData.CurrentHighScore = Mathf.Max(
                            _dataPersistance.PlayerData.CurrentHighScore,
                            CurrentScore);
                        _dataPersistance.Save();

                        // Broadcast the game over event
                        GenericEvent.Trigger(GenericEventType.GameOver, null);
                    } else
                    {
                        var player = eventType.Originator.GetComponent<Character>();
                        StartCoroutine(RespawnPlayer(player, 3));
                    }
                    break;
                case GenericEventType.EnemyDestroyed:
                    GameObject enemyObj = eventType.Originator;
                    // If an enemy was destroyed then it should have a health component
                    Health enemyHealth = enemyObj.GetComponent<Health>();
                    AddScore(enemyHealth.ScoreOnDeath);
                    break;
                case GenericEventType.LevelStarted:
                    _isPlaying = true;
                    break;
                case GenericEventType.LevelEnd:
                    _isPlaying = false;
                    break;
            }
        }

        private void OnEnable()
        {
            this.EventStartListening<GenericEvent>();
        }

        private void OnDisable()
        {
            this.EventStopListening<GenericEvent>();
        }

        IEnumerator RespawnPlayer(Character player, float time)
        {
            GenericEvent.Trigger(GenericEventType.RespawnStarted, player.gameObject);

            yield return new WaitForSeconds(time);

            player.RespawnAt(player.transform.position);
            var health = player.GetComponent<Health>();
            health.SetDamageable(true);
            yield return health.SetDamageEnabledInTime(2);

            GenericEvent.Trigger(GenericEventType.RespawnCompleted, player.gameObject);
        }
    }
}