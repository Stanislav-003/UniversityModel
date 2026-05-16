using UniversityModel.Models;

namespace UniversityModel.Helpers;

public static class MockDataService
{
    private static readonly Guid Teacher1Id = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid Teacher2Id = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid Teacher3Id = Guid.Parse("10000000-0000-0000-0000-000000000003");

    private static readonly Guid MathCourseId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid PhysicsCourseId = Guid.Parse("20000000-0000-0000-0000-000000000002");
    private static readonly Guid ChemistryCourseId = Guid.Parse("20000000-0000-0000-0000-000000000003");

    private static readonly Guid Student1Id = Guid.Parse("30000000-0000-0000-0000-000000000001");
    private static readonly Guid Student2Id = Guid.Parse("30000000-0000-0000-0000-000000000002");

    public static List<Teatcher> GetTestTeachers()
    {
        return new List<Teatcher>
        {
            new Teatcher
            {
                Id = Teacher1Id,
                FirstName = "John",
                LastName = "Smith",
                Gender = "Male",
                Age = 40,
            },
            new Teatcher
            {
                Id = Teacher2Id,
                FirstName = "Anna",
                LastName = "Johnson",
                Gender = "Female",
                Age = 35,
            },
            new Teatcher
            {
                Id = Teacher3Id,
                FirstName = "Paul",
                LastName = "Roland",
                Gender = "Male",
                Age = 35,
            }
        };
    }

    public static List<Course> GetTestCourses()
    {
        return new List<Course>
        {
            new Course
            {
                Id = MathCourseId,
                Name = "Math",
                TeacherId = Teacher1Id
            },
            new Course
            {
                Id = PhysicsCourseId,
                Name = "Physics",
                TeacherId = Teacher2Id
            },
            new Course
            {
                Id = ChemistryCourseId,
                Name = "Chemistry",
                TeacherId = Teacher3Id
            }
        };
    }

    public static List<Student> GetTestStudents()
    {
        return new List<Student>
        {
            new Student
            {
                Id = Student1Id,
                FirstName = "Alice",
                LastName = "Brown",
                Gender = "Female",
                Age = 20,
                CourseIds = new List<Guid>
                {
                    PhysicsCourseId
                }
            },
            new Student
            {
                Id = Student2Id,
                FirstName = "Bob",
                LastName = "White",
                Gender = "Male",
                Age = 22,
                CourseIds = new List<Guid>
                {
                    ChemistryCourseId
                }
            }
        };
    }
}