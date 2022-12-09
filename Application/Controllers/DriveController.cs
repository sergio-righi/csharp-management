
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Models;
using Application.Models.Drive;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("arquivo/")]
    public partial class DriveController : FileController
    {
        private readonly IFileService FileService;
        private readonly IFolderService FolderService;
        private readonly IFileFolderService FileFolderService;

        public int? ReferenceId
        {
            get { return HttpContext.Session.Get<int?>(AppConfig.Session.Folder); }
            set { HttpContext.Session.Set(AppConfig.Session.Folder, value); }
        }

        public DriveController(IFileService fileService,
                               IFolderService folderService,
                               IFileFolderService fileFolderService) : base(fileService)
        {
            FileService = fileService;
            FolderService = folderService;
            FileFolderService = fileFolderService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<File> filter)
        {
            if (hasSession())
            {
                int personId = base.GetCurrentUser();

                var driveFilder = new DriveFilter();

                driveFilder.Pagination = await FileService.List(name: filter.GetName(),
                                                           category: filter.SituationId,
                                                           personId: personId,
                                                           folderId: filter.ReferenceId,
                                                           activated: filter.IsActivated(),
                                                           deleted: false,
                                                           order: GetOrder(filter.Sort),
                                                           direction: filter.Direction,
                                                           page: filter.Page,
                                                           count: filter.Count);

                driveFilder.Folders = await FolderService.GetQueryable(x => x.FolderId == filter.ReferenceId && x.CreatedBy == personId);

                driveFilder.Filter = filter;

                if (filter.ReferenceId.HasValue && filter.ReferenceId.IsPositive())
                {
                    driveFilder.FolderName = (await FolderService.Find(filter.ReferenceId))?.Name ?? string.Empty;
                }

                ReferenceId = filter.ReferenceId;

                return View(driveFilder);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        public async virtual Task<int> NewFile(IFormFile file, string key)
        {
            try
            {
                var result = await base.Upload(file, null, null, null);

                if (result != null)
                {
                    if (ReferenceId.IsPositive())
                    {
                        var fileFolder = new FileFolder()
                        {
                            FileId = result.Id,
                            FolderId = (int)ReferenceId,
                            CreatedBy = base.GetCurrentUser(),
                            UpdatedBy = base.GetCurrentUser()
                        };

                        await FileFolderService.Insert(fileFolder);
                    }

                    return result.Id;
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return -1;
        }

        [HttpPost]
        public async virtual Task<int> NewFolder(string name, int id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    var folder = new Folder()
                    {
                        Name = name,
                        CreatedBy = base.GetCurrentUser(),
                        UpdatedBy = base.GetCurrentUser(),
                        FolderId = id.IsPositive() ? id : (int?)null
                    };

                    var result = await FolderService.Insert(folder);

                    if (result != null)
                    {
                        return result.Id;
                    }
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return -1;
        }

        [HttpPost]
        public async virtual Task<File> GetInfo(int id)
        {
            try
            {
                if (id.IsPositive())
                {
                    var file = await FileService.Find(id);

                    if (file != null)
                    {
                        return file;
                    }
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return null;
        }

        [HttpPost]
        public async virtual Task<int> MoveFile(int fileId, int folderId)
        {
            try
            {
                if (fileId.IsPositive())
                {
                    var fileFolder = (await FileFolderService.GetQueryable(x => x.FileId == fileId)).FirstOrDefault();

                    if (fileFolder != null && fileFolder.FileId != folderId)
                    {
                        if (folderId.IsPositive())
                        {
                            fileFolder.FolderId = folderId;
                            fileFolder.UpdatedBy = base.GetCurrentUser();

                            await FileFolderService.Update(fileFolder);
                        }
                        else
                        {
                            await FileFolderService.Delete(fileFolder.Id);
                        }

                        return fileId;
                    }
                    else
                    {
                        if (folderId.IsPositive())
                        {
                            fileFolder = new FileFolder()
                            {
                                FileId = fileId,
                                FolderId = folderId,
                                CreatedBy = base.GetCurrentUser(),
                                UpdatedBy = base.GetCurrentUser()
                            };

                            await FileFolderService.Insert(fileFolder);

                            return fileId;
                        }
                    }
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return -1;
        }

        [HttpPost]
        public async virtual Task<int> MoveFolder(int from, int to)
        {
            try
            {
                if (!from.Equals(to))
                {
                    var folder = await FolderService.Find(from);

                    if (folder != null)
                    {
                        folder.UpdatedBy = base.GetCurrentUser();
                        folder.FolderId = !to.IsPositive() ? (int?)null : to;

                        await FolderService.Update(folder);

                        return from;
                    }
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return -1;
        }

        [Route("download/{key}")]
        public async virtual Task<IActionResult> Download(string key)
        {
            return await base.Download(key);
        }

        [HttpPost]
        public async virtual Task<string> RenameFile(string name, int id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(name) && id.IsPositive())
                {
                    var file = await FileService.Find(id);

                    if (file != null)
                    {
                        file.Name = name;
                        file.UpdatedBy = base.GetCurrentUser();

                        await FileService.Update(file);

                        return name;
                    }
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return string.Empty;
        }

        [HttpPost]
        public async virtual Task<string> RenameFolder(string name, int id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(name) && id.IsPositive())
                {
                    var folder = await FolderService.Find(id);

                    if (folder != null)
                    {
                        folder.Name = name;
                        folder.UpdatedBy = base.GetCurrentUser();

                        await FolderService.Update(folder);

                        return name;
                    }
                }
            }
            catch
            {
                throw new ApplicationException();
            }

            return string.Empty;
        }

        private static Expression<Func<File, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.UpdatedAt;
                case 2:
                    return x => x.Name;
            }

            return null;
        }
    }
}