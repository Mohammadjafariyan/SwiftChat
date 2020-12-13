using System.Web.Mvc;
using SignalRMVCChat.Areas.Common.Service;

namespace SignalRMVCChat.Areas.Common.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class FileController:Controller
    {
        private readonly MyFileService _myFileService;

        public FileController(MyFileService myFileService)
        {
            _myFileService = myFileService;
        }


        public FileResult Download(int fileId)
        {
            var file= _myFileService.GetById(fileId);
            string fileName = file.Single.FileName;
            return File(file.Single.FileContent
                , System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        } 
    }
}