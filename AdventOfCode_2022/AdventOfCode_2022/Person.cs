using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal class Person : Gridable {
	public Person(string text) {
		Initials = $"{text.Split('_')[0].ToUpper()}{text.Split('_')[1].ToUpper()}";
	}

	public string Initials { get; set; }

	public override string ToString() => Initials;
}
