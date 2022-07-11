#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
public partial class _Datatable {
private static readonly Lazy<_Datatable> _instance = new Lazy<_Datatable>(() => new _Datatable());
public static _Datatable Instance { get { return _instance.Value; } }

	public class etg2_villager {
		public int index;
		public int resource_index;
		public int resource_face_index;
		public string resource_body_index;
		public string ui_name;
		public string ui_explanation;
		public int grade;
		public int open_level;
		public int max_level;
		public int quest_index;
		public int mission_min_index;
		public int mission_max_index;
		public int ability1_type_index;
		public int ability2_type_index;
		public int ability3_type_index;
		public int ability4_type_index;
		public string stand_animation;
		public string walk_animation;
		public string workend_animation;
		public string crop_animation;
		public string animal_animation;
		public string factory_animation;
		public string store_animation;
		public string restorant_animation;
		public string theme_animation;
	};
	public Dictionary<int, etg2_villager> dtetg2_villager = new Dictionary<int, etg2_villager>();
	public void Loadetg2_villager(List<Dictionary<string, object>> rowList) {
		dtetg2_villager = new Dictionary<int, etg2_villager>();
		foreach (var rowItem in rowList) {
			etg2_villager dicItem = new etg2_villager();
			foreach (var item in rowItem) {
				var field = typeof(etg2_villager).GetField(item.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				try { field.SetValue(dicItem, item.Value); }
				catch { UnityEngine.Debug.LogError(item); }
			}
			if (dtetg2_villager.ContainsKey(dicItem.index)) {
				UnityEngine.Debug.LogError("Duplicate Key in etg2_villager");
				UnityEngine.Debug.LogError(string.Format("Duplicate Key {0}", dicItem.index));
			}
			dtetg2_villager.Add(dicItem.index, dicItem);
		}
	}
	public etg2_villager Getetg2_villagerData(int _index) {
		if (!dtetg2_villager.ContainsKey(_index)){
			UnityEngine.Debug.LogError("Table etg2_villager");
			UnityEngine.Debug.LogError(string.Format("table doesn't contain id {0}", _index));
			return null;
		}
		return dtetg2_villager[_index];
	}
	public Dictionary<int, etg2_villager> Getetg2_villagerData() {
		return dtetg2_villager;
	}


	public class Attacker {
		public int index;
		public int resource_index;
		public int resource_face_index;
		public string resource_body_index;
	};
	public Dictionary<int, Attacker> dtAttacker = new Dictionary<int, Attacker>();
	public void LoadAttacker(List<Dictionary<string, object>> rowList) {
		dtAttacker = new Dictionary<int, Attacker>();
		foreach (var rowItem in rowList) {
			Attacker dicItem = new Attacker();
			foreach (var item in rowItem) {
				var field = typeof(Attacker).GetField(item.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				try { field.SetValue(dicItem, item.Value); }
				catch { UnityEngine.Debug.LogError(item); }
			}
			if (dtAttacker.ContainsKey(dicItem.index)) {
				UnityEngine.Debug.LogError("Duplicate Key in Attacker");
				UnityEngine.Debug.LogError(string.Format("Duplicate Key {0}", dicItem.index));
			}
			dtAttacker.Add(dicItem.index, dicItem);
		}
	}
	public Attacker GetAttackerData(int _index) {
		if (!dtAttacker.ContainsKey(_index)){
			UnityEngine.Debug.LogError("Table Attacker");
			UnityEngine.Debug.LogError(string.Format("table doesn't contain id {0}", _index));
			return null;
		}
		return dtAttacker[_index];
	}
	public Dictionary<int, Attacker> GetAttackerData() {
		return dtAttacker;
	}

};
