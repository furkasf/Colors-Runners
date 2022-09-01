using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Self Variables
    
        #region Public Variables
    
        public GameStates States;
    
        #endregion Public Variables

        [SerializeField] private GameObject Fog;
        
        #endregion Self Variables
    
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    
        private void OnEnable()
        {
            SubscribeEvents();
            Fog.SetActive(true);
        }
    
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onGetGameState += OnGetGameState;
    
            PlayerSignals.Instance.onPlayerEnterIdleArea += OnPlayerEnterIdleArea;
        }
    
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onGetGameState -= OnGetGameState;
    
            PlayerSignals.Instance.onPlayerEnterIdleArea -= OnPlayerEnterIdleArea;
        }
    
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    
        private void OnChangeGameState(GameStates newState)
        {
            States = newState;
            ControlFog(newState);
        }
    
        private void ControlFog(GameStates newState) => Fog.SetActive(newState != GameStates.Idle);


        private void OnPlayerEnterIdleArea() => OnChangeGameState(GameStates.Idle);
    
        private GameStates OnGetGameState() => States;
    
        private void OnSaveGame(SaveRunnerGameDataParams saveDataParams)
        {
            if (saveDataParams.Level != null)
            {
                ES3.Save("Level", saveDataParams.Level);
            }
        }
        private void OnReset()
        {
            Fog.SetActive(true);
            OnChangeGameState(GameStates.Runner);
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Runner);
        }
    }
}