using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GearsGridManager : MonoBehaviour
{
    private static SlotsPlacer _slotsPlacer;
    
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private GridLayoutGroup gridRoot;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject motorPrefab;
    [SerializeField] private GameObject spawnerPrefab;
    
    private readonly List<Transform> slots = new();
    private GearsGridSimulator gearsGridSimulator;
    private readonly Dictionary<Vector2Int, RotatableHandler> _gears = new();

    private void Awake()
    {
        CreateGrid();
        _slotsPlacer = new SlotsPlacer(new HashSet<Transform>(slots), 50, (slot, draggable) =>
        {
            // Slot Empty Check
            if (slot.childCount == 0) return false;
            // Merge Possible Check
            if(!draggable.TryGetComponent(out GearMergable gear1)) return true;
            if(!slot.GetChild(0).TryGetComponent(out GearMergable gear2)) return true;
            // Merge
            if(!Mathf.Approximately(gear1.GearRotatable.Value, gear2.GearRotatable.Value)) return true;
            MergeGears(gear1, gear2);
            return false;
        });
        
        _slotsPlacer.OnDrop += OnGearDrop;
        
        var motor = Instantiate(motorPrefab, slots[0]);
        motor.name = "motor";
        RegisterGear(motor);
        
        var spawner = Instantiate(spawnerPrefab, slots[3]);
        spawner.name = "spawner";
        RegisterGear(spawner);
        
        var grid = _gears.Values.Select(gear => gear.Rotatable).ToList();
        gearsGridSimulator = new GearsGridSimulator(gridSize, grid);
    }

    public void RegisterGear(GameObject gear)
    {
        if (!gear.TryGetComponent(out Draggable draggable)) return;
        draggable.SetSlotsPlacer(_slotsPlacer);
        if (!draggable.TryGetComponent(out RotatableHandler gearHandler)) return;
        UpdateGrid(gearHandler);
    }

    public static void RegisterSlot(Transform slot)
    {
        _slotsPlacer.AddSlot(slot);
    }
    
    public void StartSimulation()
    {
        foreach (var (_, handler) in _gears)
        {
            if(!handler.TryGetComponent(out Draggable draggable)) continue;
            draggable.enabled = false;
        }

        StartCoroutine(Simulate());
    }

    private void OnGearDrop(bool success, Transform draggable)
    {
        if(!success) return;
        UpdateGrid(draggable.GetComponent<RotatableHandler>());
        SetSpawnerValues();
    }

    private void UpdateGrid(RotatableHandler gearHandler)
    {
        // Check is in grid
        if (gearHandler.transform.parent.parent != gridRoot.transform) return;
        var i = gearHandler.transform.parent.GetSiblingIndex();
        var newPosition = new Vector2Int(i % gridSize.y, i / gridSize.y);
        var oldPosition = gearHandler.Rotatable.Position;
        if (oldPosition == newPosition && _gears.ContainsKey(oldPosition)) return;

        // Exists, Position Changed
        if (_gears.ContainsKey(oldPosition))
        {
            print($"parent {i}, Updating Existing {oldPosition} to {newPosition}");
            // Remove Old Position & Set New Position
            gearHandler.UpdateRotatable(newPosition);
            _gears.Remove(oldPosition);
            _gears[newPosition] = gearHandler;
            gearsGridSimulator?.UpdateGrid(gearHandler.Rotatable, oldPosition);
        }
        else // Added New
        {
            print($"Adding New {newPosition}");
            gearHandler.Rotatable.SetPosition(newPosition);
            _gears[newPosition] = gearHandler;
            gearsGridSimulator?.UpdateGrid(gearHandler.Rotatable);
        }
    }

    private void Rotate(Vector2Int p, Rotatable rotatable, int steps)
    {
        _gears[p].RotateSteps(steps);
    }
    
    private void MergeGears(GearMergable gear1, GearMergable gear2)
    {
        // Update grids
        var oldPosition = gear2.GearRotatable.Position;
        _gears.Remove(oldPosition);
        if(gearsGridSimulator != null)
            gearsGridSimulator.Grid[oldPosition.y, oldPosition.x] = null;
        // Apply Merge
        gear1.GearRotatable.Merge(gear2.GearRotatable);
        Destroy(gear2.gameObject);
    }

    private IEnumerator Simulate()
    {
        gearsGridSimulator.OnRotatableStep += Rotate;
        SetSpawnerValues();
        while(gameObject.activeInHierarchy)
        {
            yield return gearsGridSimulator.MoveStep();
            yield return new WaitForSeconds(0.08f);
        }
        gearsGridSimulator.OnRotatableStep -= Rotate;
    }

    private void CreateGrid()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                var slot = Instantiate(slotPrefab, gridRoot.transform);
                slot.name = $"Slot [{i},{j}]";
                slots.Add(slot.transform);
            }
        }
        gridRoot.constraintCount = gridSize.y;
    }
    
    private void SetSpawnerValues()
    {
        foreach (var (_, rotatableHandler) in _gears)
        {
            rotatableHandler.SetGearActive(false);
            if (!rotatableHandler.TryGetComponent(out GearSpawnable spawnable)) continue;
            spawnable.SetChainValue(0);
        }

        var value = 0f;
        foreach (var (_, rotatableHandler) in _gears)
        {
            if (rotatableHandler.Rotatable is not MotorRotatable motorRotatable) continue;
            gearsGridSimulator.Traverse(motorRotatable, (p, rotatable) =>
            {
                if(rotatable is GearRotatable gearRotatable)
                    value = gearRotatable.AccumulateValue(value);
            }, new HashSet<Vector2Int>());
        }
        
        foreach (var (_, rotatableHandler) in _gears)
        {
            if (rotatableHandler.Rotatable is not MotorRotatable motorRotatable) continue;
            gearsGridSimulator.Traverse(motorRotatable, (p, rotatable) =>
            {
                _gears[p].SetGearActive(true);
                if (!_gears[p].TryGetComponent(out GearSpawnable spawnable))
                    return;
                spawnable.SetChainValue(value/16f);
            }, new HashSet<Vector2Int>());
            return;
        }
    }
}
