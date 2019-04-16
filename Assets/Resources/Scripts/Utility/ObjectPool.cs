using System.Collections.Generic;

public class ObjectPool<T> where T : class, new() {

    private Queue<T> _idleObjects;

    public ObjectPool() : this(2) { }

    public ObjectPool(int defaultCapacity) => _idleObjects = new Queue<T>(defaultCapacity);

    public T GetObject() {
        if (_idleObjects.Count > 0) return _idleObjects.Dequeue();
        else return GenerateObject();
    }

    private T GenerateObject() => new T();

    public void Recycle(T @object) => _idleObjects.Enqueue(@object);
}