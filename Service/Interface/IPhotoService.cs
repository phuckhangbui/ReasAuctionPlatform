using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Service.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file, int idReas, string nameReas, string nameOwner);
        Task<ImageUploadResult> AddPhotoNewsAsync(IFormFile file, string newsNames);
        Task<DeletionResult> DeletePhotoAsync(string id);
    }
}
