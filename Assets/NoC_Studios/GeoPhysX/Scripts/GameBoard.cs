using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Manages game pieces on the game board, including their lifecycle and count by shape.
    /// </summary>
    /// <remarks>
    /// The GameBoard class interacts with game pieces, providing functionalities such as spawning and removing game pieces. It also keeps track of the count of game pieces in play, categorized by their shapes.
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
        [SerializeField] List<GameObject> m_playablePieces;

        /// <summary>
        /// Represents the list of colors that can be assigned to the game pieces during gameplay.
        /// </summary>
        [SerializeField] List<Material> m_playableColors;

        /// <summary>
        /// Holds a reference to the next game piece that will be spawned on the game board.
        /// </summary>
        GameObject m_nextGamePiece;

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
        /// Initializes the GameBoard by setting up the initial next game piece to be spawned
        /// and creating a new HashSet to keep track of game pieces in play.
        /// </summary>
        void Start()
        {
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
            Instantiate(m_nextGamePiece, spawnPosition, m_nextGamePiece.transform.rotation);
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
            var gamePieceObject = GetRandomGamePiece();
            var gamePiece = gamePieceObject.GetComponent<GamePiece>();
            m_gameUIManager.UpdateNextShape(gamePiece.Shape);
            m_gameUIManager.UpdateNextColor(gamePiece.Color);
            return gamePieceObject;
        }

        /// <summary>
        /// Selects a random game piece from the list of playable pieces.
        /// </summary>
        /// <returns>
        /// A GameObject representing a randomly selected game piece.
        /// </returns>
        GameObject GetRandomGamePiece()
        {
            return ApplyRandomColor(m_playablePieces[Random.Range(0, m_playablePieces.Count)]);
        }

        /// <summary>
        /// Applies a random color from the list of available colors to the given game piece.
        /// </summary>
        /// <param name="gamePiece">The game piece to which the random color will be applied.</param>
        /// <returns>The game piece with the random color applied.</returns>
        GameObject ApplyRandomColor(GameObject gamePiece)
        {
            return ApplyColor(gamePiece, m_playableColors[Random.Range(0, m_playableColors.Count)]);
        }

        /// <summary>
        /// Applies the given color material to the specified game piece.
        /// </summary>
        /// <param name="gamePiece">The game piece to which the color will be applied.</param>
        /// <param name="coloredMaterial">The material representing the color to apply.</param>
        /// <return>The game piece with the applied color material.</return>
        GameObject ApplyColor(GameObject gamePiece, Material coloredMaterial)
        {
            gamePiece.GetComponentInChildren<MeshRenderer>().SetMaterials(new List<Material> {coloredMaterial});
            gamePiece.GetComponent<GamePiece>().SetColor(coloredMaterial);
            return gamePiece;
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