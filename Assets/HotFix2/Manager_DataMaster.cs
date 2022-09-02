using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
public enum Table
{
    DataMasterIcons,
}
public enum Field
{
    none,
    id,
    name,
    type,
    count,
    goods,
}
public class TableDatas
{
    private Table type;
    private object data;
    public TFieldType Get<TFieldType>(Field field)
    {
        TFieldType ret = default;
        if (!object.Equals(data, null))
        {
            var retType = typeof(TFieldType);
            var fieldName = $"_{field.ToString()}";
            var tableData = Convert.ChangeType(data, Type.GetType(type.ToString()));

            var dataType = tableData.GetType();
            var dataField = dataType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default);
            switch (dataField)
            {
                case null:
                    var tableFields = tableData.GetType().GetFields(BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic);
                    var fields = "";
                    foreach (var item in tableFields)
                    {
                        var tempType = item.GetValue(tableData).GetType();
                        fields += $"{item.Name.Replace("_", "")}:{tempType}\n\t";
                    }
                    Debug.Log($"table: {type} , not find field: {field} \n" +
                        $"{fields}");
                    break;
                default:
                    try
                    {
                        var dataValue = dataField.GetValue(tableData);
                        var tempData = Convert.ChangeType(dataValue, retType);
                        ret = (TFieldType)tempData;
                    }
                    catch (Exception)
                    {
                        Debug.Log($"type convert defeated, value type: { dataField.GetValue(tableData).GetType()},  inpit type: {retType}");
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("传入参数不能为空");
        }
        return ret;
    }
}
public partial class Manager_DataMaster
{
    public static Manager_DataMaster Instance = new Manager_DataMaster();

    public TableDatas GetInstance<TKey>(Table table, TKey id)
    {
        TableDatas ret = null;

        var tableField = Instance.GetType().GetField(table.ToString());
        var tableValue = tableField.GetValue(Instance);
        var TryGetValueMethod = tableValue.GetType().GetMethod("TryGetValue");
        //var typeInstance = Activator.CreateInstance(Type.GetType(table.ToString()));
        object[] param = new object[] { id, null };
        TryGetValueMethod.Invoke(tableValue, param);
        var itemValue = param[1];
        if (!object.Equals(itemValue, null))
        {
            ret = new TableDatas();
            var retType = ret.GetType();
            retType.GetField("data", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default).SetValue(ret, itemValue);
            retType.GetField("type", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default).SetValue(ret, table);
        }
        else
        {
            Debug.Log($"table: {table}    key: {id}  is absent");
        }
        return ret;
    }

    public Dictionary<string, string> GetAll<TKey>(Table table, TKey id)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        var tableField = Instance.GetType().GetField(table.ToString());
        var tableValue = tableField.GetValue(Instance);
        var TryGetValueMethod = tableValue.GetType().GetMethod("TryGetValue");
        //var typeInstance = Activator.CreateInstance(Type.GetType(table.ToString()));
        object[] param = new object[] { id, null };
        TryGetValueMethod.Invoke(tableValue, param);
        var itemValue = param[1];
        if (!object.Equals(itemValue, null))
        {
            var itemType = itemValue.GetType();
            var itemFields = itemType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default);
            foreach (var item in itemFields)
            {
                ret.Add(item.Name, item.GetValue(itemValue).ToString());
            }
        }
        else
        {
            Debug.Log($"table: {table}    key: {id}  is absent");
        }
        return ret;
    }
    public TFieldType Get<TKey, TFieldType>(Table table, TKey id, Field field)
    {
        var tableField = Instance.GetType().GetField(table.ToString());
        var tableValue = tableField.GetValue(Instance);
        var TryGetValueMethod = tableValue.GetType().GetMethod("TryGetValue");
        var fieldName = $"_{field}";
        TFieldType ret = default;
        //var typeInstance = Activator.CreateInstance(Type.GetType(table.ToString()));
        object[] param = new object[] { id, null };
        TryGetValueMethod.Invoke(tableValue, param);
        var itemValue = param[1];
        if (!object.Equals(itemValue, null))
        {
            var itemType = itemValue.GetType();
            var itemField = itemType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default);
            switch (itemField)
            {
                case null:
                    var tableFields = itemValue.GetType().GetFields(BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic);
                    var fields = "";
                    foreach (var item in tableFields)
                    {
                        var tempType = item.GetValue(itemValue).GetType();
                        fields += $"{item.Name.Replace("_", "")}:{tempType}\n\t";
                    }
                    Debug.Log($"table: {table}   not find field: {field} \n" +
                        $"fields: {fields}");
                    break;
                default:
                    var itemFieldValue = itemField.GetValue(itemValue);
                    var fieldType = typeof(TFieldType);
                    try
                    {
                        var tempRet = Convert.ChangeType(itemFieldValue, fieldType);
                        ret = (TFieldType)tempRet;
                    }
                    catch (Exception)
                    {
                        Debug.Log($"type mismatching!  table field is type: {itemFieldValue.GetType()}, but TReturn is: {fieldType} ,  ought to input TReturn is: {itemFieldValue.GetType()}");
                    }
                    break;
            }
        }
        else
        {
            Debug.Log($"table: {table}    key: {id}  is absent");
        }
        return ret;
    }
}
 