namespace task13;

﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Subject
{
    public required string Name { get; set; }
    public int Grade { get; set; }
}

public class Student
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public  DateTime BirthDate { get; set; }
    public required List<Subject> Grades { get; set; }
}


public class JSONService
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            DateTime.ParseExact(reader.GetString()!, "MM-yyyy-dd", null);

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString("MM-yyyy-dd", null));
    }

    public static string Serialize(Student student)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, Converters = { new CustomDateTimeConverter() }, WriteIndented = true };
        return JsonSerializer.Serialize(student, options);
    }

    public static Student Deserialize(string json)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, Converters = { new CustomDateTimeConverter() }, WriteIndented = true };
        return JsonSerializer.Deserialize<Student>(json, options)!;
    }

    public static void SaveToFile(Student student, string path)
    {
        var jsonString = Serialize(student);
        File.WriteAllText(path, jsonString);
    }

    public static Student LoadFromFile(string path)
    {
        var content = File.ReadAllText(path);
        var student = Deserialize(content);
        return student;
    }
}
