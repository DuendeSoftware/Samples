namespace IdentityServerHost.Pages.PAT
{
    public class ViewModel
    {
        public int LifetimeDays { get; set; } = 365;
        public bool IsReferenceToken { get; set; }

        public bool ForScope1 { get; set; } = true;
        public bool ForScope2 { get; set; }
    }
}