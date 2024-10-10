using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    public class GamePiece : MonoBehaviour
    {
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
    }
}
