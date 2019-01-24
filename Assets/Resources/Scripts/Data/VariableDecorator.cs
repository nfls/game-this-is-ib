using System;

[Serializable]
public abstract class VariableDecorator<T> where T : struct {

	public string type;
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
public abstract class IntDecorator : VariableDecorator<int> { }

[Serializable]
public class IntAdd : IntDecorator {
	public IntAdd() { type = "Add"; }
	public override int Execuate(int f) => f + value;
}

[Serializable]
public class IntSub : IntDecorator {
	public IntSub() { type = "Sub"; }
	public override int Execuate(int f) => f - value;
}

[Serializable]
public class IntMul : IntDecorator {
	public IntMul() { type = "Mul"; }
	public override int Execuate(int f) => f * value;
}

[Serializable]
public class IntDiv : IntDecorator {
	public IntDiv() { type = "Div"; }
	public override int Execuate(int f) => f / value;
}

[Serializable]
public abstract class LongDecorator : VariableDecorator<long> { }

[Serializable]
public class LongAdd : LongDecorator {
	public LongAdd() { type = "Add"; }
	public override long Execuate(long f) => f + value;
}

[Serializable]
public class LongSub : LongDecorator {
	public LongSub() { type = "Sub"; }
	public override long Execuate(long f) => f - value;
}

[Serializable]
public class LongMul : LongDecorator {
	public LongMul() { type = "Mul"; }
	public override long Execuate(long f) => f * value;
}

[Serializable]
public class LongDiv : LongDecorator {
	public LongDiv() { type = "Div"; }
	public override long Execuate(long f) => f / value;
}

[Serializable]
public abstract class FloatDecorator : VariableDecorator<float> { }

[Serializable]
public class FloatAdd : FloatDecorator {
	public FloatAdd() { type = "Add"; }
	public override float Execuate(float f) => f + value;
}

[Serializable]
public class FloatSub : FloatDecorator {
	public FloatSub() { type = "Sub"; }
	public override float Execuate(float f) => f - value;
}

[Serializable]
public class FloatMul : FloatDecorator {
	public FloatMul() { type = "Mul"; }
	public override float Execuate(float f) => f * value;
}

[Serializable]
public class FloatDiv : FloatDecorator {
	public FloatDiv() { type = "Div"; }
	public override float Execuate(float f) => f / value;
}