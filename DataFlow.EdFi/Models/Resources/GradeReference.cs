using System;

namespace DataFlow.EdFi.Models.Resources 
{
    public class GradeReference 
    {
        /// <summary>
        /// The name of the grading period during the school year in which the grade is offered (e.g., 1st cycle, 1st semester)
        /// </summary>
        public string gradingPeriodDescriptor { get; set; }

        /// <summary>
        /// Month, day, and year of the first day of the grading period.
        /// </summary>
        public DateTime? gradingPeriodBeginDate { get; set; }

        /// <summary>
        /// The type of grade (e.g., Exam, Final, Grading Period, Progress Report)
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// A unique alpha-numeric code assigned to a student.
        /// </summary>
        public string studentUniqueId { get; set; }

        /// <summary>
        /// School Identity Column  
        /// </summary>
        public int? schoolId { get; set; }

        /// <summary>
        /// An indication of the portion of a typical daily session in which students receive instruction in a specified subject (e.g., morning, sixth period, block period or AB schedules).  =
        /// </summary>
        public string classPeriodName { get; set; }

        /// <summary>
        /// A unique number or alphanumeric code assigned to a room by a school, school system, state, or other agency or entity.
        /// </summary>
        public string classroomIdentificationCode { get; set; }

        /// <summary>
        /// The local code assigned by the LEA or Campus that identifies the organization of subject matter and related learning experiences provided for the instruction of students.
        /// </summary>
        public string localCourseCode { get; set; }

        /// <summary>
        /// A unique identifier for the section, that is defined for a campus by the classroom, the subjects taught, and the instructors that are assigned.  NEDM: Unique Course Code
        /// </summary>
        public string uniqueSectionCode { get; set; }

        /// <summary>
        /// When a section is part of a sequence of parts for a course, the number if the sequence.  If the course has only onle part, the value of this section attribute should be 1.
        /// </summary>
        public int? sequenceOfCourse { get; set; }

        /// <summary>
        /// The identifier for the school year.
        /// </summary>
        public int? schoolYear { get; set; }

        /// <summary>
        /// A unique identifier used as Primary Key, not derived from business logic, when acting as Foreign Key, references the parent table.
        /// </summary>
        public string termDescriptor { get; set; }

        /// <summary>
        /// Month, day, and year of the Student's entry or assignment to the Section.
        /// </summary>
        public DateTime? beginDate { get; set; }

        /// <summary>
        /// Represents a hyperlink to the related grade resource.
        /// </summary>
        public Link link { get; set; }

        }
}

