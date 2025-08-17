using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearMergable : MonoBehaviour
{
    
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private Image gearImage;
    [SerializeField] Gradient gradient;

    private RotatableHandler rotatableHandler;
    public GearRotatable GearRotatable => rotatableHandler.Rotatable as GearRotatable;

    private void Awake()
    {
        rotatableHandler = GetComponent<RotatableHandler>();
    }

    private void OnEnable()
    {
        GearRotatable.Merged += OnMerge;
        RotatableHandler.ActiveChanged += SetGearActive;
    }

    private void OnDisable()
    {
        GearRotatable.Merged -= OnMerge;
        RotatableHandler.ActiveChanged -= SetGearActive;
    }

    private void Start()
    {
        SetValue();
    }
    
    private void SetGearActive(RotatableHandler handler, bool value)
    {
        if (handler != rotatableHandler) return;
        var color = gearImage.color;
        color.a = value ? 1f : 0.5f;
        gearImage.color = color;
    }

    private void OnMerge(GearRotatable first, GearRotatable other)
    {
        if (first != GearRotatable)
            return;
        SetValue();
    }

    private void SetValue()
    {
        gearImage.color = gradient.Evaluate(GearRotatable.Value / 5f);
        gearText.text = $"{GearRotatable.Value.ToString()}{(GearRotatable.IsMultiplier ? "x" : "")}";
    }
}
