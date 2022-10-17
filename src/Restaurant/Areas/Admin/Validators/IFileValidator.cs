namespace Restaurant.Areas.Admin.Validators
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile? file);
    }
}
