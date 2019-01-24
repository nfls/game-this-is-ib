using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DVariable<T> where T : struct {

	public delegate void Listener(T t);
	public event Listener onChanged;

	public T Value {
		get { return _value; }
		set {
			_realValue = value;
			Refresh();
			onChanged?.Invoke(value);
		}
	}

	public abstract int DecoratorCount { get; }
	
	[SerializeField]
	protected T _realValue;
	[SerializeField]
	protected T _value;

	public virtual void Refresh() => _value = _realValue;

	public static implicit operator T(DVariable<T> dVar) => dVar._value;
}

[Serializable]
public class DInt : DVariable<int> {

	public override int DecoratorCount => variableDecorators.Count;

	[SerializeField]
	public List<IntDecorator> variableDecorators = new List<IntDecorator>(2);
	
	public override void Refresh() {
		base.Refresh();
		foreach (var decorator in variableDecorators) _value = decorator.Execuate(_value);
	}

	public void Reorder() => variableDecorators.Sort(IntDecorator.Compare);
	
	public void AddDecorator(IntDecorator decorator) {
		int index = 0;
		for (int l = variableDecorators.Count; index < l; index++) if (decorator.priority < variableDecorators[index].priority) break;
		variableDecorators.Insert(index, decorator);
		Refresh();
	}

	public void RemoveDecorator(int index) {
		variableDecorators.RemoveAt(index);
		Refresh();
	}

	public void RemoveDecorator(IntDecorator decorator) {
		variableDecorators.Remove(decorator);
		Refresh();
	}
}

[Serializable]
public class DLong : DVariable<long> {
	
	public override int DecoratorCount => variableDecorators.Count;
	
	[SerializeField]
	public List<LongDecorator> variableDecorators = new List<LongDecorator>(2);
	
	public override void Refresh() {
		base.Refresh();
		foreach (var decorator in variableDecorators) _value = decorator.Execuate(_value);
	}
	
	public void Reorder() => variableDecorators.Sort(LongDecorator.Compare);
	
	public void AddDecorator(LongDecorator decorator) {
		int index = 0;
		for (int l = variableDecorators.Count; index < l; index++) if (decorator.priority < variableDecorators[index].priority) break;
		variableDecorators.Insert(index, decorator);
		Refresh();
	}
	
	public void RemoveDecorator(int index) {
		variableDecorators.RemoveAt(index);
		Refresh();
	}

	public void RemoveDecorator(LongDecorator decorator) {
		variableDecorators.Remove(decorator);
		Refresh();
	}
}

[Serializable]
public class DFloat : DVariable<float> {
	
	public override int DecoratorCount => variableDecorators.Count;
	
	[SerializeField]
	private List<FloatDecorator> variableDecorators = new List<FloatDecorator>(2);
	
	public override void Refresh() {
		base.Refresh();
		foreach (var decorator in variableDecorators) _value = decorator.Execuate(_value);
	}
	
	public void Reorder() => variableDecorators.Sort(FloatDecorator.Compare);
	
	public void AddDecorator(FloatDecorator decorator) {
		int index = 0;
		for (int l = variableDecorators.Count; index < l; index++) if (decorator.priority < variableDecorators[index].priority) break;
		variableDecorators.Insert(index, decorator);
		Refresh();
	}

	public void RemoveDecorator(FloatDecorator decorator) {
		variableDecorators.Remove(decorator);
		Refresh();
	}
}