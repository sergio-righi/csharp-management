using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Application.Classes;
using Domain.Entity;
using Domain.Entity.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers.Base
{
    [Route("arquivo/")]
    public partial class FileController : BaseController
    {
        private readonly IFileService FileService;

        public FileController(IFileService fileService)
        {
            FileService = fileService;
        }

        //public async Task<IActionResult> Attach()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> List()
        //{
        //    return View();
        //}

        protected async virtual Task<IActionResult> Download(string key)
        {
            var model = await FileService.Find(key, null, false);

            if (model == null)
            {
                return Content(Message.FileNotFound);
            }

            var path = Path.Combine(
                       FileManagement.Write,
                       FileManagement.GetExtensionDirectory(model.Extension),
                       model.NotGeneratedName);

            var memory = new MemoryStream();

            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, GetContentType(path), model.NotName);
        }

        protected async virtual Task<bool> Replace(IFormFile httpFile, Domain.Entity.File file, string directory)
        {
            try
            {
                await Delete(file.Key, directory);

                await Upload(httpFile, file.Key, directory, file.GeneratedName);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// the file will be logical deleted
        /// </summary>
        protected async virtual Task<bool> Delete(string key)
        {
            var model = await FileService.Find(key, null, false);

            if (model != null)
            {
                model.IsDeleted = true;
                model.IsActivated = false;
                model.UpdatedBy = base.GetCurrentUser();

                await FileService.Update(model);

                return true;
            }

            return false;
        }

        /// <summary>
        /// the file will be deleted from the directory
        /// </summary>
        protected async virtual Task<Domain.Entity.File> Delete(string key, string folder)
        {
            var model = await FileService.Find(key, null, false);

            if (model != null)
            {
                var path = Path.Combine(
                           FileManagement.Write,
                           string.IsNullOrWhiteSpace(folder) ? FileManagement.GetExtensionDirectory(model.Extension) : folder,
                           model.GeneratedName + model.Extension);

                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                catch
                {
                    return null;
                }
            }

            return model;
        }

        /// <summary>
        /// the file will be deleted from the database
        /// </summary>
        protected async virtual Task<bool> Delete(Domain.Entity.File file)
        {
            try
            {
                await FileService.Delete(file);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        protected async virtual Task<Domain.Entity.File> Upload(IFormFile file, string key, string folder, string fileName)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return null;
                }

                var extension = GetExtension(file.FileName);
                var generatedName = string.IsNullOrWhiteSpace(fileName) ? DateTime.Now.Ticks + "" : fileName;

                var path = Path.Combine(
                           FileManagement.Write,
                           string.IsNullOrWhiteSpace(folder) ? FileManagement.GetExtensionDirectory(extension) : folder,
                           generatedName + extension);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    var criptografy = new MD5CryptoServiceProvider();

                    if (string.IsNullOrWhiteSpace(key))
                    {
                        var model = new Domain.Entity.File()
                        {
                            IsActivated = true,
                            Size = file.Length,
                            Extension = extension,
                            GeneratedName = generatedName,
                            CreatedBy = base.GetCurrentUser(),
                            UpdatedBy = base.GetCurrentUser(),
                            Name = Path.GetFileNameWithoutExtension(file.FileName),
                            ExtensionId = FileManagement.GetExtensionCategory(extension),
                            Key = BitConverter.ToString(criptografy.ComputeHash(stream)).Replace("-", string.Empty)
                        };

                        return await FileService.Insert(model);
                    }
                    else
                    {
                        var model = await FileService.Find(key, null, false);

                        if (model != null)
                        {
                            model.Size = file.Length;
                            model.Extension = extension;
                            model.UpdatedBy = base.GetCurrentUser();
                            model.Name = Path.GetFileNameWithoutExtension(file.FileName);
                            model.ExtensionId = FileManagement.GetExtensionCategory(extension);
                            model.Key = BitConverter.ToString(criptografy.ComputeHash(stream)).Replace("-", string.Empty);

                            await FileService.Update(model);

                            return model;
                        }
                    }
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return null;
        }

        protected async virtual Task<int?> ManageFile(int? fileId, GFormFile voucher, string directory = null)
        {
            if (voucher != null)
            {
                var oldFile = await FileService.Find(fileId);

                if (voucher.IsSetDefault)
                {
                    directory ??= FileManagement.GetExtensionDirectory(oldFile.Extension);

                    if (oldFile != null)
                    {
                        if (await Delete(oldFile.Key, directory) != null)
                        {
                            await Delete(oldFile.Key);
                        }

                        return null;
                    }
                }
                else
                {
                    if (voucher.HttpFile != null)
                    {
                        if (oldFile != null)
                        {
                            await Replace(voucher.HttpFile, oldFile, directory);

                            return oldFile?.Id;
                        }
                        else
                        {
                            var newFile = await Upload(voucher.HttpFile, null, directory, null);

                            return newFile?.Id;
                        }
                    }
                }
            }

            return fileId;
        }

        protected async Task<IEnumerable<string>> ListExtension(int personId, bool? activated, bool deleted)
        {
            return await FileService.ListExtension(personId: personId, activated: activated, deleted: deleted);
        }

        private static string GetExtension(string path)
        {
            return Path.GetExtension(path).ToLowerInvariant();
        }

        private static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var extension = GetExtension(path);
            return types[extension];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}