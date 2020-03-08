using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using BaseSystems.EventSystem;
using BaseSystems.DataPersistance;
using BaseSystems.SceneHandling;

namespace GameplayLogic
{
    public class GameManager : Manager, IEventListener<GenericEvent>
    {
        public int CurrentLives { get; private set; }
        public int MaxLives { get; private set; }
        public int CurrentScore { get; private set; }
        public int HighScore { get; private set; }
        public bool IsPaused { get; private set; }

        public GameGlobalConfig GlobalConfig;

        private float _previousTimeScale;
        private bool _isPlaying;
        private float _playthroughTime;
        private DataPersistenceManager _dataPersistance;
        private SceneTransitionManager _transitionManager;
        private SpawnerManager _spawnerManager;
        private int _conditionCounter = 0;

        public override void Initialize()
        {
            base.Initialize();
            _previousTimeScale = Time.timeScale;

            // Request the providers required
            _dataPersistance = ManagerProvider.Get<DataPersistenceManager>();
            _transitionManager = ManagerProvider.Get<SceneTransitionManager>();
            _spawnerManager = ManagerProvider.Get<SpawnerManager>();

            HighScore = _dataPersistance.PlayerData.CurrentHighScore;
        }

        public void SetupLevel(int playerLives)
        {
            MaxLives = CurrentLives = playerLives;
            CurrentScore = 0;
            ScoreUpdateEvent.Trigger(CurrentScore);
            _conditionCounter = 0;
        }

        public void AddScore(int scoreToAdd)
        {
            CurrentScore += scoreToAdd;
            HighScore = Mathf.Max(CurrentScore, HighScore);
            ScoreUpdateEvent.Trigger(CurrentScore);
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
                        StartCoroutine(RespawnPlayer(player, 2f));
                    }
                    break;
                case GenericEventType.EnemyDestroyed:
                    GameObject enemyObj = eventType.Originator;
                    // If an enemy was destroyed then it should have a health component
                    Health enemyHealth = enemyObj.GetComponent<Health>();
                    AddScore(enemyHealth.ScoreOnDeath);
                    if(GlobalConfig.WinCondition == WinCondition.Score)
                    {
                        _conditionCounter = CurrentScore;
                        if(_conditionCounter >= GlobalConfig.AmountToWin)
                        {
                            GenericEvent.Trigger(GenericEventType.LevelCompleted, null);
                        }
                    }
                    break;
                case GenericEventType.WaveFinished:
                    if(GlobalConfig.WinCondition == WinCondition.WaveSurvived)
                    {
                        _conditionCounter++;
                        if (_conditionCounter >= GlobalConfig.AmountToWin)
                            GenericEvent.Trigger(GenericEventType.LevelCompleted, null);
                    }
                    break;
                case GenericEventType.LevelStarted:
                    _isPlaying = true;
                    break;
                case GenericEventType.LevelCompleted:
                case GenericEventType.LevelEnd:
                    // Store the high score in persistance
                    _dataPersistance.PlayerData.CurrentHighScore = Mathf.Max(
                        _dataPersistance.PlayerData.CurrentHighScore,
                        CurrentScore);
                    _dataPersistance.Save();
                    GameOverSceneModel model = new GameOverSceneModel()
                    {
                        CurrentScore = CurrentScore,
                        HighScore = HighScore,
                    };
                    model.OnAccept += () =>
                    {
                        GameplaySceneModel gameplayModel = new GameplaySceneModel();
                        _transitionManager.LoadScene(SceneIndex.GameplayScene, gameplayModel);
                    };

                    _transitionManager.AddScene(SceneIndex.WinScreen, model);
                    _isPlaying = false;
                    break;
                case GenericEventType.GameOver:
                    // When game over event is triggered
                    GameOverSceneModel gosmodel = new GameOverSceneModel()
                    {
                        CurrentScore = CurrentScore,
                        HighScore = HighScore,
                    };
                    gosmodel.OnAccept += () =>
                    {
                        GameplaySceneModel gameplayModel = new GameplaySceneModel();
                        _transitionManager.LoadScene(SceneIndex.GameplayScene, gameplayModel);
                    };

                    // Show the game over screen screen
                    _transitionManager.AddScene(SceneIndex.GameOverScreen, gosmodel);
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

            player.RespawnAt(_spawnerManager.GetSpawnerByID("Player"));
            var health = player.GetComponent<Health>();
            health.SetInvulnerable(true);
            StartCoroutine(health.SetBlink(2));
            yield return health.SetDamageEnabledInTime(2);

            GenericEvent.Trigger(GenericEventType.RespawnCompleted, player.gameObject);
        }
    }
}