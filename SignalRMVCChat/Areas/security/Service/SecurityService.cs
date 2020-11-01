using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.DependencyInjection;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.Areas.security.Service
{
    public class SecurityService
    {
        private readonly AppUserService _appUserService;

        public SecurityService(AppUserService appUserService)
        {
            _appUserService = appUserService;
        }

     



        public AppUser SignInAsync(string userUserName, string userPassword)
        {

           var user= _appUserService.GetByUsername(userUserName,false);

           if (user==null)
           {
               throw new Exception("نام کاربری یا رمز عبور اشتباه است");
           }


           if (!string.Equals(user.Password,userPassword))
           {
               throw new Exception("نام کاربری یا رمز عبور اشتباه است");
           }
           
           /*if (!string.IsNullOrEmpty(user.Token))
           {
               return user;
           }*/
           
           string tokn= GenerateToken(user);

           user.Token = tokn;


           _appUserService.Save(user);

           return user;

        } 
        
        public static string GenerateToken(AppUser user)
        {
            var encrypt = EncryptionHelper.Encrypt($@"{user.Id}_{DateTime.Now}_{user.UserName}");
            return encrypt;
        }

        public static AppLoginViewModel ParseToken(string userToken)
        {
            var token = EncryptionHelper.Decrypt(userToken);
            int id=int.Parse(token.Split('_')[0]);


            var strr = token.Split('_')[1];
            DateTime date=DateTime.Parse(strr);
            string username=token.Split('_')[2];

            return new AppLoginViewModel
            {
                AppUserId = id,
                LoginDateTime = date,
                Username=username
            };
        }

        public void Logout()
        {
            var appUser = GetCurrentUser();

            if (appUser!=null)
            {
                appUser.Token = null;
                _appUserService.Save(appUser);
            }
        }

        public static AppUser GetCurrentUser()
        {

            var _appUserService = Injector.Inject<AppUserService>();

            var _currentRequestHolder = CurrentRequestSingleton.CurrentRequest;
            
           return _appUserService.GetById(
                            SecurityService.ParseToken(_currentRequestHolder.Token).AppUserId).Single;
        }
    }
    public enum MySignInStatus
    {
        Success,
        Failure
    }
    
    
    
    public class SecurityServiceTests
    {
        
        [Test]
        public void GenerateToken()
        {
            
            MyDependencyResolver.RegisterDependencies();
            var appUser = new AppUser
            {
                Id = 15
            };

          string token=  SecurityService.GenerateToken(appUser);

          var user= SecurityService.ParseToken(token);
          
          Assert.True(user.AppUserId==appUser.Id);
        }
    }
}