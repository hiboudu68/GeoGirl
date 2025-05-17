using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static float MoveSpeed = 5.2f;
    public static float JumpForce = 10f;
    public static float AngularForce = -290f;
    public static float Lift = 50f;
    public static float ShipGravity = 0.8f;
    public static float CubeHitboxSize = 0.6f;

    private Slider jumpSlider;
    private Text jumpText;
    private Slider angularSlider;
    private Text angularText;
    private Slider moveSlider;
    private Text moveText;
    private Slider liftSlider;
    private Text liftText;
    private Slider shipGravitySlider;
    private Text shipGravityText;
    private Slider cubeHitboxSizeSlider;
    private Text cubeHitboxSizeText;


    void Awake()
    {
        GameGrid grid = FindAnyObjectByType<GameGrid>();

        RetrieveTexts();
        RetrieveSliders();
    }
    private void RetrieveTexts()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            switch (text.name)
            {
                case "jumpValue":
                    jumpText = text;
                    break;
                case "rotationValue":
                    angularText = text;
                    break;
                case "speedValue":
                    moveText = text;
                    break;
                case "liftValue":
                    liftText = text;
                    break;
                case "shipGravityValue":
                    shipGravityText = text;
                    break;
                case "cubeHitboxSizeValue":
                    cubeHitboxSizeText = text;
                    break;
            }
        }
    }
    private void RetrieveSliders()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach (Slider slider in sliders)
        {
            switch (slider.name)
            {
                case "jumpStrength":
                    jumpSlider = slider;
                    slider.value = JumpForce;
                    break;
                case "rotationSpeed":
                    angularSlider = slider;
                    slider.value = -AngularForce;
                    break;

                case "speed":
                    moveSlider = slider;
                    slider.value = MoveSpeed;
                    break;

                case "lift":
                    liftSlider = slider;
                    slider.value = Lift;
                    break;

                case "shipGravity":
                    shipGravitySlider = slider;
                    slider.value = ShipGravity;
                    break;

                case "cubeHitboxSize":
                    cubeHitboxSizeSlider = slider;
                    slider.value = CubeHitboxSize;
                    break;
            }
        }
    }
    public void OnSpeedChanged()
    {
        MoveSpeed = moveSlider.value;
        moveText.text = MoveSpeed.ToString();
    }
    public void OnRotationChanged()
    {
        AngularForce = -angularSlider.value;
        angularText.text = (-AngularForce).ToString();
    }
    public void OnJumpStrengthChanged()
    {
        JumpForce = jumpSlider.value;
        jumpText.text = JumpForce.ToString();
    }
    public void OnLiftChanged()
    {
        Lift = liftSlider.value;
        liftText.text = Lift.ToString();
    }
    public void OnShipGravityChanged()
    {
        ShipGravity = shipGravitySlider.value;
        shipGravityText.text = ShipGravity.ToString();
    }
    public void OnCubeHitboxSizeChanged()
    {
        CubeHitboxSize = cubeHitboxSizeSlider.value;
        cubeHitboxSizeText.text = CubeHitboxSize.ToString();
    }
}
