using UnityEngine;

public class Tile : IndexedObject {
    public bool isOccupied = false;
    public bool isHighlighted = false;

    private Material material;

    private void Start() {
        material = GetComponent<Renderer>().material;
    }

    public void ToggleHighlight(bool val) {
        isHighlighted = val;
        material.color = isHighlighted ? Color.green : Color.white;
    }
}
