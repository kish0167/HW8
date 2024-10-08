using System;
using System.Collections;
using Arkanoid.Utility;
using UnityEngine;

namespace Arkanoid.Services
{
    public class GameService : SingletonMonoBehaviour<GameService>
    {
        #region Variables

        [Header("Auto Play")]
        [SerializeField] private bool _isAutoPlay;

        [Header("Settings")]
        [SerializeField] private int _maxLives = 3;

        [Header("Stats")]
        [SerializeField] private int _score;
        [SerializeField] private int _lives;

        [SerializeField] private AudioClip _gameOverSfx;

        #endregion

        #region Events

        public event Action OnGameOver;
        public event Action OnLivesChanged;

        public event Action<int> OnScoreChanged;

        #endregion

        #region Properties

        public bool IsAutoPlay => _isAutoPlay;

        public bool IsGameOver { get; set; }

        public int Lives => _lives;
        public int Score => _score;

        #endregion

        #region Unity lifecycle

        protected override void Awake()
        {
            base.Awake();

            ResetLives();
            IsGameOver = false;
        }

        private void Start()
        {
            LevelService.Instance.OnAllBlocksDestroyed += AllBlocksDestroyedCallback;
        }

        private void OnDestroy()
        {
            LevelService.Instance.OnAllBlocksDestroyed -= AllBlocksDestroyedCallback;
        }

        #endregion

        #region Public methods

        public void AddScore(int value)
        {
            if (IsGameOver)
            {
                return;
            }

            _score += value;
            OnScoreChanged?.Invoke(_score);
        }

        public void ChangeLife(int value)
        {
            if (IsGameOver)
            {
                return;
            }

            if (_lives + value < 0)
            {
                _lives = 0;
            }
            else
            {
                _lives += value;
            }

            OnLivesChanged?.Invoke();
            GameOverCheck();
        }

        public void ResetLives()
        {
            _lives = _maxLives;
            OnLivesChanged?.Invoke();
        }

        #endregion

        #region Private methods

        private void AllBlocksDestroyedCallback()
        {
            if (SceneLoaderService.Instance.HasNextLevel()) // TODO: This is not fine
            {
                SceneLoaderService.Instance.LoadNextLevelDelayed();
            }
            else
            {
                SceneLoaderService.Instance.LoadWinScene();
            }
        }

        private void GameOverCheck()
        {
            if (_lives > 0)
            {
                return;
            }
            
            GameOver();
        }

        private void GameOver()
        {
            IsGameOver = true;
            Instance.ResetScore();
            AudioService.Instance.PlaySfx(_gameOverSfx);
            OnGameOver?.Invoke();
        }

        public void GameRestart()
        {
            IsGameOver = false;
            ResetLives();
            SceneLoaderService.Instance.LoadMenuScene();
        }
        
        private void ResetScore()
        {
            _score = 0;
        }

        #endregion
    }
}