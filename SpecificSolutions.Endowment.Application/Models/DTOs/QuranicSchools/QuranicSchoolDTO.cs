namespace SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools
{
    public class QuranicSchoolDTO
    {
        public int QuranicSchoolID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ContactInfo { get; set; }
        public int NumberOfStudents { get; set; }
        public string Status { get; set; }
        public Guid Id { get; set; }
    }
}