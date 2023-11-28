using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2022.Day_7;

namespace AdventOfCode.Version2022;

public class Day_7 : AoCBaseDay<int, int, Dir>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_CleanReadText(7, 2022, resourceType)
            .Replace("$ ls", "")
            .Replace("$ cd /", "");

        var inputs = Helpers.Text_CleanReadLines(text)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        var dir = CreateDirFromInputs(inputs);

        return new AoCSolution<int, int>(Part1(dir), Part2(dir));
    }

    protected override int Part1(Dir dir)
    {
        return Part1_SumDir(dir);
    }

    protected override int Part2(Dir dir)
    {
        var size = Math.Abs(70000000 - dir.Sum - 30000000);
        return Part2_DeleteBest(dir, size, int.MaxValue);
    }

    public static int Part1_SumDir(Dir dir)
    {
        var sum = dir.Sum <= 100000 ? dir.Sum : 0;
        dir.Dirs.ForEach(d => sum += Part1_SumDir(d));
        return sum;
    }

    public static int Part2_DeleteBest(Dir dir, int size, int closest)
    {
        var best = dir.Sum > size && dir.Sum < closest ? dir.Sum : closest;
        dir.Dirs.ForEach(d => best = Part2_DeleteBest(d, size, best));
        return best;
    }

    private static Dir CreateDirFromInputs(string[] inputs)
    {
        var dir = new Dir("/", null);

        foreach (var line in inputs)
        {
            if (line.StartsWith("dir"))
            {
                var newDir = new Dir(line.Split(' ')[1], dir);
                dir.Dirs.Add(newDir);
            }
            else if (char.IsDigit(line[0]))
            {
                var l = line.Split(' ');
                var file = l[1].Split('.');
                dir.AddFile(new MyFile(file[0], file.Length > 1 ? file[1] : "", int.Parse(l[0])));
            }
            else if (line.StartsWith("$ cd .."))
            {
                dir = dir.Parent;
            }
            else if (line.StartsWith("$ cd"))
            {
                dir = dir.Dirs.FirstOrDefault(x => x.Name == line.Split(' ').Last());
            }
        }

        while (dir.Name != "/")
        {
            dir = dir.Parent;
        }

        return dir;
    }

    public class Dir
    {
        public Dir(string name, Dir parent)
        {
            Name = name;
            Parent = parent;
        }

        public string Name { get; set; }
        public int Sum { get; set; }
        public Dir Parent { get; set; }
        public List<Dir> Dirs { get; set; } = new();
        public List<MyFile> Files { get; set; } = new();

        public void AddFile(MyFile file)
        {
            Files.Add(file);
            Sum += file.Size;

            var parent = Parent;
            while (parent != null)
            {
                parent.Sum += file.Size;
                parent = parent.Parent;
            }
        }

        public override string ToString()
            => $"{Name} - Dirs:{Dirs.Count}[{string.Join(' ', Dirs.Select(x => x.Name))}] - Files:{Files.Count}";
    }

    public class MyFile
    {
        public MyFile(string name, string myType, int size)
        {
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
