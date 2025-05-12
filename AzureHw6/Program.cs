using AzureHw6;
using Microsoft.EntityFrameworkCore;

void MainMenu()
{
    Console.WriteLine("1. Add Student");
    Console.WriteLine("2. Add Course");
    Console.WriteLine("3. Enroll Student in Course");
    Console.WriteLine("4. List Students with Courses");
    Console.WriteLine("5. Course Statistics");
    Console.WriteLine("6. Students with Grade > 90");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
}

void AddStudent(StudentDbContext db)
{
    Console.Write("\nEnter first name: ");
    string firstName = Console.ReadLine();

    Console.Write("Enter last name: ");
    string lastName = Console.ReadLine();

    Console.Write("Enter email: ");
    string email = Console.ReadLine();

    var student = new Student { FirstName = firstName, LastName = lastName, Email = email };
    db.Students.Add(student);
    db.SaveChanges();

    Console.WriteLine($"Student {firstName} {lastName} added");
}

void AddCourse(StudentDbContext db)
{
    Console.Write("\nEnter course name: ");
    string courseName = Console.ReadLine();

    Console.Write("Enter credits: ");
    int credits = int.Parse(Console.ReadLine());

    var course = new Course { CourseName = courseName, Credits = credits };
    db.Courses.Add(course);
    db.SaveChanges();

    Console.WriteLine($"Course {courseName} added");
}

void EnrollStudent(StudentDbContext db)
{
    Console.Write("\nEnter student ID: ");
    int studentId = int.Parse(Console.ReadLine());

    Console.Write("Enter course ID: ");
    int courseId = int.Parse(Console.ReadLine());

    Console.Write("Enter grade: ");
    int grade = int.Parse(Console.ReadLine());

    var enrollment = new Enrollment { StudentId = studentId, CourseId = courseId, Grade = grade };
    db.Enrollments.Add(enrollment);
    db.SaveChanges();

    Console.WriteLine("Student enrolled");
}

void ListStudentsWithCourses(StudentDbContext db)
{
    var students = db.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).ToList();
    foreach (var student in students)
    {
        Console.WriteLine($"\nStudent: {student.FirstName} {student.LastName}");
        foreach (var enrollment in student.Enrollments)
        {
            Console.WriteLine($" - Course: {enrollment.Course.CourseName}, Grade: {enrollment.Grade}");
        }
    }
}

void CourseStatistics(StudentDbContext db)
{
    var courses = db.Courses.Include(c => c.Enrollments).ToList();
    foreach (var course in courses)
    {
        Console.WriteLine($"\nCourse: {course.CourseName}, Students Enrolled: {course.Enrollments.Count}");
    }
}

void StudentsWithHighGrades(StudentDbContext db)
{
    var students = db.Enrollments.Where(e => e.Grade > 90).Include(e => e.Student).Include(e => e.Course).ToList();
    foreach (var enrollment in students)
    {
        Console.WriteLine($"{enrollment.Student.FirstName} {enrollment.Student.LastName} - {enrollment.Course.CourseName}, Grade: {enrollment.Grade}");
    }
}

using (var db = new StudentDbContext())
{
    int choice = -1;

    while (choice != 0)
    {
        MainMenu();
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("Invalid choice");
            continue;
        }

        switch (choice)
        {
            case 1: AddStudent(db); break;
            case 2: AddCourse(db); break;
            case 3: EnrollStudent(db); break;
            case 4: ListStudentsWithCourses(db); break;
            case 5: CourseStatistics(db); break;
            case 6: StudentsWithHighGrades(db); break;
            case 0: Console.WriteLine("Exiting..."); break;
            default: Console.WriteLine("Invalid choice"); break;
        }
    }
}

