using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Provides the necessary structure and behavior for managing game pieces on the game board.
    /// </summary>
    /// <remarks>
    /// The GameBoard class includes properties and methods for handling the game pieces currently in play,
    /// counting game pieces by their shapes, and managing the lifecycle of game pieces.
    /// It requires a BoxCollider component.
    /// </remarks>
    [RequireComponent(typeof(BoxCollider))]
    public class GameBoard : MonoBehaviour
    {
        /// <summary>
        /// Handles the game's user interface components and updates based on game activity.
        /// </summary>
        [SerializeField] GameUIManager m_gameUIManager;

        /// <summary>
        /// Contains a list of GamePiece objects that are currently available for play on the GameBoard.
        /// </summary>
        [SerializeField] List<GameObject> m_playablePieces;

        /// <summary>
        /// Holds a reference to the next game piece that will be spawned on the game board.
        /// </summary>
        GameObject m_nextGamePiece;

        /// <summary>
        /// Represents the BoxCollider component attached to the GameBoard, used to detect and handle collisions within the game environment.
        /// </summary>
        BoxCollider m_boxCollider;

        /// <summary>
        /// Maintains a list of game pieces currently active on the game board.
        /// </summary>
        HashSet<GamePiece> m_gamePiecesInPlay;

        /// <summary>
        /// Gets the number of game pieces currently in play on the GameBoard.
        /// </summary>
        public int PiecesInPlay => m_gamePiecesInPlay.Count;

        /// <summary>
        /// Denotes the current count of cubes that are active within the game environment.
        /// </summary>
        public int CubesInPlay => m_gamePiecesInPlay.Count(gamePiece => gamePiece.Shape == GamePiece.PieceShape.Cube);

        /// <summary>
        /// Represents the number of cylinders currently active on the GameBoard.
        /// </summary>
        public int CylindersInPlay => m_gamePiecesInPlay.Count(gamePiece => gamePiece.Shape == GamePiece.PieceShape.Cylinder);

        /// <summary>
        /// Indicates the presence of the capsules in play within the game environment.
        /// </summary>
        public int CapsulesInPlay => m_gamePiecesInPlay.Count(gamePiece => gamePiece.Shape == GamePiece.PieceShape.Capsule);

        /// <summary>
        /// Tracks the current number of active spheres within the game environment.
        /// </summary>
        public int SpheresInPlay => m_gamePiecesInPlay.Count(gamePiece => gamePiece.Shape == GamePiece.PieceShape.Sphere);

        /// <summary>
        /// Gets the next game piece that will be spawned on the game board.
        /// </summary>
        public GameObject NextGamePiece => m_nextGamePiece;

        /// <summary>
        /// Initializes the GameBoard by setting up the required BoxCollider
        /// and preparing the initial next game piece to be spawned.
        /// </summary>
        void Start()
        {
            m_boxCollider = GetComponent<BoxCollider>();
            m_nextGamePiece = GetNextGamePiece();
            m_gamePiecesInPlay = new HashSet<GamePiece>();
        }

        /// <summary>
        /// Spawns the next game piece at the specified position on the game board.
        /// The spawned game piece uses the transform rotation of the next game piece.
        /// The next game piece is then updated to a new randomly selected piece.
        /// </summary>
        /// <param name="spawnPosition">The position at which the next game piece will be spawned.</param>
        public void SpawnNextGamePiece(Vector3 spawnPosition)
        {
            var gamePiece = Instantiate(m_nextGamePiece, spawnPosition, m_nextGamePiece.transform.rotation);
            m_nextGamePiece = GetNextGamePiece();
        }

        /// <summary>
        /// Removes the specified game piece from the game board.
        /// </summary>
        /// <param name="gamePiece">The game piece to be removed.</param>
        public void RemoveGamePiece(GamePiece gamePiece)
        {
            UnregisterGamePiece(gamePiece);
            DestroyGamePiece(gamePiece);
        }

        /// <summary>
        /// Destroys the specified game piece, removing it from the game board and performing related cleanup.
        /// </summary>
        /// <param name="gamePiece">The game piece to be destroyed.</param>
        void DestroyGamePiece(GamePiece gamePiece)
        {
            //TODO: Implement pooling
            //TODO: Add in vfx when destroying a piece
            //TODO: Add in sfx when destroying a piece 
            Destroy(gamePiece.gameObject);
        }

        /// <summary>
        /// Retrieves the next game piece to be spawned, updates the UI to reflect this next piece,
        /// and returns the game piece object.
        /// </summary>
        /// <returns>The GameObject representing the next game piece to be spawned.</returns>
        GameObject GetNextGamePiece()
        {
            var gamePiece = GetRandomGamePiece();
            m_gameUIManager.UpdateNextShape(gamePiece.GetComponent<GamePiece>().Shape);
            return gamePiece;
        }

        /// <summary>
        /// Selects a random game piece from the list of playable pieces.
        /// </summary>
        /// <returns>
        /// A GameObject representing a randomly selected game piece.
        /// </returns>
        GameObject GetRandomGamePiece()
        {
            var pieceIndex = Random.Range(0, m_playablePieces.Count);
            return m_playablePieces[pieceIndex];
        }

        /// <summary>
        /// Called when another collider enters the trigger collider attached to this GameObject.
        /// Registers any game piece if it has the tag specified by the global constant.
        /// </summary>
        /// <param name="other">The collider that enters the trigger.</param>
        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Globals.k_gamePieceTag)) return;
            RegisterGamePiece(other.gameObject.GetComponent<GamePiece>());
        }

        /// <summary>
        /// Registers a game piece to the game board, adds it to the
        /// collection of game pieces currently in play, and updates the
        /// game UI counter for the specific game piece shape.
        /// </summary>
        /// <param name="piece">The game piece to be registered.</param>
        void RegisterGamePiece(GamePiece piece)
        {
            if (!m_gamePiecesInPlay.Add(piece)) return;
            m_gameUIManager.UpdateGamePieceCounter(piece.Shape);
        }

        /// <summary>
        /// Unregisters a game piece from the game board.
        /// Updates the game piece counter in the GameUIManager.
        /// </summary>
        /// <param name="piece">The game piece to unregister from the game board.</param>
        void UnregisterGamePiece(GamePiece piece)
        {
            if (!m_gamePiecesInPlay.Remove(piece)) return;
            m_gameUIManager.UpdateGamePieceCounter(piece.Shape);
        }
    }
}