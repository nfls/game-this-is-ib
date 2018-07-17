using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Subject {

	public static readonly Subject ChineseSL = new Subject("Chinese", "CHI", SubjectGroup.LanguageAndLiterature, SubjectLevel.StandardLevel);
	public static readonly Subject EnglishSL = new Subject("English", "ENG", SubjectGroup.LanguageAcquistion, SubjectLevel.StandardLevel);
	public static readonly Subject EconomicsSL = new Subject("Economics", "ECO", SubjectGroup.SocialSciences, SubjectLevel.StandardLevel);
	public static readonly Subject BusinessSL = new Subject("Business", "BUS", SubjectGroup.SocialSciences, SubjectLevel.StandardLevel);
	public static readonly Subject HistorySL = new Subject("History", "HIS", SubjectGroup.SocialSciences, SubjectLevel.StandardLevel);
	public static readonly Subject ComputerScienceSL = new Subject("Computer Science", "CS", SubjectGroup.ExperimentalSciences, SubjectLevel.StandardLevel);
	public static readonly Subject PhysicsSL = new Subject("Physics", "PHY", SubjectGroup.ExperimentalSciences, SubjectLevel.StandardLevel);
	public static readonly Subject ChemistrySL = new Subject("Chemistry", "CHE", SubjectGroup.ExperimentalSciences, SubjectLevel.StandardLevel);
	public static readonly Subject BiologySL = new Subject("Biology", "BIO", SubjectGroup.ExperimentalSciences, SubjectLevel.StandardLevel);
	public static readonly Subject MathematicsSL = new Subject("Mathematics", "MAT", SubjectGroup.Mathematics, SubjectLevel.StandardLevel);
	public static readonly Subject MusicSL = new Subject("Music", "MUS", SubjectGroup.Arts, SubjectLevel.StandardLevel);
	public static readonly Subject VisualArtsSL = new Subject("Visual Arts", "VA", SubjectGroup.Arts, SubjectLevel.StandardLevel);
	
	public static readonly Subject ChineseHL = new Subject("Chinese", "CHI", SubjectGroup.LanguageAndLiterature, SubjectLevel.HighLevel);
	public static readonly Subject EnglishHL = new Subject("English", "ENG", SubjectGroup.LanguageAcquistion, SubjectLevel.HighLevel);
	public static readonly Subject EconomicsHL = new Subject("Economics", "ECO", SubjectGroup.SocialSciences, SubjectLevel.HighLevel);
	public static readonly Subject BusinessHL = new Subject("Business", "BUS", SubjectGroup.SocialSciences, SubjectLevel.HighLevel);
	public static readonly Subject HistoryHL = new Subject("History", "HIS", SubjectGroup.SocialSciences, SubjectLevel.HighLevel);
	public static readonly Subject ComputerScienceHL = new Subject("Computer Science", "CS", SubjectGroup.ExperimentalSciences, SubjectLevel.HighLevel);
	public static readonly Subject PhysicsHL = new Subject("Physics", "PHY", SubjectGroup.ExperimentalSciences, SubjectLevel.HighLevel);
	public static readonly Subject ChemistryHL = new Subject("Chemistry", "CHE", SubjectGroup.ExperimentalSciences, SubjectLevel.HighLevel);
	public static readonly Subject BiologyHL = new Subject("Biology", "BIO", SubjectGroup.ExperimentalSciences, SubjectLevel.HighLevel);
	public static readonly Subject MathematicsHL = new Subject("Mathematics", "MAT", SubjectGroup.Mathematics, SubjectLevel.HighLevel);
	public static readonly Subject MusicHL = new Subject("Music", "MUS", SubjectGroup.Arts, SubjectLevel.HighLevel);
	public static readonly Subject VisualArtsHL = new Subject("Visual Arts", "VA", SubjectGroup.Arts, SubjectLevel.HighLevel);
	
	public static readonly Subject CreativityActivityService = new Subject("Creativity Activity Service", "CAS", SubjectGroup.Other);
	public static readonly Subject ExtendedEssay = new Subject("Extended Essay", "EE", SubjectGroup.Other);
	public static readonly Subject TheoryOfKnowledge = new Subject("Theory Of Knowledge", "TOK", SubjectGroup.Other);

	public static string[] Names {
		get { return subjects.Keys.ToArray(); }
	}

	public static Subject[] Subjects {
		get { return subjects.Values.ToArray(); }
	}

	public static int Count {
		get { return subjects.Count; }
	}

	private static readonly Dictionary<string, Subject> subjects;

	public readonly string name;
	public readonly string shortName;             
	public readonly SubjectGroup group;
	public readonly SubjectLevel level;
	public readonly bool isOptional;

	#region
	static Subject() {
		subjects = new Dictionary<string, Subject>(27);
		
		subjects["ChineseSL"] = ChineseSL;
		subjects["EnglishSL"] = EnglishSL;
		subjects["EconomicsSL"] = EconomicsSL;
		subjects["BusinessSL"] = BusinessSL;
		subjects["HistorySL"] = HistorySL;
		subjects["ComputerScienceSL"] = ComputerScienceSL;
		subjects["PhysicsSL"] = PhysicsSL;
		subjects["ChemistrySL"] = ChemistrySL;
		subjects["BiologySL"] = BiologySL;
		subjects["MathematicsSL"] = MathematicsSL;
		subjects["MusicSL"] = MusicSL;
		subjects["VisualArtsSL"] = VisualArtsSL;
		
		subjects["ChineseHL"] = ChineseHL;
		subjects["EnglishHL"] = EnglishHL;
		subjects["EconomicsHL"] = EconomicsHL;
		subjects["BusinessHL"] = BusinessHL;
		subjects["HistoryHL"] = HistoryHL;
		subjects["ComputerScienceHL"] = ComputerScienceHL;
		subjects["PhysicsHL"] = PhysicsHL;
		subjects["ChemistryHL"] = ChemistryHL;
		subjects["BiologyHL"] = BiologyHL;
		subjects["MathematicsHL"] = MathematicsHL;
		subjects["MusicHL"] = MusicHL;
		subjects["VisualArtsHL"] = VisualArtsHL;

		subjects["CreativityActivityService"] = CreativityActivityService;
		subjects["ExtendedEssay"] = ExtendedEssay;
		subjects["TheoryOfKnowledge"] = TheoryOfKnowledge;
	}
	#endregion

	private Subject(string name, string shortName, SubjectGroup group, bool isOptional = false) {
		this.name = name;
		this.shortName = shortName;
		this.group = group;
		this.isOptional = isOptional;
	}

	private Subject(string name, string shortName, SubjectGroup group, SubjectLevel level, bool isOptional = true) : this(name, shortName, group, isOptional) {
		this.level = level;
	}

	public static Subject ToSubject(string name) {
		return subjects[name];
	}
}

[AttributeUsage(AttributeTargets.Field)]
public class SubjectAttribute : PropertyAttribute { }

public enum SubjectGroup {
	LanguageAndLiterature = 1,
	LanguageAcquistion = 2,
	SocialSciences = 3,
	ExperimentalSciences = 4,
	Mathematics = 5,
	Arts = 6,
	Other = 7
}

public enum SubjectLevel {
	StandardLevel,
	HighLevel
}