public interface IInputDetectable {

	string Name {
		get;
	}

	bool IsPressed {
		get;
	}

	bool IsHeld {
		get;
	}

	bool IsReleased {
		get;
	}
	
	float ChargeTime {
		get;
	}

	void Refresh();
}