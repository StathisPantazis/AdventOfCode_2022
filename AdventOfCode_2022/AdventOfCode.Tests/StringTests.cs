using AdventOfCode.Core.Extensions;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class StringTests
{
    [Fact]
    public void Should_crop_correctly()
    {
        var str = "12345";

        // Crop From
        str.CropFrom("6", out var stringChanged, out var charsCropped).Should().Be(str);
        stringChanged.Should().BeFalse();
        charsCropped.Should().Be(0);

        str.CropFrom(str, out stringChanged, out charsCropped).Should().Be(str);
        stringChanged.Should().BeFalse();
        charsCropped.Should().Be(0);

        str.CropFrom("3", out stringChanged, out charsCropped).Should().Be("345");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);

        str.CropFrom("34", out stringChanged, out charsCropped).Should().Be("345");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);

        str.CropFrom('3', out stringChanged, out charsCropped).Should().Be("345");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);

        str.CropFrom(2, out stringChanged, out charsCropped).Should().Be("345");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);

        // Crop From Without
        str.CropFromWithout("6", out stringChanged, out charsCropped).Should().Be(str);
        stringChanged.Should().BeFalse();
        charsCropped.Should().Be(0);

        str.CropFromWithout(str, out stringChanged, out charsCropped).Should().Be(str);
        stringChanged.Should().BeFalse();
        charsCropped.Should().Be(0);

        str.CropFromWithout("3", out stringChanged, out charsCropped).Should().Be("45");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        str.CropFromWithout("23", out stringChanged, out charsCropped).Should().Be("45");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        str.CropFromWithout('3', out stringChanged, out charsCropped).Should().Be("45");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        str.CropFromWithout(2, out stringChanged, out charsCropped).Should().Be("45");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        // Crop Until
        str.CropUntil("6", out stringChanged, out charsCropped).Should().Be(str);
        stringChanged.Should().BeFalse();
        charsCropped.Should().Be(0);

        str.CropUntil(str, out stringChanged, out charsCropped).Should().Be("");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(5);

        str.CropUntil("3", out stringChanged, out charsCropped).Should().Be("12");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        str.CropUntil("34", out stringChanged, out charsCropped).Should().Be("12");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        str.CropUntil('3', out stringChanged, out charsCropped).Should().Be("12");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        str.CropUntil(2, out stringChanged, out charsCropped).Should().Be("12");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(3);

        // Crop Until With
        str.CropUntilWith("6", out stringChanged, out charsCropped).Should().Be(str);
        stringChanged.Should().BeFalse();
        charsCropped.Should().Be(0);

        str.CropUntilWith(str, out stringChanged, out charsCropped).Should().Be(str);
        stringChanged.Should().BeFalse();
        charsCropped.Should().Be(0);

        str.CropUntilWith("3", out stringChanged, out charsCropped).Should().Be("123");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);

        str.CropUntilWith("23", out stringChanged, out charsCropped).Should().Be("123");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);

        str.CropUntilWith('3', out stringChanged, out charsCropped).Should().Be("123");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);

        str.CropUntilWith(2, out stringChanged, out charsCropped).Should().Be("123");
        stringChanged.Should().BeTrue();
        charsCropped.Should().Be(2);
    }

    [Fact]
    public void Should()
    {

    }
}
