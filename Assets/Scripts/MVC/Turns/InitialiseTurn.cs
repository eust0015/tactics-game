using MVC.EventData;
using ScriptableObjects.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace MVC.Turns
{
    public class InitialiseTurn : MonoBehaviour
    {
        #region Fields
        [Header("Fields")]
        [SerializeField] private TurnManagerSO turnManager;

        #endregion
        #region Events
        [Header("Events")]
        [SerializeField] private UnityEvent<PlayerAndTransformEventData> onTurnInitialised;
        
        #endregion
        #region Properties
        public TurnManagerSO TurnManager { get => turnManager; private set => turnManager = value; }

        #endregion
        #region Event Properties

        public UnityEvent<PlayerAndTransformEventData> OnTurnInitialised { get => onTurnInitialised; private set => onTurnInitialised = value; }

        #endregion
        #region MonoBehaviour
        #endregion
        #region Event Handlers
        #endregion
        #region Methods

        public void Initialise(PlayerAndTransformEventData context)
        {
            TurnManager.StartNextTurn();
            OnTurnInitialised.Invoke(context);
        }
        #endregion
    }
}