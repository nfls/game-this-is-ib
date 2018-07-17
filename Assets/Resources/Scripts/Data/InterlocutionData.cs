using System;
using UnityEngine;

[Serializable]
public class InterlocutionData {
	
	public const string DIPLOMA = "Diploma";
	public static readonly string[] Keys;
	
	static InterlocutionData() {
		Keys = new string[Subject.Count + 1];
		string[] names = Subject.Names;
		for (int i = 0, l = Subject.Count; i < l; i++) {
			Keys[i] = names[i];
		}

		Keys[Keys.Length - 1] = DIPLOMA;
	}

	public string question;
	public string optionA;
	public string optionB;
	public string optionC;
	public string optionD;
	public Option answer;
}

public enum Option {
	A,
	B,
	C,
	D
}

[AttributeUsage(AttributeTargets.Field)]
public class InterlocutionSubjectNameAttribute : PropertyAttribute { }