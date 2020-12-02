using SignalRMVCChat.Models.GapChatContext;
using System.Collections.Generic;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Compaign
{
    public class CompaignTemplateService : GenericService<Models.Compaign.CompaignTemplate>
    {
        public CompaignTemplateService() : base(null)
        {
        }


        public static void Init(GapChatContext db)
        {


            db.CompaignTemplates.Add(new Models.Compaign.CompaignTemplate
            {
                Html = CompaignTemplateSamples.Sample1,
                Name = "نمونه یک",
                IsSystemDefaultTemplate = true
            });

            db.CompaignTemplates.Add(new Models.Compaign.CompaignTemplate
            {
                Html = CompaignTemplateSamples.Sample2,
                Name = "نمونه 2",
                IsSystemDefaultTemplate = true
            });


            db.CompaignTemplates.Add(new Models.Compaign.CompaignTemplate
            {
                Html = CompaignTemplateSamples.Sample3,
                Name = "نمونه 3",
                IsSystemDefaultTemplate = true
            });


            db.CompaignTemplates.Add(new Models.Compaign.CompaignTemplate
            {
                Html = CompaignTemplateSamples.Sample4,
                Name = "نمونه 4",
                IsSystemDefaultTemplate = true
            });



            db.CompaignTemplates.Add(new Models.Compaign.CompaignTemplate
            {
                Html = CompaignTemplateSamples.Sample5,
                Name = "نمونه 5",
                IsSystemDefaultTemplate = true
            });



            db.CompaignTemplates.Add(new Models.Compaign.CompaignTemplate
            {
                Html = CompaignTemplateSamples.Sample6,
                Name = "نمونه 6",
                IsSystemDefaultTemplate = true
            });



            db.CompaignTemplates.Add(new Models.Compaign.CompaignTemplate
            {
                Html = CompaignTemplateSamples.Sample7,
                Name = "نمونه 7",
                IsSystemDefaultTemplate = true
            });

            db.SaveChanges();

        }
    }
}