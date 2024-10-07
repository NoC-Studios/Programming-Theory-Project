using System;
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
        /// Represents the number of game pieces currently in play on the GameBoard.
        /// </summary>
        [SerializeField] private uint m_piecesInPlay;

        /// <summary>
        /// Denotes the current count of cubes that are active within the game environment.
        /// </summary>
        [SerializeField] private uint m_cubesInPlay;

        /// <summary>
        /// Represents the number of cylinders currently active on the GameBoard.
        /// </summary>
        [SerializeField] private uint m_cylindersInPlay;

        /// <summary>
        /// Indicates the presence of the capsule in play within the game environment.
        /// </summary>
        [SerializeField] private uint m_capsuleInPlay;

        /// <summary>
        /// Tracks the current number of active spheres within the game environment.
        /// </summary>
        [SerializeField] private uint m_spheresInPlay;

        /// <summary>
        /// Handles the event when a game piece enters the trigger collider attached to the game board.
        /// </summary>
        /// <param name="other">The Collider of the object that triggered the enter event.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Globals.k_gamePieceTag))
            {
                GamePiece gamePiece = other.GetComponent<GamePiece>();
                UpdatePieceCount(gamePiece, 1);
            }
        }

        /// <summary>
        /// Handles the event when a game piece exits the trigger collider attached to the game board.
        /// </summary>
        /// <param name="other">The Collider of the object that triggered the exit event.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Globals.k_gamePieceTag))
            {
                GamePiece gamePiece = other.GetComponent<GamePiece>();
                UpdatePieceCount(gamePiece, -1);
            }
        }

        /// <summary>
        /// Updates the count of game pieces based on their shape.
        /// </summary>
        /// <param name="gamePiece">The game piece whose count needs to be updated.</param>
        /// <param name="value">The value to update the count by (e.g., 1 for adding, -1 for removing).</param>
        private void UpdatePieceCount(GamePiece gamePiece, int value)
        {
            m_piecesInPlay = (uint)((int)m_piecesInPlay + value);

            switch (gamePiece.Shape)
            {
                case GamePiece.PieceShape.Cube:
                    m_cubesInPlay = (uint)((int)m_cubesInPlay + value);
                    break;
                case GamePiece.PieceShape.Cylinder:
                    m_cylindersInPlay = (uint)((int)m_cylindersInPlay + value);
                    break;
                case GamePiece.PieceShape.Capsule:
                    m_capsuleInPlay = (uint)((int)m_capsuleInPlay + value);
                    break;
                case GamePiece.PieceShape.Sphere:
                    m_spheresInPlay = (uint)((int)m_spheresInPlay + value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}