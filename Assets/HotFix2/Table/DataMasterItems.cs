/*
*this script this template create
*template name: AssetClass.txt
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DataMasterItems
{
	[SerializeField] private ulong _type = 0;
	public ulong type {get=>_type;}
	[SerializeField] private float _attack = 0;
	public float attack {get=>_attack;}
	[SerializeField] private string _name = "";
	public string name {get=>_name;}
	[SerializeField] private ushort _age = 0;
	public ushort age {get=>_age;}
	[SerializeField] private uint _age2 = 0;
	public uint age2 {get=>_age2;}
	[SerializeField] private List<string> _goods = new List<string>();
	public List<string> goods {get=>_goods;}
	[SerializeField] private ulong _id = 0;
	public ulong id {get=>_id;}
	[SerializeField] private short _age3 = 0;
	public short age3 {get=>_age3;}


}