using SignalRMVCChat.Models;
using SignalRMVCChat.TelegramBot.BotBehaviour;
using SignalRMVCChat.TelegramBot.CustomerBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SignalRMVCChat.TelegramBot
{
    public class BotHelp
    {
        public static void Init(TelegramBotClient botClient
            , IBotBehaviour behaviour)
        {

            botClient.OnMessage += behaviour.Bot_OnMessage;
            botClient.OnCallbackQuery += behaviour.OnCallbackQuery;
            botClient.OnInlineQuery += behaviour.OnInlineQuery;
            botClient.OnInlineResultChosen += behaviour.OnInlineResultChosen;
            botClient.OnMessageEdited += behaviour.OnMessageEdited;
            botClient.OnReceiveError += behaviour.OnReceiveError;
            botClient.OnUpdate += behaviour.OnUpdate;
            botClient.OnReceiveGeneralError += behaviour.OnReceiveGeneralError;

        }

        internal static dynamic ConvertContentToHtml(Models.Chat chat)
        {
            return "hi";
           // throw new NotImplementedException();
        }

        internal static string ConvertMessageToHtml(Message message, BotViewModel botViewModel)
        {
            string text = message.Text ?? message.Caption;
            switch (message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Unknown:

                    break;
                case Telegram.Bot.Types.Enums.MessageType.Text:
                    return $@"<p>{text}</p>";
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Photo:

                    string img = BotHelp.GetPhoto(message.Photo, botViewModel);


                    return $@"<p>{text}</p>
                              <img src='/Content/upload/{img}' width='150' height='150'>";
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Audio:
                    string audio = BotHelp.GetFile(message.Audio?.FileId, botViewModel);

                    var ext = Path.GetExtension(audio);

                    return $@"

                <audio controls>
                <source src=""/Content/upload/{audio}"" type=""audio/{ext}"" >
                {text}
                 </audio>";
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    string video = BotHelp.GetFile(message.Video?.FileId, botViewModel);

                    var ext2 = Path.GetExtension(video);

                    return $@"
<video width=""320"" height=""240"" controls>
  <source src=""/Content/upload/{video}"" type=""video/{ext2}"">
 {text}
</video>
             ";
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Voice:
                    string voice = BotHelp.GetFile(message.Audio?.FileId, botViewModel);

                    var ext3 = Path.GetExtension(voice);

                    return $@"

                <audio controls>
                <source src=""/Content/upload/{voice}"" type=""audio/{ext3}"" >
                {text}
                 </audio>";
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Document:

                    if (message?.Document?.MimeType?.Contains("audio") == true)
                    {
                        string _audio = BotHelp.
                            GetFile(message?.Document?.FileId, botViewModel);

                        return $@"
                            <audio controls>
                            <source src=""/Content/upload/{_audio}"" type=""{message?.Document?.MimeType}"" >
                            {text}
                             </audio>";
                    }
                    if (message?.Document?.MimeType?.Contains("video") == true)
                    {
                        string _audio = BotHelp.
                            GetFile(message?.Document?.FileId, botViewModel);

                        return $@"

                <video width=""320"" height=""240"" controls>
                <source src=""/Content/upload/{_audio}"" type=""{message?.Document?.MimeType}"" >
                {text}
                 </video>";
                    }

                    break;
                case Telegram.Bot.Types.Enums.MessageType.Sticker:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Location:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Contact:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Venue:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Game:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.VideoNote:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Invoice:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.SuccessfulPayment:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.WebsiteConnected:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.ChatMemberLeft:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.ChatTitleChanged:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.ChatPhotoChanged:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.MessagePinned:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.ChatPhotoDeleted:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.GroupCreated:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.SupergroupCreated:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.ChannelCreated:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.MigratedToSupergroup:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.MigratedFromGroup:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Animation:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Poll:
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Dice:
                    break;
                default:
                    break;
            }

            return @"<p style=""color:red"">پیغام پشتیبانی نمی شود</p>"+$@"<b>{message.Type }</b";
        }




        private static string GetFile(string fileId,
            BotViewModel botViewModel)
        {
            var pic = botViewModel.botClient.
                 GetFileAsync(fileId).GetAwaiter().GetResult();

            var ext = Path.GetExtension(pic.FilePath);




            var filePath = MySpecificGlobal.GenerateUploadFileNameUniqPath();

            filePath += ext;



            using (var saveImageStream = System.IO.File.Open(filePath, FileMode.Create))
            {
                botViewModel.botClient
                   .DownloadFileAsync(pic.FilePath, saveImageStream)
               .GetAwaiter().GetResult();
            }

            var fileName = Path.GetFileName(filePath);

            return fileName;
        }
        private static string GetPhoto(PhotoSize[] photo, BotViewModel botViewModel)
        {

            string fileName = GetFile(photo.LastOrDefault().FileId, botViewModel);
            return fileName;
        }
    }
}