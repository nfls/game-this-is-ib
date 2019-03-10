using UnityEngine;

public class DataSystem : MonoBehaviour {

	public StringDIntDictionary intData = new StringDIntDictionary();
	public StringDLongDictionary longData = new StringDLongDictionary();
	public StringDFloatDictionary floatData = new StringDFloatDictionary();

	public bool ContainsData(string key) {
		if (intData.ContainsKey(key)) return true;
		if (longData.ContainsKey(key)) return true;
		if (floatData.ContainsKey(key)) return true;
		return false;
	}

	public bool ContainsInt(string key) => intData.ContainsKey(key);
	
	public bool ContainsLong(string key) => longData.ContainsKey(key);
	
	public bool ContainsFloat(string key) => floatData.ContainsKey(key);

	public void AddData(string key, DInt data) => intData.Add(key, data);

	public void AddData(string key, DLong data) => longData.Add(key, data);
	
	public void AddData(string key, DFloat data) => floatData.Add(key, data);
	
	public void AddData(string key, int data) => intData.Add(key, data);

	public void AddData(string key, long data) => longData.Add(key, data);
	
	public void AddData(string key, float data) => floatData.Add(key, data);

	public void SetData(string key, DInt data) => intData[key] = data;

	public void SetData(string key, DLong data) => longData[key] = data;
	
	public void SetData(string key, DFloat data) => floatData[key] = data;
	
	public void SetData(string key, int data) => intData[key] = data;

	public void SetData(string key, long data) => longData[key] = data;
	
	public void SetData(string key, float data) => floatData[key] = data;

	public void SetData(string key, object data) {
		if (data is DInt || data is int) intData[key] = (DInt) data;
		else if (data is DLong || data is long) longData[key] = (DLong) data;
		else if (data is DFloat || data is float) floatData[key] = (DFloat) data;
		else throw new UnityException("Type " + data.GetType() + " is not supported !");
	}

	public bool Remove(string key) {
		if (intData.ContainsKey(key)) return intData.Remove(key);
		if (longData.ContainsKey(key)) return longData.Remove(key);
		if (floatData.ContainsKey(key)) return floatData.Remove(key);
		return false;
	}

	public bool RemoveInt(string key) => intData.Remove(key);
	
	public bool RemoveLong(string key) => longData.Remove(key);
	
	public bool RemoveFloat(string key) => floatData.Remove(key);

	public DInt GetInt(string key) => intData[key];
	
	public DLong GetLong(string key) => longData[key];
	
	public DFloat GetFloat(string key) => floatData[key];

	public DVariable GetData(string key) {
		if (intData.ContainsKey(key)) return intData[key];
		if (longData.ContainsKey(key)) return longData[key];
		if (floatData.ContainsKey(key)) return floatData[key];
		// throw new ArgumentException(key + " does not exist in any dictionaries !");
		return null;
	}

	public void AddDecorator(string key, IntDecorator decorator) => intData[key]?.Add(decorator);
	
	public void AddDecorator(string key, LongDecorator decorator) => longData[key]?.Add(decorator);
	
	public void AddDecorator(string key, FloatDecorator decorator) => floatData[key]?.Add(decorator);
	
	public void AddDecorator(string key, VariableDecorator decorator) {
		if (decorator is IntDecorator) intData[key]?.Add((IntDecorator) decorator);
		else if (decorator is LongDecorator) longData[key]?.Add((LongDecorator) decorator);
		else if (decorator is FloatDecorator) floatData[key]?.Add((FloatDecorator) decorator);
		else throw new UnityException("Type " + decorator.GetType() + " is not supported !");
	}
	
	public void RemoveDecorator(string key, IntDecorator decorator) => intData[key]?.Remove(decorator);
	
	public void RemoveDecorator(string key, LongDecorator decorator) => longData[key]?.Remove(decorator);
	
	public void RemoveDecorator(string key, FloatDecorator decorator) => floatData[key]?.Remove(decorator);

	public void RemoveDecorator(string key, VariableDecorator decorator) {
		if (decorator is IntDecorator) intData[key]?.Remove((IntDecorator) decorator);
		else if (decorator is LongDecorator) longData[key]?.Remove((LongDecorator) decorator);
		else if (decorator is FloatDecorator) floatData[key]?.Remove((FloatDecorator) decorator);
		else throw new UnityException("Type " + decorator.GetType() + " is not supported !");
	}
	
	public object this[string key] {
		get { return GetData(key); }
		set {
			if (value is DInt || value is int) intData[key] = (DInt) value;
			else if (value is DLong || value is long) longData[key] = (DLong) value;
			else if (value is DFloat || value is float) floatData[key] = (DFloat) value;
			else throw new UnityException("Type " + value.GetType() + " is not supported !");
		}
	}
}