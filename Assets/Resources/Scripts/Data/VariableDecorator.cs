using System;
using UnityEngine;

[Serializable]
public abstract class VariableDecorator<T> where T : struct {

	public VariableDecoratorType type;
	public int priority;
	public T value;

	public abstract T Execuate(T t);

	public static int Compare(VariableDecorator<T> decorator1, VariableDecorator<T> decorator2) {
		if (decorator1.priority > decorator2.priority) return 1;
		if (decorator1.priority == decorator2.priority) return 0;
		return -1;
	}
}

[Serializable]
public enum VariableDecoratorType {
	Add,
	Sub,
	Mul,
	Div
}

[Serializable]
public class IntDecorator : VariableDecorator<int> {
	
	public override int Execuate(int t) {
		switch (type) {
			case VariableDecoratorType.Add: return t + value;
			case VariableDecoratorType.Sub: return t - value;
			case VariableDecoratorType.Mul: return t * value;
			case VariableDecoratorType.Div: return t / value;
		}

		return t;
	}
}

[Serializable]
public class LongDecorator : VariableDecorator<long> {
	public override long Execuate(long t) {
		switch (type) {
			case VariableDecoratorType.Add: return t + value;
			case VariableDecoratorType.Sub: return t - value;
			case VariableDecoratorType.Mul: return t * value;
			case VariableDecoratorType.Div: return t / value;
		}

		return t;
	}
}

[Serializable]
public class FloatDecorator : VariableDecorator<float> {
	public override float Execuate(float t) {
		switch (type) {
			case VariableDecoratorType.Add: return t + value;
			case VariableDecoratorType.Sub: return t - value;
			case VariableDecoratorType.Mul: return t * value;
			case VariableDecoratorType.Div: return t / value;
		}

		return t;
	}
}