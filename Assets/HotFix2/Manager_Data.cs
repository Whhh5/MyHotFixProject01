using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class Manager_Data
{
    public void InitializationData()
    {
        var manager_dataManager = Manager_Path.manager_dataManager;

        var assetType = LinkEditor.GetScriteType(manager_dataManager);
        var assetType_field = assetType.GetField("Instance", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        var assetTypeInstantiate = assetType_field.GetValue(assetType);
        GetExclesData(assetTypeInstantiate);
    }
    private void GetExclesData(object data)
    {
        var path_excles = Manager_Path.path_excles;
        var map_typeConvert = Manager_Path.map_typeConvert;

        var allExcleFiles = Directory.GetFiles(path_excles, "*.xlsx");
        foreach (var file in allExcleFiles)
        {
            var openFile = File.Open($"{file}", FileMode.Open, FileAccess.Read, FileShare.Read);
            var exlce = ExcelReaderFactory.CreateOpenXmlReader(openFile);
            var excleData = exlce.AsDataSet();
            var tables = excleData.Tables;
            for (int i = 0; i < tables.Count; i++)
            {
                var csName = tables[i].TableName;
                var type = LinkEditor.GetScriteType(csName);
                Dictionary<int, string> colums_rows = new Dictionary<int, string>();
                var field_tableAttributeF = data.GetType().GetField(csName);
                var value_tableAttribute = field_tableAttributeF.GetValue(data);
                var method_tableAdd = value_tableAttribute.GetType().GetMethod("Add");
                var method_tableAdd_param = method_tableAdd.GetParameters();

                //初始化 key: name -> value: index
                if (object.Equals(type, null))
                {
                    Debug.Log($"找不到类型    {csName}");
                    continue;
                }
                var rows = tables[i].Rows;
                for (int j = 0; j < rows.Count; j++)
                {
                    if (object.Equals(rows[j][0], "N"))
                    {
                        var itemArray = rows[j].ItemArray;
                        for (int k = 1; k < itemArray.Length; k++)
                        {
                            colums_rows.Add(k, itemArray[k].ToString());
                        }
                        break;
                    }
                }
                for (int j = 0; j < rows.Count; j++)
                {
                    var tempData = rows[j][0].ToString();
                    if (!object.Equals(tempData, ""))
                    {
                        Debug.Log($"{csName}   该行有关键字    {tempData.ToString()}");
                        continue;
                    }
                    var rowItemArr = rows[j].ItemArray;
                    var tempCs = LinkEditor.GetScriteType(csName);
                    var csInstnce = Activator.CreateInstance(tempCs);
                    for (int k = 1; k < rowItemArr.Length; k++)
                    {
                        var itemData = rowItemArr[k];
                        if (object.Equals(itemData.ToString(), ""))
                        {
                            Debug.Log($"table: {csName}  数据是空值   line {j + 1}   column {k + 1}");
                            continue;
                        }
                        string attributeName;
                        if (!colums_rows.TryGetValue(k, out attributeName))
                        {
                            Debug.Log($"{csName} colume = {k}   不存在属性索引");
                            continue;
                        }
                        var field = csInstnce.GetType().GetField($"_{attributeName}", BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
                        var tempItemValue = field.GetValue(csInstnce);
                        var tempItemType = tempItemValue.GetType();
                        itemData = itemData.ToString();
                        if (object.Equals(tempItemType.ToString(), map_typeConvert["string[]"]))
                        {
                            List<string> tempList = new List<string>();
                            foreach (var item in itemData.ToString().Split(';'))
                            {
                                tempList.Add(item);
                            }
                            itemData = (object)tempList;
                        }
                        var value = Convert.ChangeType(itemData, tempItemType);
                        field.SetValue(csInstnce, value);
                    }
                    var tempId = rowItemArr[1].ToString();
                    var tempDatas = csInstnce;
                    var param = new object[] { tempId, tempDatas };
                    if (object.Equals(tempId, ""))
                    {
                        Debug.Log($"{csName}  没有 id   line {j}");
                        continue;
                    }
                    for (int k = 0; k < method_tableAdd_param.Length; k++)
                    {
                        var paramType = method_tableAdd_param[k].ParameterType;
                        param[k] = Convert.ChangeType(param[k], paramType);
                    }
                    method_tableAdd.Invoke(value_tableAttribute, param);
                }
            }
            openFile.Close();
        }
    }
}
