using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Represents a game board within the NoC Studios GeoPhysX namespace.
    /// </summary>
    /// <remarks>
    /// The GameBoard class provides the structure and behavior associated with a game environment.
    /// Inherits from UnityEngine.MonoBehaviour.
    /// </remarks>
    public class GameBoard : MonoBehaviour
    {
        /// <summary>
        /// Handles the game's user interface components and updates based on game activity.
        /// </summary>
        [SerializeField] GameUIManager m_gameUIManager;

        /// <summary>
        /// Contains a list of GamePiece objects that are currently available for play on the GameBoard.
        /// </summary>
        [SerializeField] List<GamePiece> m_playablePieces;

        /// <summary>
        /// Constant used to increment game piece count.
        /// </summary>
        const int k_addOne = 1;

        /// <summary>
        /// Constant used to decrement game piece count.
        /// </summary>
        const int k_minusOne = -1;

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

        void Start()
        {
            m_boxCollider = GetComponent<BoxCollider>();
            if (m_boxCollider == null)
            {
                Debug.LogError("BoxCollider component missing from GameBoard.");
                enabled = false;
                return;
            }

            m_gamePiecesInPlay = new HashSet<GamePiece>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("GamePiece")) return;
            RegisterGamePiece(other.gameObject.GetComponent<GamePiece>());
        }

        void RegisterGamePiece(GamePiece piece)
        {
            if (!m_gamePiecesInPlay.Add(piece)) return;
            m_gameUIManager.UpdateGamePieceCounter(piece.Shape);
        }

        void UnregisterGamePiece(GamePiece piece)
        {
            if (!m_gamePiecesInPlay.Remove(piece)) return;
            m_gameUIManager.UpdateGamePieceCounter(piece.Shape);
        }
    }
}