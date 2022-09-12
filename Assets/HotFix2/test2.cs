using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        return;
        var data = new Manager_Data();
        data.InitializationData();

        var name = Manager_DataMaster.Instance.Get<ulong, List<string>>(Table.DataMasterIcons, 1, Field.none);
        Debug.Log(name);

         var tt = Manager_DataMaster.Instance.GetAll<ulong>(Table.DataMasterIcons, 1);
        foreach (var item in tt)
        {
            Debug.Log($"{item.Key}: {item.Value}");
        }



        var items = Manager_DataMaster.Instance.GetInstance<ulong>(Table.DataMasterIcons, 1);
        var field = items.Get<ulong>(Field.id);
        Debug.Log(field);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
