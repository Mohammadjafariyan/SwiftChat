import React, {Component} from 'react';
import UsersSeparationForm from "./UsersSeparationForm";

class UsersSeparationLayout extends Component {
    render() {
        return (
            <div>
                
              <div className={'text-center'}>
                  <h3>صفحه تفکیک کاربران عضو</h3>

                  <p>در این بخش میتوانید بازدیدکنندگان سایت خود را با کاربران عضو تفکیک نموده
                      با اطلاعات و شناخت بهتر ، کیفیت پشتیبانی را افزایش دهید</p>


              </div>
                
                
                <UsersSeparationForm/>
                
            </div>
        );
    }
}

export default UsersSeparationLayout;