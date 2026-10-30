namespace College_DB.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int? TeacherId { get; set; }
    }
}