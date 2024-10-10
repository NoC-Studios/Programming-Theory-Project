using System;
using System.Collections.Generic;
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
        [SerializeField] GameUIManager m_gameUIManager;

        /// <summary>
        /// Represents the number of game pieces currently in play on the GameBoard.
        /// </summary>
        private uint m_piecesInPlay;

        /// <summary>
        /// Denotes the current count of cubes that are active within the game environment.
        /// </summary>
        private uint m_cubesInPlay;

        /// <summary>
        /// Represents the number of cylinders currently active on the GameBoard.
        /// </summary>
        private uint m_cylindersInPlay;

        /// <summary>
        /// Indicates the presence of the capsule in play within the game environment.
        /// </summary>
        private uint m_capsuleInPlay;

        /// <summary>
        /// Tracks the current number of active spheres within the game environment.
        /// </summary>
        private uint m_spheresInPlay;

        private BoxCollider boxCollider;
        private List<GameObject> gamePiecesInPlay;

        /// <summary>
        /// Gets the number of game pieces currently in play on the GameBoard.
        /// </summary>
        public uint PiecesInPlay => m_piecesInPlay;

        /// <summary>
        /// Denotes the current count of cubes that are active within the game environment.
        /// </summary>
        public uint CubesInPlay => m_cubesInPlay;

        /// <summary>
        /// Represents the number of cylinders currently active on the GameBoard.
        /// </summary>
        public uint CylindersInPlay => m_cylindersInPlay;

        /// <summary>
        /// Indicates the presence of the capsules in play within the game environment.
        /// </summary>
        public uint CapsulesInPlay => m_capsuleInPlay;

        /// <summary>
        /// Tracks the current number of active spheres within the game environment.
        /// </summary>
        public uint SpheresInPlay => m_spheresInPlay;

        void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                Debug.LogError("BoxCollider component missing from GameBoard.");
                enabled = false;
                return;
            }

            gamePiecesInPlay = new List<GameObject>();
        }

        void Update()
        {
            MonitorGamePieces();
        }

        void MonitorGamePieces()
        {
            List<GameObject> currentGamePieces = new List<GameObject>(gamePiecesInPlay);

            foreach (var piece in currentGamePieces)
            {
                if (piece == null || !IsWithinBounds(piece))
                {
                    UnregisterGamePiece(piece);
                }
            }

            m_gameUIManager.UpdateGamePieceCounter(GamePiece.PieceShape.Cube);
            m_gameUIManager.UpdateGamePieceCounter(GamePiece.PieceShape.Cylinder);
            m_gameUIManager.UpdateGamePieceCounter(GamePiece.PieceShape.Capsule);
            m_gameUIManager.UpdateGamePieceCounter(GamePiece.PieceShape.Sphere);
            m_gameUIManager.UpdateGamePieceCounter(GamePiece.PieceShape.None);
        }

        bool IsWithinBounds(GameObject piece)
        {
            return boxCollider.bounds.Contains(piece.transform.position);
        }

        public void RegisterGamePiece(GameObject piece)
        {
            if (!gamePiecesInPlay.Contains(piece))
            {
                gamePiecesInPlay.Add(piece);
                GamePiece gamePiece = piece.GetComponent<GamePiece>();
                UpdatePieceCount(gamePiece, 1);
            }
        }

        public void UnregisterGamePiece(GameObject piece)
        {
            if (gamePiecesInPlay.Contains(piece))
            {
                gamePiecesInPlay.Remove(piece);
                GamePiece gamePiece = piece.GetComponent<GamePiece>();
                UpdatePieceCount(gamePiece, -1);
            }
        }

        private void UpdatePieceCount(GamePiece gamePiece, int value = 0)
        {
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
                case GamePiece.PieceShape.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            RefreshTotalCount();
        }

        void RefreshTotalCount()
        {
            m_piecesInPlay = m_cubesInPlay + m_cylindersInPlay + m_capsuleInPlay + m_spheresInPlay;
        }
    }
}