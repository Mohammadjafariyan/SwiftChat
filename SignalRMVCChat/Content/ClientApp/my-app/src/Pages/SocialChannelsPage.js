import React, {Component} from 'react';


import {Input} from "../Components/FormInputs";

import { MyCaller, CurrentUserInfo } from './../Help/Socket';
import {Row} from "react-bootstrap";

class SocialChannelsPage extends Component {


    constructor(props, context) {
        super(props, context);
        
        this.state={};
        CurrentUserInfo.SocialChannelsPage=this;
    }

    json1=`{
                                "searchTerm":"متنی که کاربر جستجو می کند"
                            }`;
    json2=`{
                                "array": [
                                     {title:"عنوان لینک",link:"http://..."}
                                ],

                                "emptyText": "پیغام در صورت خالی بودن"
                            }`;


    
    componentDidMount() {
        MyCaller.Send('GetSocialChannelsInfo')
        
    }

    getSocialChannelsInfoCallback(res){
        if (!res || !res.Content)
        {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return 
        }
       
       
        this.setState({email:res.Content.email,
            telegram:res.Content.telegram,
            whatsapp:res.Content.whatsapp,
            helpDeskApi:res.Content.helpDeskApi,
            helpDeskUrlLink:res.Content.helpDeskUrlLink,
        })
    }


    saveSocialChannelsInfoCallback(res){
        CurrentUserInfo.LayoutPage.showMsg('با موفقیت ذخیره شد')

    }
    save(){
        
        
        CurrentUserInfo.LayoutPage.showMsg('در حال ذخیره')
        
        MyCaller.Send('SaveSocialChannelsInfo',{
            email:this.state.email,
            telegram:this.state.telegram,
            whatsapp:this.state.whatsapp,
            helpDeskApi:this.state.helpDeskApi,
            helpDeskUrlLink:this.state.helpDeskUrlLink,
        })
        
    }
    render() {
        return (
            <>
                <Row style={{direction:'ltr'}}>
                  
            <div className={'form col-md-5 offset-md-1'}>
                
                <div >
                    
                    <h5>هر کدام از فیلد ها خالی باشد به معنای عدم نمایش آن است</h5>
<hr/>
                    <div className={'form-group'}>
                        <Input value={this.state.email}
                               onChange={(e)=>this.setState({email:e.target.value})}

                               type={'text'} name={'ایمیل'} placeholder={'mohammad.jafariyan7@gmail.com'}></Input>
                    </div>
                    <div className={'form-group'}>
                        <Input value={this.state.telegram}
                               onChange={(e)=>this.setState({telegram:e.target.value})}
                               type={'text'} name={'تلگرام'} placeholder={'https://t.me/asharsoft'}></Input>
                    </div>
                    <div className={'form-group'}>
                        <Input value={this.state.whatsapp} type={'text'}
                               onChange={(e)=>this.setState({whatsapp:e.target.value})}
                               name={'واتساپ'} placeholder={'https://wa.me/+989148980692'}></Input>
                    </div>
                    <hr/>
                    <div className={'form-group'}>
                        <Input value={this.state.helpDeskApi} type={'text'} 
                               name={'آدرس Api فراخوانی HelpDesk'}
                               placeholder={'/HelpDeskApi/Search'}
                        
                        onChange={(e)=>this.setState({helpDeskApi:e.target.value})}
                        
                        ></Input>
                        <hr/>

                        <h5 style={{textAlign:'right'}}>نمونه درخواست اطلاعات از پلاگین</h5>
                        
                        <code>
                            {this.json1}
                        </code>

                        <hr/>
                        <h5 style={{textAlign:'right'}}>نمونه ساختار اطلاعات مورد انتظار پلاگین</h5>
                        <code>
                            {this.json2}
                        </code>
                    </div>
                    
                </div>

                <hr/>
                <div className={'form-group'}>
                    <Input value={this.state.helpDeskUrlLink}


                           onChange={(e)=>this.setState({helpDeskUrlLink:e.target.value})}

                           type={'text'} name={'آدرس helpdesk در سایت شما'} placeholder={'/Home/Docs'}></Input>

                    <small dir={'rtl'}>اگر تنها ادرس helpdesk وارد شده باشد و آدرس Api وارد نشود بصورت لینک نمایش داده می شود</small>
                </div>

                <div className={'form-group'}>

                    <button className={'btn btn-primary'} onClick={()=>{
                        this.save();
                    }}>ذخیره</button>

                    <button className={'btn btn-info'} onClick={()=>{
                        this.factoryReset();
                    }}>بازگشت به کارخانه</button>

                </div>

                </div>
                    <div className={' col-md-4 offset-md-1'} style={{borderRight:'1px solid #ddd'}}>
                        <h6 style={{textAlign:'right'}} >دکمه های زیر</h6>
                        <img src={`${process.env.PUBLIC_URL}/Content/HelpImages/circles.PNG`} style={{width:'80%'}}/>
                        <hr/>
                        <h6 style={{textAlign:'right'}}>help desk</h6>
                        <img src={`${process.env.PUBLIC_URL}/Content/HelpImages/helpDesk.PNG`} style={{width:'80%'}}/>
                        <hr/>
                        <h6 style={{textAlign:'right'}}>باز شدن آدرس در همان صفحه</h6>
                        <img src={`${process.env.PUBLIC_URL}/Content/HelpImages/circleHelpDesk.PNG`}  style={{width:'80%'}} />

                    </div>
                </Row>
            </>   );
    }

    factoryReset() {



        this.setState({
            helpDeskApi:'/HelpDeskApi/Search',
            helpDeskUrlLink:'mohammad.jafariyan7@gmail.com',
        });
    }
}

export default SocialChannelsPage;





