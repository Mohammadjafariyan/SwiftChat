import React, {Component} from 'react';
import BaseCrudLayout from "../CRUD/BaseCrudLayout";
import {CurrentUserInfo} from "../Help/Socket";
import {Alert, Card} from "react-bootstrap";
import CompaignSave from "./Manage/CompaignSave";
import CompaignFilter from "./Filter/CompaignFilter";
import BaseIndex from "../CRUD/BaseIndex";
import CompaignIndex from "./Override/CompaignIndex";

class CompaignLayout extends BaseCrudLayout {
    state = {
        get: 'GetCompaignList',
        save: 'CompaignSave',
        delete: 'DeleteCompaign',
        setIsEnabled: 'SetIsEnabledCompaign',
        saveDraft: 'CompaignSave',
        menuCols: 5,
        bodyCols: 7
    };

    constructor(prp) {
        super(prp);

        CurrentUserInfo.CompaignLayout = this;

        console.log('hi');
    }

    
    render(): * {
        return <br>
        
        
            <CompaignFilter></CompaignFilter>
            <br/>
            <CompaignIndex menuCols={this.state.menuCols}
                       bodyCols={this.state.bodyCols}

                       parent={this} {...this.props} {...this.state}
                       get={this.state.get}
                       save={this.state.save}
                       delete={this.state.delete}
                       setIsEnabled={this.state.setIsEnabled}
                       RenderWelcome={this.RenderWelcome}
                       saveDraft={this.state.saveDraft}>


                {this.RenderForm()}


            </CompaignIndex>
        </>;
    }

    RenderWelcome() {
        return <Card>
            <Card.Body>
                <Card.Text>
                    <Alert variant={'warning'}>
                        <div className="cap-plugin-content-box-inner">
                            <p className="cap-font-sans-regular"><span
                                className="cap-font-sans-semibold">
                                
                               کمپین های خودکار برای ارسال ایمیل به کاربران شما براساس فیلترهای محرک رویداد از پیش تعریف شده استفاده می شوند. یک ایمیل بنویسید ، برنامه ریزی کنید که در کدام شرایط فیلتر باید برای یک کاربر خاص اجرا شود و آن را فعال کنید.
                                <b> کمپین های اتوماتیک ، </b>
                                <b>  کمپین های دستی </b>

                            </span>
                            </p>
                            <p className="cap-font-sans-regular">
                                کمپین های خودکار چه کاری انجام می دهند؟

                                کمپین های خودکار ایمیل ها یا پیام های گپ از پیش تعریف شده ای هستند که براساس رویدادهای
                                کاربر براساس هر کاربر ارسال می شوند. این ابزار مناسب جذب مشتری است. </p>


                            <ol className="cap-font-sans-semibold help-list" style={{textAlign: 'right'}}>
                                <li>تعریف کمپین جدید</li>
                                <li>انتخاب اتوماتیک یا دستی</li>
                                <li>تعریف قالب و متن ایمیل یا پیغام</li>
                                <li>ارسال زمانبندی شده یا دستی</li>
                            </ol>

                            <div className="help-screen-wrap">
                                <div className="help-screen"></div>
                            </div>
                        </div>

                        <hr/>
                        در این بخش کمپین های (ارسال ایمیل یا پیغام) با سیاست خاصی به کاربران تعریف و ارسال میشود
                    </Alert>
                </Card.Text>
            </Card.Body>
        </Card>
    }

    RenderForm() {
        return <>{this.state.selected && <CompaignSave parent={this} selected={this.state.selected}/>}</>
    }
}

export default CompaignLayout;