public interface IInputMapper {

	KeyDetector this[InputMap map] {
		get;
		set;
	}

	void Refresh();
	void Remap(InputMap toMap, string input);
	void Reset();
}