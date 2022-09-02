using Excel;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;

public class MyEditor : Editor
{
    [MenuItem("My Tools/Force Refresh")]
    public static void ForceRefresh()
    {
        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
    }
    public static string CreateAttribute(string type, string name)
    {
        var map_variableInit = Manager_Path.map_variableInit;
        var templet_attribute = Manager_Path.templet_attribute;

        string ret;
        string templeta;
        if (!map_variableInit.TryGetValue(type, out templeta))
        {
            templeta = "default";
        }
        type = GetTypeMap(type);
        ret = string.Format(templet_attribute, type, name, templeta);
        return ret;
    }
    public static string GetTypeMap(string type)
    {
        var map_type = Manager_Path.map_type;

        string ret;
        if (!map_type.TryGetValue(type, out ret))
        {
            ret = type;
        }
        return ret;
    }


    [MenuItem("My Tools/Read Excle And Create Script Class")]
    public static void ReadExcle()
    {
        var path_templet = Manager_Path.path_templet;
        var path_excles = Manager_Path.path_excles;
        var path_exclesScripts = Manager_Path.path_exclesScripts;
        var manager_dataManager = Manager_Path.manager_dataManager;
        var ch = Manager_Path.ch;

        string template;
        DataTableCollection excleData;

        //read class template
        var assetClass_Template_Name = "AssetClass.txt";
        var assetClass_Template_Path = $"{path_templet}{assetClass_Template_Name}";
        template = File.ReadAllText(assetClass_Template_Path);
        List<string> dataMaterTables = new List<string>();


        //read excle
        List<Task> tasks = new List<Task>();
        var allExcleFiles = Directory.GetFiles(path_excles, "*.xlsx");
        if (Directory.Exists(path_exclesScripts))
        {
            Directory.Delete(path_exclesScripts);
        }
        Directory.CreateDirectory(path_exclesScripts);
        foreach (var exclePath in allExcleFiles)
        {
            var fileStream = File.Open($"{exclePath}", FileMode.Open, FileAccess.Read, FileShare.Read);
            var excle = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
            var dataSet = excle.AsDataSet();
            excleData = dataSet.Tables;
            //get data
            for (int i = 0; i < excleData.Count; i++)
            {
                var applicationPath = Application.dataPath;
                var tableName = excleData[i].TableName;
                var tableData = excleData[i].Rows;
                var tableRowsCount = tableData.Count;
                dataMaterTables.Add(tableName);
                Dictionary<string, string> scriptsType = new Dictionary<string, string>();
                var T_Rows = -1;
                var N_Rows = -1;
                for (int j = 0; j < tableRowsCount; j++)
                {
                    try
                    {
                        if (T_Rows < 0 && object.Equals(tableData[j][0], "T")) T_Rows = j;
                        else if (N_Rows < 0 && object.Equals(tableData[j][0], "N")) N_Rows = j;
                        if (!(T_Rows < 0 || N_Rows < 0)) break;
                    }
                    catch (Exception exp)
                    {
                        Debug.Log(exp.Message);
                        Debug.Log($"{tableName} 表类型识别失败");
                        return;
                    }
                }
                if (N_Rows < 0 || T_Rows < 0)
                {
                    Debug.Log($"{tableName} 未找到关键字  T  or N  跳过该表");
                    break;
                }
                var itemArr_T = tableData[T_Rows].ItemArray;
                var itemArr_N = tableData[N_Rows].ItemArray;
                for (int j = 1; j < itemArr_T.Length; j++)
                {
                    if (j >= itemArr_N.Length || object.Equals(itemArr_N[j], "") || object.Equals(itemArr_T[j], ""))
                    {
                        Debug.Log($"table： {tableName} 属性的类型或者名字有一个为空 列号：{j}");
                        break;
                    }
                    var tempName = itemArr_N[j].ToString();
                    var tempType = itemArr_T[j].ToString();
                    scriptsType.Add(tempName, tempType);
                }
                Debug.Log($"检测到 {tableName} 表中有 {excleData[i].Rows.Count} 行 {excleData[i].Columns.Count} 列");
                if (object.Equals(scriptsType.Count, 0))
                {
                    Debug.Log($"{tableName} 未找到关键字  T  跳过该表");
                    continue;
                }



                Task templetaTask = Task.Run(() =>
                {
                    var values = scriptsType;
                    var csName = tableName;
                    string attarbutes = "";
                    string applocationPath = applicationPath;


                    foreach (var item in values)
                    {
                        //产生随机数
                        RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
                        byte[] byteCsp = new byte[10];
                        csp.GetBytes(byteCsp);
                        //--
                        attarbutes += CreateAttribute(item.Value, item.Key);
                    }

                    var assetsClassPath = $"{path_exclesScripts}{csName}.cs";
                    if (File.Exists(assetsClassPath))
                    {
                        File.Delete(assetsClassPath);
                    }
                    FileStream file = File.Create(assetsClassPath);
                    var content = string.Format(template, csName, attarbutes, "");
                    byte[] stream = Encoding.UTF8.GetBytes(content);
                    file.Write(stream, 0, stream.Length);
                    file.Close();
                    Debug.Log(attarbutes);
                });
                tasks.Add(templetaTask);
            }
        }
        var tempTask = Task.Run(() =>
         {
             var path_dataManager = $"{path_exclesScripts}{ch}{manager_dataManager}_Partial.cs";
             var paramTemporary = Manager_Path.templet_Dictionary;
             if (File.Exists(path_dataManager))
             {
                 File.Delete(path_dataManager);
             }
             var tempTablesData = File.ReadAllText($"{path_templet}{ch}TablesDataClass.txt");
             var str = "";
             foreach (var item in dataMaterTables)
             {
                 str += string.Format(paramTemporary, "ulong", item);
             }
             tempTablesData = string.Format(tempTablesData, manager_dataManager, str);
             var txtBuffer = Encoding.UTF8.GetBytes(tempTablesData);
             var createFile = File.Create($"{path_exclesScripts}{ch}{manager_dataManager}_Partial.cs");
             createFile.Write(txtBuffer, 0, txtBuffer.Length);
         });
        tasks.Add(tempTask);


        Task.WaitAll(tasks.ToArray());


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}