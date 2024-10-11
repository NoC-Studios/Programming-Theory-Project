using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Represents a game piece in the game environment.
    /// </summary>
    public class GamePiece : MonoBehaviour
    {
        /// <summary>
        /// The name of the material used for the red game piece. This constant string value
        /// is used to reference the red material in the game environment.
        /// </summary>
        const string k_redColorMaterialName = "RedGamePiece";

        /// <summary>
        /// The name of the material used for the green game piece. This constant string value
        /// is used to reference the green material in the game environment.
        /// </summary>
        const string k_greenColorMaterialName = "GreenGamePiece";

        /// <summary>
        /// The name of the material used for the blue game piece. This constant string value
        /// is used to reference the blue material in the game environment.
        /// </summary>
        const string k_blueColorMaterialName = "BlueGamePiece";
        
        /// <summary>
        /// Enum representing the various shapes a game piece can take within the game environment.
        /// </summary>
        public enum PieceShape { None, Cube, Cylinder, Capsule, Sphere }

        /// <summary>
        /// Enum representing the various colors a game piece can take within the game environment.
        /// </summary>
        public enum PieceColor { None, Red, Green, Blue }

        /// <summary>
        /// The shape of the game piece. This variable holds the PieceShape enum value
        /// that determines the specific geometry of the game piece within the game environment.
        /// </summary>
        [SerializeField] PieceShape m_pieceShape;

        /// <summary>
        /// The color of the game piece. This variable holds the PieceColor enum value
        /// that determines the specific color of the game piece within the game environment.
        /// </summary>
        [SerializeField] PieceColor m_pieceColor;

        /// <summary>
        /// Gets the current shape of the game piece as defined by the PieceShape enum.
        /// This property provides a way to retrieve the shape information of a game piece.
        /// </summary>
        public PieceShape Shape
        {
            get { return m_pieceShape; }
        }

        /// <summary>
        /// The color of the game piece. This property retrieves the PieceColor enum value
        /// that determines the specific color of the game piece within the game environment.
        /// </summary>
        public PieceColor Color
        {
            get { return m_pieceColor; }
        }

        /// <summary>
        /// Sets the color of the game piece based on the specified material.
        /// </summary>
        /// <param name="color">The material whose color name determines the color of the game piece.</param>
        public void SetColor(Material color)
        {
            m_pieceColor = color.name switch
            {
                k_redColorMaterialName => PieceColor.Red,
                k_greenColorMaterialName => PieceColor.Green,
                k_blueColorMaterialName => PieceColor.Blue,
                _ => PieceColor.None
            };
        }
    }
}
