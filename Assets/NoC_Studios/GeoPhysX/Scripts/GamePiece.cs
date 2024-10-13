using UnityEngine;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Represents a game piece in the game environment.
    /// </summary>
    public class GamePiece : MonoBehaviour
    {
        /// <summary>
        /// The shader property name used to control the emission color of a material. This constant string value is particularly
        /// useful for dynamically changing or adjusting the emission color of materials in the game environment.
        /// </summary>
        const string k_emissionColor = "_EmissionColor";

        /// <summary>
        /// Stores the unique identifier for the shader property that controls the emission color of materials.
        /// This identifier is crucial for setting or modifying the emission color dynamically within the game environment.
        /// </summary>
        static readonly int EmissionColor = Shader.PropertyToID(k_emissionColor);
        
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
        public PieceShape Shape => m_pieceShape;

        /// <summary>
        /// The color of the game piece. This property retrieves the PieceColor enum value
        /// that determines the specific color of the game piece within the game environment.
        /// </summary>
        public PieceColor Color => m_pieceColor;

        /// <summary>
        /// The color used to represent the glow effect of the game piece. This variable holds a UnityEngine.Color value
        /// that determines the visual glow color of the game piece within the game environment.
        /// </summary>
        [SerializeField] Color m_glowColor = UnityEngine.Color.white;

        /// <summary>
        /// The duration of the pulse effect for the game piece. This constant floating-point value
        /// specifies the length of time in seconds that the pulse effect remains active.
        /// </summary>
        const float k_pulseDuration = 1f;

        /// <summary>
        /// The intensity of the glow effect for the game piece. This float value ranges from 0 (no glow) to 10 (maximum glow),
        /// allowing customization of how prominently the game piece emits light or stands out in the game environment.
        /// </summary>
        [Range(0f, 10f)] [SerializeField] float m_glowIntesity = 1f;

        /// <summary>
        /// The time interval in seconds during which the game piece pulses or changes its visual state. This variable is used to control or animate
        /// the pulsing effect of the game piece, creating dynamic visual feedback based on the specified pulse time.
        /// </summary>
        float m_pulseTime;

        /// <summary>
        /// Indicates whether the game piece is currently pulsing. This boolean value determines if the game piece
        /// is in an animated state that causes it to pulse visually, adding a dynamic effect within the game environment.
        /// </summary>
        bool m_isPulsing = false;

        /// <summary>
        /// The material associated with the game piece. This variable is used to hold
        /// the Material object that defines the appearance of the game piece in terms
        /// of texture, shader, and color within the game environment.
        /// </summary>
        Material m_material;

        /// <summary>
        /// Initializes the game piece by setting up its material component.
        /// This method is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            m_material = GetComponentInChildren<Renderer>().material;
        }

        /// <summary>
        /// Enables the emission keyword for the material associated with the game piece.
        /// This method is called when the script instance is enabled just before any of the
        /// Update methods are called for the first time.
        /// </summary>
        void Start()
        {
            const string emissionKeyword = "_EMISSION"; 
            m_material.EnableKeyword(emissionKeyword);
        }

        /// <summary>
        /// Updates the game piece's pulsing state on each frame refresh. This method handles the timing and transition for the pulsing
        /// effect. If the game piece is pulsing, it increments the pulse time by delta time and triggers the Pulse method.
        /// Once the pulse duration is met, the pulsing state is reset.
        /// </summary>
        void Update()
        {
            if (m_isPulsing)
            {
                m_pulseTime += Time.deltaTime;
                Pulse();
            }

            if (m_pulseTime >= k_pulseDuration)
            {
                m_isPulsing = false;
                ResetPulseTime();
            }
        }

        /// <summary>
        /// Begins the pulsing animation for the game piece. This method enables the pulsing state,
        /// which is a visual effect to highlight the game piece within the game environment.
        /// </summary>
        void StartPulse()
        {
            m_isPulsing = true;
            ResetPulseTime();
        }

        /// <summary>
        /// Updates the emission color of the game piece material based on the current pulse time.
        /// This method creates a pulsing visual effect by varying the intensity of the emission color.
        /// It uses a PingPong function to calculate the pulsing effect proportionally within the pulse duration.
        /// </summary>
        void Pulse()
        {
            var emission = Mathf.PingPong(m_pulseTime, k_pulseDuration) / k_pulseDuration;
            m_material.SetColor(EmissionColor, m_glowColor * emission * m_glowIntesity);
        }

        /// <summary>
        /// Resets the pulse time to its initial value and updates the emission state of the game piece.
        /// </summary>
        void ResetPulseTime()
        {
            m_pulseTime = 0;
            ResetEmission();
        }

        /// <summary>
        /// Resets the emission color of the game piece's material to black.
        /// This method is typically called to turn off any emission effects on the material,
        /// ensuring that the game piece appears non-emissive.
        /// </summary>
        void ResetEmission()
        {
            m_material.SetColor(EmissionColor, UnityEngine.Color.black);
        }

        /// <summary>
        /// Sets the color of the game piece based on the specified material.
        /// </summary>
        /// <param name="color">The material whose color name determines the color of the game piece.</param>
        public void SetColor(Material color)
        {
            switch (color.name)
            {
                case k_redColorMaterialName:
                    m_pieceColor = PieceColor.Red;
                    m_glowColor = UnityEngine.Color.red;
                    break;
                case k_greenColorMaterialName:
                    m_pieceColor = PieceColor.Green;
                    m_glowColor = UnityEngine.Color.green;
                    break;
                case k_blueColorMaterialName:
                    m_pieceColor = PieceColor.Blue;
                    m_glowColor = UnityEngine.Color.cyan;
                    break;
                default:
                    m_pieceColor = PieceColor.None;
                    m_glowColor = UnityEngine.Color.white;
                    break;
            }
        }

        /// <summary>
        /// Triggered when the game piece collides with another object.
        /// Plays a collision sound using the GameManager instance.
        /// </summary>
        /// <param name="other">The Collision object that contains information about the collision.</param>
        void OnCollisionEnter(Collision other)
        {
            GameManager.Instance.PlayCollisionSound();
            StartPulse();
        }
    }
}