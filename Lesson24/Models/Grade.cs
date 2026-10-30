namespace College_DB.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public int? TeacherId { get; set; }
        public int? GradeValue { get; set; }
        public DateTime? GradeDate { get; set; }
    }
}