using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class MySocketService : GenericService<MySocket>
    {
        public MySocketService() : base(null)
        {
        }

        public MyEntityResponse<int> SaveWithAttach(MySocket model)
        {
            if (MyGlobal.IsUnitTestEnvirement)
            {
                return base.Save(model);
            }

            /*var customerProviderService = DependencyInjection.Injector.Inject<CustomerProviderService>();
            var myAccountProviderService = DependencyInjection.Injector.Inject<MyAccountProviderService>();
            if (model.IsCustomerOrAdmin == MySocketUserType.Admin)
            {
                Impl.AttachUpdate(model, (socket, entry) => { entry.Reference(e => e.MyAccount).Load(); });
            }
            else
            {
                Impl.AttachUpdate(model, (socket, entry) => { entry.Reference(e => e.Customer).Load(); });
            }

            if (model.AdminWebsiteId.HasValue)
            {
                Impl.AttachUpdate(model, (socket, entry) => { entry.Reference(e => e.AdminWebsite).Load(); });
            }

            if (model.CustomerWebsiteId.HasValue)
            {
                Impl.AttachUpdate(model, (socket, entry) => { entry.Reference(e => e.CustomerWebsite).Load(); });
            }*/


            model.Customer = null;
            model.CustomerWebsite = null;
            model.AdminWebsite = null;
            model.MyAccount = null;

            return base.Save(model);
        }

        /*public void Update(MySocket model)
        {
            Update(model, (socket, mySocket, db) =>
            {
                var o = MySpecificGlobal.Clone(mySocket);
                db.Entry(o).Property(e => e._myConnectionInfo).IsModified = true;
                return 1;
            });
        }*/
        public void Update(MySocket con)
        {
            Impl.AttachUpdate(con, (socket, entry) =>
            {
                entry.Property(p => p._myConnectionInfo).IsModified = true;
                entry.Property(p => p.Token).IsModified = true;
            });
            Impl.SaveChanges();
        }
    }
}