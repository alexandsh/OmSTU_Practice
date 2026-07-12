namespace task13tests;

using Xunit;
using task13;

public class JSONServiceTests
{
    [Fact]
    public void SerializeReturnsJsonString()
    {
        var student = new Student
        {
            FirstName = "Alex",
            LastName = "Shchelkanov",
            BirthDate = new DateTime(2007, 1, 29),
            Grades = new List<Subject> { new Subject { Name = "Programming", Grade = 5 } }
        };

        var json = JSONService.Serialize(student);

        Assert.Contains("Alex", json);
        Assert.Contains("01-2007-29", json);
    }

    [Fact]
    public void DeserializeReturnsStudent()
    {
        var json = @"{
          ""FirstName"": ""Alex"",
          ""LastName"": ""Shchelkanov"",
          ""BirthDate"": ""01-2007-29"",
          ""Grades"": [{ ""Name"": ""Programming"", ""Grade"": 5 }]
        }";
        var student = JSONService.Deserialize(json);

        Assert.Equal("Alex", student.FirstName);
        Assert.Equal(new DateTime(2007, 1, 29), student.BirthDate);
        Assert.Equal("Programming", student.Grades[0].Name);
    }

    [Fact]
    public void SaveToFileCreatesFile()
    {
        var student = new Student
        {
            FirstName = "Alex",
            LastName = "Shchelkanov",
            BirthDate = new DateTime(2007, 1, 29),
            Grades = new List<Subject> { new Subject { Name = "Programming", Grade = 5 } }
        };
        var path = "./testFile.json";

        JSONService.SaveToFile(student, path);
        var json = File.ReadAllText(path);

        Assert.Contains("Alex", json);
        Assert.Contains("Shchelkanov", json);
        Assert.Contains("01-2007-29", json);
        Assert.Contains("Programming", json);
        File.Delete(path);
    }
    [Fact]
    public void LoadFromFileReturnsStudent()
    {
        var student = new Student
        {
            FirstName = "Alex",
            LastName = "Shchelkanov",
            BirthDate = new DateTime(2007, 1, 29),
            Grades = new List<Subject> { new Subject { Name = "Programming", Grade = 5 } }
        };
        var path = "./testFile.json";

        JSONService.SaveToFile(student, path);
        var studentLoad = JSONService.LoadFromFile(path);

        Assert.Equal("Alex", studentLoad.FirstName);
        Assert.Equal("Shchelkanov", studentLoad.LastName);
        Assert.Equal("Programming", studentLoad.Grades[0].Name);
        Assert.Equal(new DateTime(2007, 1, 29), studentLoad.BirthDate);
        File.Delete(path);
    }
}
