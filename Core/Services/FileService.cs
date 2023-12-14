using System.Net.Mime;
using System.Text;
using Core.Model.ResponseModels;
using Core.Staff;
using Core.ViewModels.Common;
using Core.ViewModels.FileStorage;
using ImageMagick;
using Infrastructure.Data.Contracts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Core.Services;

public class FileService
{
    private static IWebHostEnvironment _environment;
    private readonly IUnitOfWork _unitOfWork;
    public static string BaseImagePath = "Images";
    public static string BasePdfPath = "Pdf";
    
    public FileService(IWebHostEnvironment environment,IUnitOfWork unitOfWork)
    {
        _environment = environment;
        _unitOfWork = unitOfWork;
    }
    
    public static readonly Dictionary<string, string> AllowedFileExtensionsWithMiMeType = new Dictionary<string, string>()
    {
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".png", "image/png"},
        {".pdf", "application/pdf"},
        {".mp4", "video/mp4"}
    };

    private const long AllowedPdfFileLength = 1024 * 1024 * 10;
    private const long AllowedPhotoFileLength = 1024 * 1024 * 1;
    private const long AllowedVideoFileLength = 1024 * 1024 * 20;
    
    
    
    public async Task<BaseDataResponse<UploadFileResponse>> UploadFileUrlAsync(IFormFile file)
    {

        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (!ExtensionAllowed(fileExtension))
            return null; 

        var filename = string.Concat(RandomGenerator.GenerateNewPassword(7) + fileExtension);
        var filePath = Path.Combine(filename);
        var path = Path.Combine(_environment.WebRootPath);
        EnsureDirectoryCreated(path);
        path = Path.Combine(_environment.WebRootPath, filePath);

        var fileStorage = new FileStorage()
        {
            FileName = filename,
            FileLocation = filePath,
            IsPublish = true,
            IsDeleted = false
        };
       
        await _unitOfWork.FileStorages.AddAsync(fileStorage);
        await _unitOfWork.CommitAsync();
       
        using (var stream = File.Create(path))
        {
            await file.CopyToAsync(stream);
        }
        return BaseDataResponse<UploadFileResponse>.Success(new UploadFileResponse(filePath));
    }

    public async Task<BaseDataResponse<UploadFileResponse>> UploadAndSaveResizedImage(IFormFile file, int width , int height)
{
    if (file == null || file.Length == 0)
    {
        return BaseDataResponse<UploadFileResponse>.Fail("File is empty or null");
    }
    using (var imageStream = file.OpenReadStream())
    {
        using (var image = new MagickImage(imageStream))
        {
            image.Resize(new MagickGeometry(width, height));
            image.Quality = 100;
            using (var outputStream = new MemoryStream())
            {
                await image.WriteAsync(outputStream);
                outputStream.Position = 0;
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!ExtensionAllowed(fileExtension))
                {
                    return BaseDataResponse<UploadFileResponse>.Fail("Invalid file extension");
                }

                var filename = string.Concat(RandomGenerator.GenerateNewPassword(7) + fileExtension);
                var filePath = Path.Combine(filename);
                var path = Path.Combine(_environment.WebRootPath);
                EnsureDirectoryCreated(path);
                path = Path.Combine(_environment.WebRootPath, filePath);

                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await outputStream.CopyToAsync(fileStream);
                }

                var fileStorage = new FileStorage()
                {
                    FileName = filename,
                    FileLocation = filePath,
                    IsPublish = true,
                    IsDeleted = false
                };

                await _unitOfWork.FileStorages.AddAsync(fileStorage);
                await _unitOfWork.CommitAsync();

                return BaseDataResponse<UploadFileResponse>.Success(new UploadFileResponse(filePath));
            }
        }
    }
}
    
    public async Task<BaseDataResponse<BaseEntityCreatedResponse>> UploadFileIdAsync(IFormFile file)
    {

        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (!ExtensionAllowed(fileExtension))
            return null; 

        var filename = string.Concat(RandomGenerator.GenerateNewPassword(7) + fileExtension);
        var filePath = Path.Combine(BaseImagePath, filename);
        var path = Path.Combine(_environment.WebRootPath);
        EnsureDirectoryCreated(path);
        path = Path.Combine(_environment.WebRootPath, filePath);

        var fileStorage = new FileStorage()
        {
            FileName = filename,
            FileLocation = filePath,
            IsPublish = true,
            IsDeleted = false
        };
        
        await _unitOfWork.FileStorages.AddAsync(fileStorage);
        await _unitOfWork.CommitAsync();
       
        using (var stream = File.Create(path))
        {
            await file.CopyToAsync(stream);
        }
        return BaseDataResponse<BaseEntityCreatedResponse>.Success(new BaseEntityCreatedResponse(fileStorage.Id));
    }

    
    public async Task<bool> DeleteFileAsync(string filename)
    {
        var path = Path.Combine(BaseImagePath, filename);
        if (File.Exists(path) == true)
        {
            await Task.Run(() => File.Delete(path));
            return true;
        }
        return false;
    }

    public async Task<BaseDataResponse<BaseEntityCreatedResponse>> UpdateFileAsync(string oldFilename,  IFormFile newFile)
    {
        try
        {
            await DeleteFileAsync(oldFilename);
    
            var fileId = await UploadFileIdAsync(newFile);
    
            return fileId;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private static void EnsureDirectoryCreated(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    private bool ExtensionAllowed(string extension)
    {
        return AllowedFileExtensionsWithMiMeType.Keys.Any(p => p.Equals(extension));
    }

    public static string GetMimeType(string fileExtension)
    {
        return AllowedFileExtensionsWithMiMeType[fileExtension];
    }

    public static bool ValidateFileSize(IFormFile formFile, FileSizeType fileSizeType)
    {
        if (formFile == null) return false;
        var extension = Path.GetExtension(formFile.FileName).ToLower();
        if (extension == ".pdf" && fileSizeType == FileSizeType.Large)
        {
            return formFile.Length < AllowedPdfFileLength;
        }

        if (extension == ".mp4" && fileSizeType == FileSizeType.Large)
        {
            return formFile.Length < AllowedVideoFileLength;
        } 
        
        if ((extension == ".png" || extension == ".jpeg" || extension == ".jpg") && fileSizeType == FileSizeType.Large)
        {
            return formFile.Length < AllowedPhotoFileLength;
        }

        return true;
    }
}

public enum FileSizeType
{
    Small,
    Large
}