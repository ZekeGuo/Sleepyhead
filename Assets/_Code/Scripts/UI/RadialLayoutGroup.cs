using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class RadialLayoutGroup : MonoBehaviour
{
    [Range(100.0f,1000.0f)] [SerializeField] private float _radius;
    [Range(0.0f, 360.0f)] [SerializeField] private float _startingAngle;
    [SerializeField] private bool _startWithOffset;
    private List<GameObject> _groupObjects = new List<GameObject>();

    private void PlaceObjectsWithinRing()
    {
        float angularSpacing = 360.0f / _groupObjects.Count;
        float startAngle = _startingAngle;
        Vector2 currPosition = Vector2.right;

        if (_startWithOffset)
        {
            startAngle += angularSpacing * 0.5f;
        }

        for (int i = 0; i < _groupObjects.Count; i++)
        {
            float currAngle = startAngle + i * angularSpacing;

            Quaternion rot = Quaternion.AngleAxis(currAngle, Vector3.forward);
            Vector3 resultPosition = rot * currPosition * _radius;

            _groupObjects[i].transform.position = this.transform.position + resultPosition;
        }
    }

    public void OnValidate()
    {
        AddChildrenToLayoutGroup();

        if (_groupObjects.Count == 0)
        {
            return;
        }

        List<GameObject> withDupes = _groupObjects;
        _groupObjects = withDupes.Distinct().ToList();
        _groupObjects.RemoveAll(item => item == null);

        PlaceObjectsWithinRing();
    }

    private void AddChildrenToLayoutGroup()
    {
        _groupObjects.Clear();
        foreach (Transform child in transform)
        {
            _groupObjects.Add(child.gameObject);
        }
    }

    private void OnTransformChildrenChanged()
    {
        OnValidate();
    }
}