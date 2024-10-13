using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
        /// Represents the current mission that needs to be accomplished in the game.
        /// Stores details such as the count, color, and shape of game pieces required to complete the mission.
        /// </summary>
        Mission m_currentMission;

        /// <summary>
        /// Tracks the player's current score in the game.
        /// </summary>
        int m_score;

        /// <summary>
        /// Tracks the remaining time for the current mission.
        /// </summary>
        float m_currentMissionTimeLeft;

        /// <summary>
        /// Indicates whether the game has ended.
        /// </summary>
        bool m_isGameOver = false;

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
        /// Gets a value indicating whether the game has ended.
        /// </summary>
        public bool IsGameOver => m_isGameOver;

        /// <summary>
        /// Initializes the GameBoard by setting up the initial game piece to be spawned,
        /// resetting the score to zero, and creating a new collection to keep track of
        /// game pieces currently in play.
        /// </summary>
        void Start()
        {
            m_gamePiecesInPlay = new HashSet<GamePiece>();
            ResetScore();
            SetPlayerName(GameManager.Instance.PlayerName);
            GetNextMission();
            m_nextGamePiece = GetNextGamePiece();
        }

        /// <summary>
        /// Updates the game state by iterating through all game pieces currently in play
        /// and checking each one for matches. It also updates the mission timer based on the
        /// elapsed game time.
        /// </summary>
        void Update()
        {
            UpdateMissionTimer(Time.deltaTime);
            
            if (m_gamePiecesInPlay == null ||
                !m_gamePiecesInPlay.Any() ||
                m_isGameOver) return;

            foreach (var piece in m_gamePiecesInPlay.ToList())
            {
                CheckForMatches(piece);
            }
        }

        /// <summary>
        /// Spawns the next game piece at the specified position on the game board.
        /// The spawned game piece uses the transform rotation of the next game piece.
        /// The next game piece is then updated to a new randomly selected piece.
        /// </summary>
        /// <param name="spawnPosition">The position at which the next game piece will be spawned.</param>
        public void SpawnNextGamePiece(Vector3 spawnPosition)
        {
            GameManager.Instance.PlayDropSound();
            Instantiate(m_nextGamePiece, spawnPosition, m_nextGamePiece.transform.rotation);
            m_nextGamePiece = GetNextGamePiece();
        }

        /// <summary>
        /// Removes the specified game piece from the game board.
        /// </summary>
        /// <param name="gamePiece">The game piece to be removed.</param>
        void RemoveGamePiece(GamePiece gamePiece)
        {
            UnregisterGamePiece(gamePiece);
            DestroyGamePiece(gamePiece);
        }

        /// <summary>
        /// Destroys the specified game piece, removing it from the game board and performing related cleanup.
        /// </summary>
        /// <param name="gamePiece">The game piece to be destroyed.</param>
        static void DestroyGamePiece(GamePiece gamePiece)
        {
            //TODO: Add in vfx when destroying a piece
            GameManager.Instance.PlayRemoveSound();
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
        static GameObject ApplyColor(GameObject gamePiece, Material coloredMaterial)
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

        /// <summary>
        /// Checks if there are any matching game pieces connected to the provided game piece.
        /// If the number of connected matches meets or exceeds the minimum required count,
        /// the matching pieces are removed and the score is updated.
        /// </summary>
        /// <param name="pieceToCheck">The game piece that needs to be checked for matches.</param>
        void CheckForMatches(GamePiece pieceToCheck)
        {
            const int minimumMatchCount = 3;

            var matchingPieces = new HashSet<GamePiece>();
            FindMatchingPieces(pieceToCheck, matchingPieces);
            var matchingCount = matchingPieces.Count;
            if (matchingCount < minimumMatchCount) return;

            foreach (var piece in matchingPieces)
            {
                ExplodePiece(piece);
                RemoveGamePiece(piece);
            }
            
            UpdateScore(matchingCount);
            
            var matchingPiece = matchingPieces.First();
            var matchingShape = matchingPiece.Shape;
            var matchingColor = matchingPiece.Color;
            UpdateMission(matchingColor, matchingShape, matchingCount);
        }

        /// <summary>
        /// Sets the player's name and updates the game UI to reflect the new name.
        /// </summary>
        /// <param name="playerName">The name of the player to display in the game UI.</param>
        void SetPlayerName(string playerName)
        {
            m_gameUIManager.UpdatePlayerName(playerName);
        }

        /// <summary>
        /// Updates the player's score by adding the specified amount and refreshes the score display.
        /// </summary>
        /// <param name="scoreAmountToAdd">The amount to add to the current score.</param>
        void UpdateScore(int scoreAmountToAdd)
        {
            m_score += scoreAmountToAdd;
            m_gameUIManager.UpdatePlayerScore(m_score);
        }
        
        /// <summary>
        /// Resets the player's score to zero.
        /// </summary>
        void ResetScore()
        {
            m_score = 0;
        }

        /// <summary>
        /// Applies an explosion force to the nearby game pieces when a specified game piece explodes.
        /// </summary>
        /// <param name="pieceToExplode">The game piece that will cause the explosion.</param>
        static void ExplodePiece(GamePiece pieceToExplode)
        {
            const float explosionForce = 5000f;
            const float explosionRadius = 25f;

            var hitColliders = Physics.OverlapSphere(pieceToExplode.transform.position, 10f);
            foreach (var hitCollider in hitColliders)
            {
                var hitRigidBody = hitCollider.GetComponent<Rigidbody>();
                if (hitRigidBody != null)
                {
                    hitRigidBody.AddExplosionForce(explosionForce, pieceToExplode.transform.position, explosionRadius);
                }
            }
        }

        /// <summary>
        /// Recursively finds and adds all game pieces that match the shape and color
        /// of the specified starting game piece within a given radius.
        /// </summary>
        /// <param name="startPiece">The initial game piece to start the search from.</param>
        /// <param name="matchingPieces">A collection to store the matching game pieces found during the search.</param>
        static void FindMatchingPieces(GamePiece startPiece, HashSet<GamePiece> matchingPieces)
        {
            if (matchingPieces.Contains(startPiece)) return;
            
            const float matchRadius = 1f;
            
            matchingPieces.Add(startPiece);
            var hitColliders = Physics.OverlapSphere(startPiece.transform.position, matchRadius);
            foreach (var hitCollider in hitColliders)
            {
                var otherPiece = hitCollider.GetComponent<GamePiece>();
                if (otherPiece != null &&
                    otherPiece.Shape == startPiece.Shape &&
                    otherPiece.Color == startPiece.Color)
                {
                    FindMatchingPieces(otherPiece, matchingPieces);
                }
            }
        }

        /// <summary>
        /// Sets a new mission for the game by randomly selecting a game piece color and shape,
        /// and establishing the required match count. Updates the game UI with the current mission details.
        /// </summary>
        void GetNextMission()
        {
            var colorCount = Enum.GetValues(typeof(GamePiece.PieceColor)).Length;
            var shapeCount = Enum.GetValues(typeof(GamePiece.PieceShape)).Length;

            // 0 index is None for color and shape, so start with 1.
            var colorIndex = Random.Range(1, colorCount);
            var shapeIndex = Random.Range(1, shapeCount);

            const int matchCount = 3;
            const int missionScore = 30;
            const float missionDuration = 90f;

            m_currentMission = new Mission()
            {
                MatchCount = matchCount,
                PieceColor = (GamePiece.PieceColor)colorIndex,
                PieceShape = (GamePiece.PieceShape)shapeIndex,
                MissionScore = missionScore,
                MissionDuration = missionDuration
            };

            SetMissionTimer();
            
            m_gameUIManager.SetMissionText(m_currentMission.ToString());
        }

        /// <summary>
        /// Sets the mission timer for the current mission by initializing the time left
        /// with the mission's duration and updating the UI to reflect this time.
        /// </summary>
        void SetMissionTimer()
        {
            var missionDuration = m_currentMission.MissionDuration;
            m_currentMissionTimeLeft = missionDuration;
            m_gameUIManager.RefreshMissionTimer(m_currentMissionTimeLeft);
        }

        /// <summary>
        /// Updates the remaining time for the current mission, ending the game if the timer reaches zero, and updates the mission timer UI.
        /// </summary>
        /// <param name="deltaTime">The amount of time that has passed since the last update.</param>
        void UpdateMissionTimer(float deltaTime)
        {
            m_currentMissionTimeLeft -= deltaTime;
            
            if (m_currentMissionTimeLeft <= 0f)
            {
                m_currentMissionTimeLeft = 0f;
                m_isGameOver = true;
                m_gameUIManager.ToggleGameOverUI();
            }

            m_gameUIManager.RefreshMissionTimer(m_currentMissionTimeLeft);
        }

        /// <summary>
        /// Updates the current mission by checking if the provided game piece color, shape,
        /// and match count meet or exceed the mission's requirements. If the mission's criteria are met,
        /// the mission is marked as completed.
        /// </summary>
        /// <param name="pieceColor">The color of the game pieces that matched.</param>
        /// <param name="pieceShape">The shape of the game pieces that matched.</param>
        /// <param name="matchCount">The number of game pieces that matched.</param>
        void UpdateMission(GamePiece.PieceColor pieceColor, GamePiece.PieceShape pieceShape, int matchCount)
        {
            if (m_currentMission.PieceColor == pieceColor &&
                m_currentMission.PieceShape == pieceShape && 
                m_currentMission.MatchCount <= matchCount)
            {
                CompleteMission();
            }
        }

        /// <summary>
        /// Completes the current mission by awarding a predefined score to the player
        /// and then initiates a new mission.
        /// </summary>
        void CompleteMission()
        {
            UpdateScore(m_currentMission.MissionScore);
            GetNextMission();
        }

        /// <summary>
        /// Represents a mission objective in the game, specifying the target match count, piece color, and piece shape.
        /// </summary>
        /// <remarks>
        /// The Mission struct is used to define the goals that players need to achieve, which includes the number of pieces, their color, and their shape. This struct helps in managing game objectives dynamically during gameplay.
        /// </remarks>
        struct Mission
        {
            /// <summary>
            /// The number of matches required to complete the mission objective.
            /// </summary>
            public int MatchCount;

            /// <summary>
            /// Enum representing the various colors a game piece can take within the game environment.
            /// </summary>
            public GamePiece.PieceColor PieceColor;

            /// <summary>
            /// Represents the shape of a game piece in the game environment.
            /// </summary>
            /// <remarks>
            /// This enumeration defines the different shapes that a game piece can assume, which can be used for gameplay mechanics and mission objectives.
            /// </remarks>
            public GamePiece.PieceShape PieceShape;

            /// <summary>
            /// The score awarded upon completion of the mission objective.
            /// </summary>
            public int MissionScore;

            /// <summary>
            /// Specifies the duration, in seconds, allotted to complete the mission objective.
            /// </summary>
            public float MissionDuration;

            /// <summary>
            /// Returns a string representation of the mission, including the required match count,
            /// piece color, and piece shape in a readable format.
            /// </summary>
            /// <returns>
            /// A string that describes the mission objective in the format "Clear {MatchCount} {PieceColor} {PieceShape}".
            /// </returns>
            public override string ToString()
            {
                return $"Clear {MatchCount} {PieceColor} {PieceShape}";
            }
        }
    }
}