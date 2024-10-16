using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Controls the behavior of the player character within the game environment.
    /// </summary>
    /// <remarks>
    /// Handles the player's input and movement, interacting with game objects and physics.
    /// Inherits from UnityEngine.MonoBehaviour to integrate seamlessly with the Unity game engine.
    /// </remarks>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// A constant floating-point value representing zero, used as an initial or neutral value
        /// in the PlayerController class.
        /// </summary>
        const float Nilf = 0f;

        /// <summary>
        /// The speed at which the player can move, measured in units per second.
        /// This value is serialized so it can be adjusted in the Unity Editor.
        /// </summary>
        [SerializeField] float m_moveSpeed = 30f;

        /// <summary>
        /// A serialized transform component used as the location where new game pieces are spawned
        /// within the PlayerController class.
        /// </summary>
        [SerializeField] Transform m_gamePieceSpawnPoint;

        /// <summary>
        /// A serialized floating-point value representing the time interval between the player's shots in seconds.
        /// This variable is used to control the rate of fire in the PlayerController class.
        /// </summary>
        [SerializeField] float m_shotTimer = 1f;

        /// <summary>
        /// The Rigidbody component used to apply physics-based interactions and movements to the player character within
        /// the game environment.
        /// </summary>
        Rigidbody m_rigidBody;

        /// <summary>
        /// An instance of the GameBoard class responsible for managing the game environment,
        /// including the current pieces in play and the next piece to spawn.
        /// </summary>
        GameBoard m_gameBoard;

        /// <summary>
        /// Tracks the time elapsed since the player last fired a shot.
        /// </summary>
        float m_timeSinceLastShot = Nilf;

        /// <summary>
        /// Instance of PlayerDropTimer responsible for managing the drop timer functionality
        /// associated with the player's actions in the game.
        /// </summary>
        PlayerDropTimer m_dropTimer;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        /// <remarks>
        /// Suitable for performing initialization tasks, this method ensures that essential components like Rigidbody and
        /// PlayerDropTimer are retrieved and set up before the game starts.
        /// </remarks>
        void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody>();
            m_dropTimer = GetComponent<PlayerDropTimer>();
        }

        /// <summary>
        /// Called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        /// <remarks>
        /// This method is responsible for initializing the GameBoard component and setting the timer for the player's shot mechanism.
        /// It searches for the GameBoard object in the scene and assigns its reference to the m_gameBoard variable.
        /// The m_timeSinceLastShot variable is initialized with the value of m_shotTimer to track shooting intervals.
        /// </remarks>
        void Start()
        {
            m_gameBoard = GameObject.Find(nameof(GameBoard)).GetComponent<GameBoard>();
            m_timeSinceLastShot = m_shotTimer;
            m_dropTimer.OnTimeOut += OnDropTimerTimeout;
            m_dropTimer.StartTimer();
        }

        /// <summary>
        /// Handles the event triggered when the drop timer reaches zero.
        /// </summary>
        /// <remarks>
        /// This method is invoked by the PlayerDropTimer component.
        /// It calls the DropNextShape method to drop the next shape onto the game board.
        /// </remarks>
        void OnDropTimerTimeout()
        {
            DropNextShape();
        }

        /// <summary>
        /// Updates the state of the PlayerController once per frame.
        /// </summary>
        /// <remarks>
        /// This method handles tracking the elapsed time since the player last fired a shot, checks for player input
        /// (specifically the space key), and triggers the firing action if the conditions are met.
        /// If the player presses the space key and the specified time interval has passed, this method will spawn the next game piece
        /// and reset the shot timer.
        /// </remarks>
        void Update()
        {
            if (m_gameBoard.IsGameOver)
            {
                m_dropTimer.StopTimer();
                return;
            }

            m_timeSinceLastShot += Time.deltaTime;
            
            if (!Input.GetKeyDown(KeyCode.Space) || !(m_timeSinceLastShot >= m_shotTimer)) return;
            
            DropNextShape();
            m_timeSinceLastShot = Nilf;
        }

        /// <summary>
        /// Called at a fixed interval, independent of the frame rate.
        /// </summary>
        /// <remarks>
        /// This method is used to apply consistent physics-related updates.
        /// It checks for player input and applies force to the Rigidbody component
        /// to move the player character left or right, depending on the input.
        /// </remarks>
        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A))
            {
                m_rigidBody.AddForce(Vector3.left * (m_moveSpeed * Time.fixedDeltaTime));
            }

            if (Input.GetKey(KeyCode.D))
            {
                m_rigidBody.AddForce(Vector3.right * (m_moveSpeed * Time.fixedDeltaTime));
            }
        }

        /// <summary>
        /// Drops the next shape onto the game board at the configured spawn point.
        /// </summary>
        /// <remarks>
        /// This method is triggered by player input and spawns the next game piece
        /// on the board using the defined spawn position. It relies on the GameBoard
        /// class to handle the actual instantiation of the game piece.
        /// </remarks>
        void DropNextShape()
        {
            m_gameBoard.SpawnNextGamePiece(m_gamePieceSpawnPoint.position);
            m_dropTimer.ResetTimer();
            m_dropTimer.StartTimer();
        }
    }
}