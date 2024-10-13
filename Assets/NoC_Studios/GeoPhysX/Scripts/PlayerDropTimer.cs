using System;
using TMPro;
using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// The PlayerDropTimer class manages a countdown timer that can be started, stopped, and reset.
    /// </summary>
    /// <remarks>
    /// It is used to control the timing of player-related events in the game.
    /// </remarks>
    public class PlayerDropTimer : MonoBehaviour
    {
        /// <summary>
        /// UI element that displays the remaining time for the player drop timer.
        /// </summary>
        /// <remarks>
        /// This TextMeshProUGUI component is used to present the current countdown time
        /// visually to the player. It is updated regularly during the countdown process
        /// to reflect the remaining time accurately.
        /// </remarks>
        [SerializeField] TextMeshProUGUI timerText;

        /// <summary>
        /// The maximum time allowed for the player drop timer.
        /// </summary>
        /// <remarks>
        /// This float value represents the upper limit of the countdown timer. It is used to initialize or reset the timer
        /// and determines the total duration for the countdown sequence.
        /// </remarks>
        [SerializeField] float maxTime = 15f;

        /// <summary>
        /// The current time remaining on the timer.
        /// </summary>
        /// <remarks>
        /// This float value represents the amount of time left before the timer reaches zero. It is updated
        /// every frame while the timer is active and can be reset to a predefined maximum time.
        /// </remarks>
        float currentTime;

        /// <summary>
        /// Indicates whether the timer is currently active and counting down.
        /// </summary>
        /// <remarks>
        /// This boolean flag is set to true when the timer is running and false when it is stopped or inactive.
        /// </remarks>
        bool isTimerRunning = false;

        /// <summary>
        /// Event triggered when the countdown timer has reached zero.
        /// </summary>
        /// <remarks>
        /// This event is invoked once the `PlayerDropTimer` finishes counting down.
        /// Subscribers to this event can handle actions that need to occur when the timer expires,
        /// such as dropping a player character or initiating a fail condition.
        /// </remarks>
        public event Action OnTimeOut;

        /// <summary>
        /// Initializes the timer when the script starts.
        /// </summary>
        /// <remarks>
        /// This method sets the current time to the maximum time and updates the timer UI to display the initial state.
        /// </remarks>
        void Start()
        {
            currentTime = maxTime;
            UpdateTimerUI();
        }

        /// <summary>
        /// Updates the timer state every frame.
        /// </summary>
        /// <remarks>
        /// This method decreases the current time by the delta time if the timer is running and updates the timer UI. When the timer reaches zero, it stops the timer and invokes the `OnTimeOut` event.
        /// </remarks>
        void Update()
        {
            if (isTimerRunning)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerUI();

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    isTimerRunning = false;
                    OnTimeOut?.Invoke();
                }
            }
        }

        /// <summary>
        /// Updates the timer UI to reflect the current time.
        /// </summary>
        /// <remarks>
        /// This method updates the text and color of the timer UI based on the current remaining time.
        /// It sets the text to the ceiling value of `currentTime` and colors the timer text green, yellow, or red depending
        /// on whether the remaining time is above half, between half and quarter, or below quarter of the maximum time, respectively.
        /// </remarks>
        void UpdateTimerUI()
        {
            const int half = 2;
            const int quarter = 4;
            
            timerText.text = Mathf.Ceil(currentTime).ToString("00");

            if (currentTime > maxTime / half)
                timerText.color = Color.green;
            else if (currentTime > maxTime / quarter)
                timerText.color = Color.yellow;
            else
                timerText.color = Color.red;
        }

        /// <summary>
        /// Starts the timer to begin counting down.
        /// </summary>
        /// <remarks>
        /// This method sets the internal flag `isTimerRunning` to true, thereby starting the countdown process.
        /// It does not reset the elapsed time or initialize the timer's duration.
        /// </remarks>
        public void StartTimer()
        {
            isTimerRunning = true;
        }

        /// <summary>
        /// Stops the currently running timer.
        /// </summary>
        /// <remarks>
        /// This method sets the internal flag `isTimerRunning` to false, effectively pausing any further countdown.
        /// It does not reset the elapsed time or update the timer UI.
        /// </remarks>
        public void StopTimer()
        {
            isTimerRunning = false;
        }

        /// <summary>
        /// Resets the timer to the configured maximum time and updates the timer UI accordingly.
        /// </summary>
        /// <remarks>
        /// This method sets the current time to the `maxTime` value and refreshes the timer UI
        /// to reflect the reset state. It does not start or stop the timer.
        /// </remarks>
        public void ResetTimer()
        {
            currentTime = maxTime;
            UpdateTimerUI();
        }
    }
}
