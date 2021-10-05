namespace IdentityServerHost.Pages.PAT
{
    public class ViewModel
    {
        public int LifetimeDays { get; set; } = 365;
        public bool IsReferenceToken { get; set; } = true;

        public bool ForApi1 { get; set; } = true;
        public bool ForApi2 { get; set; }
    }
}