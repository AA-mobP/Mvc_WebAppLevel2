﻿namespace Mvc_WebApp_Level2.Models.BusinessLogic
{
    public class clsCourse
    {
        private static AppDbContext context;
        private List<Course> courses;
        int index;
        public clsCourse()
        {
            context = new AppDbContext();
            courses = context.courses.ToList();
        }

        public List<Course> GetAll()
        {
            return courses;
        }

        public Course GetOne(int id)
        {
            index = courses.FindIndex(m => m.Id == id);
            if (index != -1)
                return courses[index];
            return new Course();
        }

        public int? GetPreviousId()
        {
            if (index > 0)
                return courses[index - 1].Id;
            return null;
        }

        public int? GetNextId()
        {
            if (index < courses.Count - 1)
                return courses[index + 1].Id;
            return null;
        }

        public Course? Find(int id) => context.courses.FirstOrDefault(c => c.Id == id);

        public void Add(Course course)
        {
            context.courses.Add(course);
            context.SaveChanges();
        }

        public void Edit(Course course)
        {
            Course? OldCourse = Find(course.Id);
            if (OldCourse != null)
            {
                OldCourse.Name = course.Name;
                OldCourse.MinDegree = course.MinDegree;
                OldCourse.MaxDegree = course.MinDegree;
                OldCourse.DeptId = course.DeptId;
                OldCourse.IctorId = course.IctorId;
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Course? course = Find(id);
            if (course != null)
            {
                context.courses.Remove(course);
                context.SaveChanges();
            }
        }
    }
}
