import React, { Component } from 'react'
import { useState } from 'react';
import { MyCaller,CurrentUserInfo } from './../Help/Socket';
import { cookieManager } from './../Help/CookieManager';
import { MyGlobal } from '../Help/MyGlobal';

export default class LoginPage extends Component {
    constructor(arg){
        super(arg);

        this.state={};
        CurrentUserInfo.LoginPage=this;
    }


    adminLoginCallback(res) {
        
        if (res.Type === 0) {
            var token = res.Token;

            cookieManager.setItem('adminToken', token);

            CurrentUserInfo.targetId = res.Content.Id;
            CurrentUserInfo.targetName = res.Content.Name;


            CurrentUserInfo.LayoutPage.setState({isLogin:true,isClearCookie:false});
            
            
            window.location.reload();

        } else {

            this.setState({err:'نام کاربری یا رمز عبور صحیح نیست'});
        }
        
    }

    

    render() {

        return (
            <div className="container h-100 row align-items-center">



     <div className="col-md-4 ">
     </div>
     <div className="col-md-6  mt-md-3 ">

{this.state.err && <div className="alert alert-danger">{this.state.err}</div>}

  <div className="form-group">
    <label for="exampleInputEmail1">نام کاربری</label>
    <input id={'username'} type="text" className="form-control" 
         aria-describedby="emailHelp" placeholder="نام کاربری را وارد نمایید"
             onChange={(e)=>
                this.setState({username:e.target.value})
             }
         />

    <small id="emailHelp" className="form-text text-muted">نام کاربری و رمز عبور از پیشخوان /  قسمت تعریف ادمین قابل دسترس است</small>
  </div>
  <div className="form-group">
    <label for="exampleInputPassword1">رمز عبور</label>
    <input id={'password'}  onChange={(e)=>
                this.setState({password:e.target.value})
             } type="password" className="form-control" placeholder="رمز عبور"/>
  </div>
  <button onClick={()=>{
      this.login();
  }} type="submit" id={'login'}  className="btn btn-primary">ورود </button>
</div>
  <div className="col-md-3 ">
     </div>
            </div>
        )
    }


    login(){

        if(!this.state.username || 
            !this.state.password
            )
            {
                this.setState({err:'اطلاعات صحیح وارد نشده است'});
                return;
            }


            if(MyGlobal.isTestingEnvirement){
                this.adminLoginCallback({
                    Type:0,
                    Token:"sdlfjslkdfj",
                    Content:{},
                });
            }


        MyCaller.Send("AdminLogin", {username: this.state.username, password:this.state.password});

    }
}
