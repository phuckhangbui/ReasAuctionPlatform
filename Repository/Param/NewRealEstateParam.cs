using Repository.DTOs;

namespace Repository.Param
{
    public class NewRealEstateParam
    {
        public string ReasName { get; set; }
        public string ReasAddress { get; set; }
        public int ReasPrice { get; set; }
        public int ReasArea { get; set; }
        public string ReasDescription { get; set; }
        public DateTime DateEnd { get; set; }
        public int Type_Reas { get; set; }
        public List<PhotoFileDto> Photos { get; set; }

        public DetailFileReasDto Detail { get; set; }

        public int OldReasId { get; set; }
    }
}
