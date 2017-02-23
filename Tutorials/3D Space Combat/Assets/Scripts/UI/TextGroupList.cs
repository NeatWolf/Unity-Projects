using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGroupList : MonoBehaviour {

    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private List<Row> rows;

    [System.Serializable]
    private struct Row
    {
        public string name;
        public string key;
    }

    void Start ()
    {
		foreach(var row in rows)
        {
            var instance = Instantiate(rowPrefab);
            instance.transform.SetParent(transform);
            instance.GetComponent<TextGroup>().SetValues(row.name, row.key);
        }
	}
}
