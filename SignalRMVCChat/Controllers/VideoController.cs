using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class VideoController : ApiController
    {
        public async Task<HttpResponseMessage> Get()
        {
            var video = VideoStreamService.Get(0);

            if (video==null)
            {
                return null;
            }
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(video.WriteToStream,
                new MediaTypeHeaderValue(@"video/webm"));

            return response;
        }
    }

    public class VideoStreamService
    {
        private static Dictionary<int, VideoStream> _videoStreams = new Dictionary<int, VideoStream>();

        public static void Add(int chatId, byte[] content)
        {
            if (_videoStreams.ContainsKey(chatId) == false)
            {
                _videoStreams.Add(0, new VideoStream(content));
            }
            else
            {
                _videoStreams[chatId].PushPart(content);
            }
        }

        public static VideoStream Get(int i)
        {
            if (_videoStreams.ContainsKey(i))
            {
                return _videoStreams[0];

            }

            return null;
        }
    }

    public class VideoStream
    {
        private bool _isFlushed = false;
        private Stream outputStream;

        public bool IsFlushed
        {
            get => _isFlushed;
            set
            {
                outputStream.Close();
                _isFlushed = value;
            }
        }

        public List<byte[]> Parts { get; set; }
        public Stack<byte[]> PartStack { get; set; }

        public VideoStream(byte[] content)
        {
            Parts = new List<byte[]>();
            PartStack = new Stack<byte[]>();
            this.Parts.Add(content);
            PartStack.Push(content);
        }

        public async Task WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            this.outputStream = outputStream;
            PartStack.Clear();
foreach (var part in Parts)
{
        PartStack.Push(part);
}
            

          await  Task.Factory.StartNew( async () =>
            {
                while (!_isFlushed)
                {
                    if (PartStack.Any())
                    {
                        var datacontent = PartStack.Pop();

                       // var bytesRead = UTF8Encoding.Default.GetBytes(datacontent);
                        await outputStream.WriteAsync(datacontent, 0, datacontent.Length);
                    }
                    else
                    {
                        outputStream.Close();
                        break;
                    }
                }
            });
         
        }

        public VideoStream PushPart(byte[] content)
        {
            this.Parts.Add(content);
            PartStack.Push(content);
            return this;
        }
    }
}