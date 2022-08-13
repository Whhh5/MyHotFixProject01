using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BXB
{
    namespace Core
    {
        public class A_LinkList<TType>
        {
            public TType value;
            public A_LinkList<TType> next;
            public A_LinkList(TType value, A_LinkList<TType> node = null)
            {
                this.value = value;
                next = node;
            }
        }

        [Serializable]
        public class A_List<TType> : A_
            where TType : UnityEngine.Object
        {
            //内部使用调用这些
            [SerializeField] private ushort _unitCount;
            [SerializeField] private uint _countP;
            [SerializeField] private List<TType> _list;
            private A_LinkList<uint> _nicksLinkList;

            //外部获取调用这个
            public List<TType> list { get => _list; }
            public ushort unitCount { get => _unitCount; }
            public uint countP { get => _countP; }
            public A_LinkList<uint> nicks { get => _nicksLinkList; }



            public A_List(ushort unitCount = 10)
            {
                if (!(unitCount > 0))
                {
                    unitCount = 1;
                }
                //初始化变量
                Init(unitCount);
            }

            public void Init(ushort unitCount)
            {
                try
                {
                    TType[] arr = new TType[unitCount];
                    _list = new List<TType>(arr);
                    this._unitCount = unitCount;
                    _countP = 0;
                    _nicksLinkList = null;
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, "init list fail: " + exp.Message);
                }
            }

            private void AddNicksLinkList(uint index)
            {
                var link = new A_LinkList<uint>(index, _nicksLinkList);
                _nicksLinkList = link;
            }
            private bool TryGetNicksLinkList(out A_LinkList<uint> data)
            {
                bool ret = false;
                data = null;
                if (_nicksLinkList != null)
                {
                    data = _nicksLinkList;
                    _nicksLinkList = _nicksLinkList.next;
                    ret = true;
                }
                return ret;
            }

            public bool TryGetValueToIndex(uint index, out TType value)
            {
                bool ret = false;
                value = null;
                try
                {
                    if (index < _countP &&
                        index.TryToInt(out int f_index) &&
                        !object.ReferenceEquals(_list[f_index], null))
                    {
                        value = _list[f_index];
                        ret = true;
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            public bool TryGetIndexToValue(TType value, out uint index)
            {
                bool ret = false;
                index = 0;
                try
                {
                    if (!object.ReferenceEquals(value, null))
                    {
                        for (uint i = 0; i < _countP; i++)
                        {
                            if (TryGetValueToIndex(i, out TType f_value) &&
                                object.ReferenceEquals(value, f_value))
                            {
                                index = i;
                                ret = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            private bool TrySetValueToIndex(uint index, TType value, out TType oldValue, bool isNone = false)
            {
                bool ret = false;
                oldValue = null;
                try
                {
                    if (index.TryToInt(out int f_index) &&
                        f_index < _countP)
                    {
                        if (isNone || !object.ReferenceEquals(value, null))
                        {
                            oldValue = _list[f_index];
                            _list[f_index] = value;
                            ret = true;
                        }
                        else
                        {
                            A_LogToColor(Color.yellow, "list set fail     value is a null");
                        }
                    }
                    else
                    {
                        A_LogToColor(Color.yellow, "list set fail");
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            private bool ExtendList()
            {
                bool ret = false;
                try
                {
                    TType[] tempArr = new TType[_list.Count + _unitCount];
                    _list.CopyTo(0, tempArr, 0, _list.Count);
                    _list = new List<TType>(tempArr);
                    ret = true;
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            public bool TryAdd(TType value, out uint index)
            {
                bool ret = false;
                index = 0;
                try
                {
                    if (value != null)
                    {
                        if (TryGetNicksLinkList(out A_LinkList<uint> data))
                        {
                            index = data.value;
                        }
                        else
                        {
                            index = _countP;
                            _countP++;
                            Update();
                        }
                        if (TrySetValueToIndex(index, value, out TType f_value2))
                        {
                            ret = true;
                        }
                        else
                        {
                            _countP--;
                            A_LogToColor(Color.yellow, "list add fail    index a error");
                        }
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            public void Update()
            {
                if (_countP >= _list.Count)
                {
                    ExtendList();
                }
            }
            public bool TryGetAll(out Dictionary<uint, TType> values)
            {
                bool ret = false;
                values = null;
                if (_countP != 0)
                {
                    for (uint i = 0; i < _countP; i++)
                    {
                        if (TryGetValueToIndex(i, out TType f_value) &&
                            !object.ReferenceEquals(f_value, null))
                        {
                            values.Add(i, f_value);
                        }
                    }
                    ret = true;
                }
                return ret;
            }
            public bool TryFind(TType obj, out uint index)
            {
                bool ret = false;
                index = 0;
                for (uint i = 0; i < _countP; i++)
                {
                    if (TryGetValueToIndex(i, out TType element) && object.Equals(element, obj))
                    {
                        index = i;
                        ret = true;
                        break;
                    }
                }
                return ret;
            }


            public bool TryRemoveAtIndex(uint index, out TType oldValue)
            {
                bool ret = false;
                oldValue = null;
                try
                {
                    if (index.TryToInt(out int f_index) &&
                        TryGetValueToIndex(index, out TType f_value))
                    {
                        oldValue = f_value;
                        _list[f_index] = null;
                        AddNicksLinkList(index);
                        ret = true;
                    }
                    else
                    {
                        A_LogToColor(Color.yellow, $"remove fail, index: {index}");
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            public bool TryRemoveAtValue(TType value, out uint oldValue)
            {
                bool ret = false;
                oldValue = 0;
                try
                {
                    if (TryGetIndexToValue(value, out uint f_index) &&
                        TryRemoveAtIndex(f_index, out TType f_value))
                    {

                        oldValue = f_index;
                        ret = true;
                    }
                    else
                    {
                        A_LogToColor(Color.yellow, $"remove fail, value: {value.name}");
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            public bool TryClear(out List<TType> oldList)
            {
                oldList = null;
                bool ret = false;
                try
                {
                    if (_countP != 0)
                    {
                        oldList = _list;
                        Init(_unitCount);
                        ret = true;
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.yellow, $"Clear list fail, {exp.Message}");
                }
                return ret;
            }
        }
    }
}