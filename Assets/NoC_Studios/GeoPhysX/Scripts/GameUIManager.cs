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
        /// <summary>
        /// Header text for displaying the number of capsules currently in play on the game UI.
        /// </summary>
        const string k_capsulesInPlayHeader = "Capsules: ";

        /// <summary>
        /// Header text for displaying the number of cubes currently in play on the game UI.
        /// </summary>
        const string k_cubesInPlayHeader = "Cubes: ";

        /// <summary>
        /// Header text for displaying the number of cylinders currently in play on the game UI.
        /// </summary>
        const string k_cylindersInPlayHeader = "Cylinders: ";

        /// <summary>
        /// Header text for displaying the number of spheres currently in play on the game UI.
        /// </summary>
        const string k_spheresInPlayHeader = "Spheres: ";

        /// <summary>
        /// Header text for displaying the total number of game pieces currently in play on the game UI.
        /// </summary>
        const string k_totalPiecesInPlayHeader = "Total: ";

        /// <summary>
        /// Placeholder text used to indicate that nothing is currently selected or available.
        /// </summary>
        const string k_noResult = "   ";

        /// <summary>
        /// Name used to represent the capsule shape in the game UI.
        /// </summary>
        const string k_capsuleShapeName = "Capsule";

        /// <summary>
        /// The name representation for the cube shape in the game UI.
        /// </summary>
        const string k_cubeShapeName = "Cube";

        /// <summary>
        /// The display name for the Cylinder shape used in the game's user interface.
        /// </summary>
        const string k_cylinderShapeName = "Cylinder";

        /// <summary>
        /// The display name for the spherical game piece shape in the game's UI.
        /// </summary>
        const string k_sphereShapeName = "Sphere";

        /// <summary>
        /// The name of the color "Red" used to label or reference red-colored elements in the game UI.
        /// </summary>
        const string k_redColorName = "Red";

        /// <summary>
        /// Name of the color blue used in the game's user interface.
        /// </summary>
        const string k_blueColorName = "Blue";

        /// <summary>
        /// The name associated with the 'Green' color for game piece displays in the user interface.
        /// </summary>
        const string k_greenColorName = "Green";

        /// <summary>
        /// Reference to the GameBoard instance, which manages and tracks the state of game pieces on the board.
        /// </summary>
        [SerializeField] GameBoard m_gameBoard;

        /// <summary>
        /// TextMeshProUGUI element displaying the player's name on the game UI.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_playerName;

        /// <summary>
        /// UI element for displaying the player's current score.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_playerScoreCounter;

        /// <summary>
        /// UI element displaying the count of capsule-shaped game pieces currently in play.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_capsulePieceCounter;

        /// <summary>
        /// The UI text component that displays the current number of cube pieces in play.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_cubePieceCounter;

        /// <summary>
        /// UI text element for displaying the number of cylinders currently in play.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_cylinderPieceCounter;

        /// <summary>
        /// UI text component for displaying the current number of sphere pieces in play.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_spherePieceCounter;

        /// <summary>
        /// UI text component for displaying the total count of all game pieces currently in play.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_totalPieceCounter;

        /// <summary>
        /// UI element that displays the name of the next game piece shape to
        /// be introduced in the game.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_nextShape;

        /// <summary>
        /// UI element that displays the color of the next game piece to be introduced in the game.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_nextColor;

        /// <summary>
        /// Button that navigates the user back to the game's title screen.
        /// </summary>
        [SerializeField] Button m_returnToTitleButton;

        /// <summary>
        /// Initializes the GameUIManager by setting up UI button listeners and refreshing version text.
        /// This method is called on the frame when a script is enabled just before any of the Update methods are called.
        /// </summary>
        void Start()
        {
            m_returnToTitleButton.onClick.AddListener(GameManager.Instance.LoadTitleScreen);

            GameManager.Instance.RefreshVersionText();
        }

        /// <summary>
        /// Updates the player's name in the UI.
        /// This method sets the text of the m_playerName TextMeshProUGUI element to the provided player name.
        /// </summary>
        /// <param name="playerName">The new name of the player to be displayed in the UI.</param>
        public void UpdatePlayerName(string playerName)
        {
            m_playerName.text = playerName;
        }

        /// <summary>
        /// Updates the player's score displayed in the UI.
        /// This method sets the text of the m_playerScoreCounter TextMeshProUGUI element to the provided score
        /// prefixed with the "Score: " label.
        /// </summary>
        /// <param name="score">The latest score of the player to be displayed in the UI.</param>
        public void UpdatePlayerScore(int score)
        {
            const string k_scoreHeader = "Score: ";
            m_playerScoreCounter.text = $"{k_scoreHeader} {score}";
        }

    /// <summary>
        /// Updates the UI counter for the given game piece shape.
        /// This method adjusts the text value of the corresponding piece counter based on the current
        /// count of that piece shape in play and refreshes the total count.
        /// </summary>
        /// <param name="pieceShape">The shape of the game piece to update the counter for.</param>
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pieceShape), pieceShape, null);
            }

            RefreshTotalCount();
        }

        /// <summary>
        /// Updates the text of the next shape UI element based on the given piece shape.
        /// Displays a textual representation corresponding to the specified shape.
        /// </summary>
        /// <param name="pieceShape">The shape of the game piece to be displayed in the UI.</param>
        public void UpdateNextShape(GamePiece.PieceShape pieceShape)
        {
            m_nextShape.text = pieceShape switch
            {
                GamePiece.PieceShape.None => $"{k_noResult}",
                GamePiece.PieceShape.Cube => $"{k_cubeShapeName}",
                GamePiece.PieceShape.Cylinder => $"{k_cylinderShapeName}",
                GamePiece.PieceShape.Capsule => $"{k_capsuleShapeName}",
                GamePiece.PieceShape.Sphere => $"{k_sphereShapeName}",
                _ => throw new ArgumentOutOfRangeException(nameof(pieceShape), pieceShape, null)
            };
        }

        /// <summary>
        /// Updates the UI element to display the color of the next game piece based on the provided <paramref name="pieceColor"/>.
        /// </summary>
        /// <param name="pieceColor">The color of the next game piece to be displayed.</param>
        public void UpdateNextColor(GamePiece.PieceColor pieceColor)
        {
            m_nextColor.text = pieceColor switch
            {
                GamePiece.PieceColor.None => $"{k_noResult}",
                GamePiece.PieceColor.Red => $"{k_redColorName}",
                GamePiece.PieceColor.Green => $"{k_greenColorName}",
                GamePiece.PieceColor.Blue => $"{k_blueColorName}",
                _ => throw new ArgumentOutOfRangeException(nameof(pieceColor), pieceColor, null)
            };
        }

        /// <summary>
        /// Updates the total piece counter displayed in the UI.
        /// This method retrieves the total count of pieces currently in play from the GameBoard and updates the corresponding UI element.
        /// </summary>
        void RefreshTotalCount()
        {
            m_totalPieceCounter.text = $"{k_totalPiecesInPlayHeader} {m_gameBoard.PiecesInPlay}";
        }
    }
}