public interface IInputDetector {

	string Name {
		get;
	}

	bool IsPressed {
		get;
	}

	bool IsDoublePressed {
		get;
	}

	bool IsHeld {
		get;
	}

	bool IsReleased {
		get;
	}

	bool IsCharged {
		get;
	}

	float ChargeTime {
		get;
	}

	void Refresh();
}