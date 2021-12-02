namespace FM21.Core.Model
{
    public class ProjectMasterModel
    {
        public int ProjectId { get; set; }
        public int ProjectCode { get; set; }
        public string NPICode { get; set; }
        public string ProjectDescription { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
       
    }
}