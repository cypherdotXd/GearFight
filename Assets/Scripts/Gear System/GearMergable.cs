using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearMergable : MonoBehaviour
{
    public GearRotatable GearRotatable => rotatableHandler.Rotatable as GearRotatable;
    
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private Image gearImage;
    [SerializeField] Gradient gradient;

    private RotatableHandler rotatableHandler;

    private void Awake()
    {
        rotatableHandler = GetComponent<RotatableHandler>();
    }

    private void OnEnable()
    {
        GearRotatable.Merged += OnMerge;
    }

    private void OnDisable()
    {
        GearRotatable.Merged -= OnMerge;
    }

    private void Start()
    {
        SetValue();
    }

    private void OnMerge(GearRotatable other)
    {
        print("MERGe");
        SetValue();
    }

    private void SetValue()
    {
        gearImage.color = gradient.Evaluate(GearRotatable.Value/5f);
        gearText.text = $"{GearRotatable.Value.ToString()}{(GearRotatable.isMultiplier ? "x" : "")}";
    }
}
