namespace SamNetMvc.Components
{
    public class PresenterModel : BlogPost
    {
        public BlogPost Item { get; set; }
        public BlogPost LastEdited { get; set; }
        public int DeletedItemId { get; set; } = 0;
    }
}