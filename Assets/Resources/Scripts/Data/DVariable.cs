using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public abstract class DVariable {
	
	[NonSerialized]
	public bool isExpanded;
	
	public abstract int DecoratorCount { get; }
	
	public abstract void Refresh();
	
	public abstract void Sort();

	public abstract void Remove(int index);
}

[Serializable]
public abstract class DVariable<TK, TV> : DVariable where TK : struct where TV : VariableDecorator, new() {

	public delegate void Listener(TK t);
	public event Listener onChanged;

	public TK Value {
		get { return _value; }
		set {
			realValue = value;
			Refresh();
			onChanged?.Invoke(value);
		}
	}

	public override int DecoratorCount => variableDecorators.Count;
	
	[SerializeField]
	public TK realValue;
	[SerializeField]
	public List<TV> variableDecorators = new List<TV>(2);
	[SerializeField]
	protected TK _value;

	public override void Sort() => variableDecorators.Sort(VariableDecorator.Compare);
	
	public override void Remove(int index) {
		variableDecorators.RemoveAt(index);
		Refresh();
	}

	public void Remove(TV decorator) {
		variableDecorators.Remove(decorator);
		Refresh();
	}
	
	public void Add(TV decorator) {
		int index = 0;
		for (int l = variableDecorators.Count; index < l; index++) if (decorator.priority < variableDecorators[index].priority) break;
		variableDecorators.Insert(index, decorator);
		Refresh();
	}

	public static implicit operator TK(DVariable<TK, TV> dVar) => dVar?._value ?? (TK) (object) 0;
}

[Serializable]
public class DInt : DVariable<int, IntDecorator> {
	
	public override void Refresh() {
		foreach (var decorator in variableDecorators) _value = decorator.Execute(_value);
	}
	
	public static implicit operator DInt(int value) => new DInt { Value = value };
}

[Serializable]
public class DLong : DVariable<long, LongDecorator> {
	
	public override void Refresh() {
		foreach (var decorator in variableDecorators) _value = decorator.Execute(_value);
	}
	
	public static implicit operator DLong(long value) => new DLong { Value = value };
}

[Serializable]
public class DFloat : DVariable<float, FloatDecorator> {
	
	public override void Refresh() {
		foreach (var decorator in variableDecorators) _value = decorator.Execute(_value);
	}
	
	public static implicit operator DFloat(float value) => new DFloat { Value = value };
}