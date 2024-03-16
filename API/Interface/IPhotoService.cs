using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file, int idReas, string nameReas, string nameOwner);
        Task<ImageUploadResult> AddPhotoNewsAsync(IFormFile file, string newsNames);
        Task<DeletionResult> DeletePhotoAsync(string id);
    }
}
