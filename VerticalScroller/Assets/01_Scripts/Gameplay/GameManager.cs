﻿using UnityEngine;
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
        public bool IsPaused { get; private set; }

        private float _previousTimeScale;
        private bool _isPlaying;
        private float _playthroughTime;
        private DataPersistanceManager _dataPersistance;

        // Player ref
        // Enemies ref

        public override void Initialize()
        {
            base.Initialize();
            _previousTimeScale = Time.timeScale;
            _dataPersistance = ManagerProvider.Get<DataPersistanceManager>();
        }

        public void SetupLevel(int playerLives)
        {
            MaxLives = CurrentLives = playerLives;
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
                    }
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
    }
}