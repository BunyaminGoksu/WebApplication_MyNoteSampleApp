namespace WebApplication_MyNoteSampleApp.Business
{
    public class ServiceResult
    {
        public bool IsError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        
        public void AddError(string errorMessage)
        {
            IsError= true;
            Errors.Add(errorMessage);
        }
    }
}
