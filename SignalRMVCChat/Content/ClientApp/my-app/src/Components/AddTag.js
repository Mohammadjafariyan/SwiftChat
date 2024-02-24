import React, {Component, useState} from 'react';
import {InputGroup, Form, Card, Container} from "react-bootstrap";
import {MyCaller} from "./../Help/Socket";

import '../styles/myStyle.css';
import {CurrentUserInfo} from "../CurrentUserInfo";
import {Button} from "primereact/button";
import {InputText} from "primereact/inputtext";





AddTag.propTypes = {
    
};





function AddTag() {
    CurrentUserInfo.AddTag = this;

    const [tagName, setTagName] = useState('');

    let addNewTag =  (tagName) => {
        if (!tagName) {

            CurrentUserInfo.LayoutPage.showError('لطفا ابتدا عنوان برچسب را وارد نمایید');
            return;
        }

        CurrentUserInfo.LayoutPage.showMsg('در حال ثبت برچسب جدید')
        MyCaller.Send("NewTagAdd", {tagTitle: tagName});
    }
    
    return (
        <div>

            <div className="p-inputgroup flex-1">
                <InputGroup.Text onClick={() => {
                    addNewTag(tagName);
                    setTagName('');
                }} className='pointer' id="inputGroupPrepend">

                    <i className={'fa fa-plus'}></i>

                </InputGroup.Text>
                <Form.Control
                    type="text"
                    placeholder="عنوان برچسب را وارد نمایید"
                    aria-describedby="inputGroupPrepend"
                    name="عنوان تگ"
                    value={tagName}
                    onChange={(e) => setTagName(e.target.value)}
                />
                
            </div>
      

        </div>
    );
}

export default AddTag;
