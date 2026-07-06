namespace task02;

using System;
using System.Linq;

public class Student
{
    public string Name { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public List<int> Grades { get; set; } = new(); 
}
public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        =>  _students.Where(s => s.Faculty == faculty);

    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade) 
        => _students.Where(s => s.Grades.DefaultIfEmpty().Average() >= minAverageGrade);

    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(s => s.Name);

    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(s => s.Faculty);

    public string GetFacultyWithHighestAverageGrade()
        => _students.GroupBy(s => s.Faculty)
                    .OrderByDescending(g => g.SelectMany(s => s.Grades).DefaultIfEmpty().Average())
                    .Select(g => g.Key)
                    .FirstOrDefault() ?? string.Empty;
}
