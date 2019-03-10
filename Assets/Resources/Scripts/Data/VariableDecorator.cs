using System;

[Serializable]
public abstract class VariableDecorator {
	
	public int priority;
	public VariableDecoratorType type;

	public VariableDecorator() { }

	public static int Compare(VariableDecorator decorator1, VariableDecorator decorator2) {
		if (decorator1.priority > decorator2.priority) return 1;
		if (decorator1.priority == decorator2.priority) return 0;
		return -1;
	}
}

[Serializable]
public abstract class VariableDecorator<T> : VariableDecorator where T : struct {
	
	public T value;

	public abstract T Execute(T t);

	public int Compare(VariableDecorator decorator) => VariableDecorator.Compare(this, decorator);
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
	
	public override int Execute(int t) {
		switch (type) {
			case VariableDecoratorType.Add: return t + value;
			case VariableDecoratorType.Sub: return t - value;
			case VariableDecoratorType.Mul: return t * value;
			case VariableDecoratorType.Div: return t / (value == 0 ? 1 : value);
		}

		return t;
	}
}

[Serializable]
public class LongDecorator : VariableDecorator<long> {
	public override long Execute(long t) {
		switch (type) {
			case VariableDecoratorType.Add: return t + value;
			case VariableDecoratorType.Sub: return t - value;
			case VariableDecoratorType.Mul: return t * value;
			case VariableDecoratorType.Div: return t / (value == 0 ? 1L : value);
		}

		return t;
	}
}

[Serializable]
public class FloatDecorator : VariableDecorator<float> {
	public override float Execute(float t) {
		switch (type) {
			case VariableDecoratorType.Add: return t + value;
			case VariableDecoratorType.Sub: return t - value;
			case VariableDecoratorType.Mul: return t * value;
			case VariableDecoratorType.Div: return t / (value == 0f ? 1f : value);
		}

		return t;
	}
}