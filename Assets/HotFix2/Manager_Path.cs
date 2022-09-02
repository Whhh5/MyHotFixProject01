using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Path
{
    public static Dictionary<string, string> map_typeConvert = new Dictionary<string, string>
    {
        { "int", "System.Int32"},
        { "short", "System.Int16"},
        { "float", "System.Single"},
        { "ushort", "System.UInt16"},
        { "uint", "System.UInt32"},
        { "ulong", "System.UInt64"},
        { "string", "System.String"},
        { "string[]", "System.Collections.Generic.List`1[System.String]"},
    };

    public static Dictionary<string, string> map_type = new Dictionary<string, string>
    {
        { "int", "int"},
        { "short", "short"},
        { "float", "float"},
        { "ushort", "ushort"},
        { "uint", "uint"},
        { "ulong", "ulong"},
        { "string", "string"},
        { "string[]", "List<string>"},
    };
    public static Dictionary<string, string> map_variableInit = new Dictionary<string, string>
    {
        { "int", "0"},
        { "short", "0"},
        { "float", "0"},
        { "ushort", "0"},
        { "uint", "0"},
        { "ulong", "0"},
        { "string", "\"\""},
        { "string[]", "new List<string>()"},
    };
    public static string ch = "/";
    public static string templet_attribute = "\t[SerializeField] private {0} _{1} = {2};\n\tpublic {0} {1} {{get=>_{1};}}\n";
    public static string templet_Dictionary = "public Dictionary<{0}, {1}> {1} = new Dictionary<{0}, {1}>();\n";
    public static string path_templet = $"{Application.dataPath}{ch}Editor{ch}Template{ch}";
    public static string path_excles = $"{Application.dataPath}{ch}Resource_Move{ch}Excles{ch}";
    public static string path_exclesScripts = $"{Application.dataPath}{ch}HotFix2{ch}Table{ch}";
    public static string path_exclesAssets = $"Assets{ch}HotFix2{ch}Excle{ch}";
    public static string manager_dataManager = "Manager_DataMaster";
    public static string manager_dataLocalization = "Manager_DataLocalization";
}
