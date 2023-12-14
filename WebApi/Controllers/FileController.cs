using Core.Model.ResponseModels;
using Core.Services;
using Core.ViewModels.FileStorage;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using WebApi.Controllers.Common;

namespace WebApi.Controllers;

public class FileController : ApiController
{
    private readonly FileService _fileService;
    private readonly ImageCompressor _imageCompressor;

    public FileController(FileService fileService, ImageCompressor imageCompressor)
    {
        _fileService = fileService;
        _imageCompressor = imageCompressor;
    }
    

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<BaseDataResponse<UploadFileResponse>> UploadFile(IFormFile file)
    {
        var response = await _fileService.UploadFileUrlAsync(file);
        return Response(response);
    }
    
    [HttpPost]
    public async Task<BaseDataResponse<UploadFileResponse>> UploadAndSaveResizedImage([FromForm]Dto dto)
    
    {
        var response = await _fileService.UploadAndSaveResizedImage(dto.File,dto.Width, dto.Height);
        return Response(response);
    }
}

public class Dto
{
    public IFormFile File { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}