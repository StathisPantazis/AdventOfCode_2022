using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_7 {
    public static void Solve() {
        string text = Helpers.File_CleanReadText(7).Replace("$ ls", "").Replace("$ cd /", "");
        string[] inputs = Helpers.Text_CleanReadLines(text).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        Dir dir = CreateDirFromInputs(inputs);

        int sum = dir.Part1_SumDir();
        Console.WriteLine($"Part 1: {sum}");

        int size = Math.Abs(70000000 - dir.Sum - 30000000);
        int fileToDelete = dir.Part2_DeleteBest(size, int.MaxValue);
        Console.WriteLine($"Part 2: {fileToDelete}");
    }

    public static int Part1_SumDir(this Dir dir) {
        int sum = dir.Sum <= 100000 ? dir.Sum : 0;
        dir.Dirs.ForEach(d => sum += d.Part1_SumDir());
        return sum;
    }

    public static int Part2_DeleteBest(this Dir dir, int size, int closest) {
        int best = dir.Sum > size && dir.Sum < closest ? dir.Sum : closest;
        dir.Dirs.ForEach(d => best = d.Part2_DeleteBest(size, best));
        return best;
    }

    private static Dir CreateDirFromInputs(string[] inputs) {
        Dir dir = new("/", null);

        foreach (string line in inputs) {
            if (line.StartsWith("dir")) {
                Dir newDir = new(line.Split(' ')[1], dir);
                dir.Dirs.Add(newDir);
            }
            else if (char.IsDigit(line[0])) {
                string[] l = line.Split(' ');
                string[] file = l[1].Split('.');
                dir.AddFile(new MyFile(file[0], file.Length > 1 ? file[1] : "", int.Parse(l[0])));
            }
            else if (line.StartsWith("$ cd ..")) {
                dir = dir.Parent;
            }
            else if (line.StartsWith("$ cd")) {
                dir = dir.Dirs.FirstOrDefault(x => x.Name == line.Split(' ').Last());
            }
        }

        while (dir.Name != "/") {
            dir = dir.Parent;
        }

        return dir;
    }

    public class Dir {
        public Dir(string name, Dir parent) {
            Name = name;
            Parent = parent;
        }

        public Dir(string[] inputs) {

        }

        public string Name { get; set; }
        public int Sum { get; set; }
        public Dir Parent { get; set; }
        public List<Dir> Dirs { get; set; } = new();
        public List<MyFile> Files { get; set; } = new();

        public void AddFile(MyFile file) {
            Files.Add(file);
            Sum += file.Size;

            Dir parent = Parent;
            while (parent != null) {
                parent.Sum += file.Size;
                parent = parent.Parent;
            }
        }

        public override string ToString()
            => $"{Name} - Dirs:{Dirs.Count}[{string.Join(' ', Dirs.Select(x => x.Name))}] - Files:{Files.Count}";
    }

    public class MyFile {
        public MyFile(string name, string myType, int size) {
            MyType = myType;
            Size = size;
            Name = name;
        }

        public string Name { get; set; }
        public string MyType { get; set; }
        public int Size { get; set; }
        public override string ToString()
            => $"{Name}.{MyType} - Size:{Size}";
    }
}
