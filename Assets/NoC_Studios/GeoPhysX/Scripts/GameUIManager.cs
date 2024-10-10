using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Manages the game's user interface components.
    /// </summary>
    public class GameUIManager : MonoBehaviour
    {
        const string k_capsulesInPlayHeader = "Capsules: ";
        const string k_cubesInPlayHeader = "Cubes: ";
        const string k_cylindersInPlayHeader = "Cylinders: ";
        const string k_spheresInPlayHeader = "Spheres: ";
        const string k_totalPiecesInPlayHeader = "Total: ";

        [SerializeField] GameBoard m_gameBoard;
        [SerializeField] TextMeshProUGUI m_capsulePieceCounter;
        [SerializeField] TextMeshProUGUI m_cubePieceCounter;
        [SerializeField] TextMeshProUGUI m_cylinderPieceCounter;
        [SerializeField] TextMeshProUGUI m_spherePieceCounter;
        [SerializeField] TextMeshProUGUI m_totalPieceCounter;
        [SerializeField] Button m_returnToTitleButton;

        void Start()
        {
            m_returnToTitleButton.onClick.AddListener(GameManager.Instance.LoadTitleScreen);

            GameManager.Instance.RefreshVersionText();
        }

        public void UpdateGamePieceCounter(GamePiece.PieceShape pieceShape)
        {
            switch (pieceShape)
            {
                case GamePiece.PieceShape.Cube:
                    m_cubePieceCounter.text = $"{k_cubesInPlayHeader} {m_gameBoard.CubesInPlay}";
                    break;
                case GamePiece.PieceShape.Cylinder:
                    m_cylinderPieceCounter.text = $"{k_cylindersInPlayHeader} {m_gameBoard.CylindersInPlay}";
                    break;
                case GamePiece.PieceShape.Capsule:
                    m_capsulePieceCounter.text = $"{k_capsulesInPlayHeader} {m_gameBoard.CapsulesInPlay}";
                    break;
                case GamePiece.PieceShape.Sphere:
                    m_spherePieceCounter.text = $"{k_spheresInPlayHeader} {m_gameBoard.SpheresInPlay}";
                    break;
                case GamePiece.PieceShape.None:
                    m_totalPieceCounter.text = $"{k_totalPiecesInPlayHeader} {m_gameBoard.PiecesInPlay}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pieceShape), pieceShape, null);
            }
        }
    }
}