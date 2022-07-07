using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public partial class ST_Price : TableBase
{
    public ObscuredInt Idx { get; set; }
    public ObscuredInt PriceType { get; set; }
    public ObscuredInt PriceRefer01 { get; set; }
    public ObscuredInt PriceRefer02 { get; set; }
    public ObscuredInt TicketIdx { get; set; }
    public ObscuredInt TicketCount { get; set; }
    public ObscuredInt ButtonText { get; set; }

    protected const string fileName = "Price.csv";
    protected static List<ST_Price> _list = new List<ST_Price>();  
    protected static Dictionary<int, ST_Price> Table = new Dictionary<int, ST_Price>();
    public static List<ST_Price> List { get { return _list; } }
    public static void Load(string path)
    {
        string csv = System.IO.File.ReadAllText(path + "/" + fileName);
        LoadFromCSV(csv);
    }
    public static void LoadFromCSV(string csv)
    {
        string[] datas = csv.Split(new string[] { NEWLINE }, System.StringSplitOptions.None);
        for (int it = 2; it < datas.Length; it++)
        {
            AddItem(datas[it]);
        }

    }
    private static void AddItem(string csv)
    {
        List<string> datalist = new List<string>(csv.Split('|'));
        var iter = datalist.GetEnumerator();
        var item = new ST_Price();
        iter.MoveNext();
        item.Idx = int.Parse(iter.Current);
        iter.MoveNext();
        item.PriceType = int.Parse(iter.Current);
        iter.MoveNext();
        item.PriceRefer01 = int.Parse(iter.Current);
        iter.MoveNext();
        item.PriceRefer02 = int.Parse(iter.Current);
        iter.MoveNext();
        item.TicketIdx = int.Parse(iter.Current);
        iter.MoveNext();
        item.TicketCount = int.Parse(iter.Current);
        iter.MoveNext();
        item.ButtonText = int.Parse(iter.Current);

        AddItem(item);

    }
    private static void AddItem(ST_Price item)
    {
        if(Table.ContainsKey(item.Idx) == false)
        {
            Table.Add(item.Idx, item);
            _list.Add(item);
        }
    }
    public static ST_Price GetItem(int _Idx)
    {
        if(Table.ContainsKey(_Idx) == true)
        { return Table[_Idx]; }
        else { return null; }
    }
    public static int GetCount()
    {
        return _list.Count;
    }
    public static ST_Price GetItemWithIndex(int _idx)
    {
        return _list[_idx];
    }
}